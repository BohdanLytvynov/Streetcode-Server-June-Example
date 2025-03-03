﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.BLL.MediatR.Account.Login;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Entities.Users;
using Streetcode.BLL.Services.CookieService.Interfaces;
using Streetcode.BLL.Services.Tokens;

namespace Streetcode.XUnitTest.MediatRTests.Account.Login
{
    public class LoginUserHandlerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly LoginUserHandler _handler;
        private readonly Mock<ICookieService> _ccokieServiceMock;
        private readonly Mock<TokensConfiguration> _tokensConfigurationMock;

        public LoginUserHandlerTests()
        {
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var logger = new Mock<ILogger<SignInManager<User>>>();
            var schemes = new Mock<IAuthenticationSchemeProvider>();
            var confirmation = new Mock<IUserConfirmation<User>>();

            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                contextAccessor.Object,
                userPrincipalFactory.Object,
                options.Object,
                logger.Object,
                schemes.Object,
                confirmation.Object);

            _tokenServiceMock = new Mock<ITokenService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILoggerService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _ccokieServiceMock = new Mock<ICookieService>();
            _tokensConfigurationMock = new Mock<TokensConfiguration>();
            
            _handler = new LoginUserHandler(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _tokenServiceMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _httpContextAccessor.Object,
                _ccokieServiceMock.Object,
                _tokensConfigurationMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsError_WhenUserNotFound()
        {
            // Arrange
            var command = new LoginUserCommand(new UserLoginDTO { Username = "nonexistent", Password = "password" });
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))!.ReturnsAsync((User)null!);
            var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.InvalidUsernameOrPassword, command);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(result.IsFailed);
                Assert.Equal(errorMsg, result.Errors[0].Message);
            });
        }

        [Fact]
        public async Task Handle_ReturnsError_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new User { UserName = "SuperAdmin" };
            var command = new LoginUserCommand(new UserLoginDTO { Username = "SuperAdmin", Password = "wrongpassword" });
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, command.LoginUser.Password, false))
                .ReturnsAsync(SignInResult.Failed);
            var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.InvalidUsernameOrPassword, command);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(result.IsFailed);
                Assert.Equal(errorMsg, result.Errors[0].Message);
            });
        }

        [Fact]
        public async Task Handle_ReturnsError_WhenMappingFails()
        {
            // Arrange
            var user = new User { UserName = "SuperAdmin" };
            var command = new LoginUserCommand(new UserLoginDTO { Username = "SuperAdmin", Password = "password" });
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, command.LoginUser.Password, false))
                .ReturnsAsync(SignInResult.Success);
            _mapperMock.Setup(x => x.Map<UserDTO>(user)).Returns((UserDTO)null!);
            var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.FailToMap, command);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(result.IsFailed);
                Assert.Equal(errorMsg, result.Errors[0].Message);
            });
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenLoginIsSuccessful()
        {
            // Arrange
            var user = new User { UserName = "SuperAdmin" };
            var tokens = new TokenResponseDTO { AccessToken = "access_token", RefreshToken = new RefreshTokenDTO { Token = "refresh_token" } };
            var userDto = new UserDTO { Username = "SuperAdmin" };
            var command = new LoginUserCommand(new UserLoginDTO { Username = "SuperAdmin", Password = "password" });

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, command.LoginUser.Password, false))
                .ReturnsAsync(SignInResult.Success);
            _mapperMock.Setup(x => x.Map<UserDTO>(user)).Returns(userDto);

            var httpContext = new Mock<HttpContext>();
            var response = new Mock<HttpResponse>();
            var cookies = new Mock<IResponseCookies>();

            response.Setup(x => x.Cookies).Returns(cookies.Object);
            httpContext.Setup(x => x.Response).Returns(response.Object);
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            _tokenServiceMock.Setup(x => x.GenerateTokens(It.IsAny<User>())).ReturnsAsync(
                tokens);
            
            _tokenServiceMock.Setup(x => x.SetRefreshToken(It.IsAny<RefreshTokenDTO>(), It.IsAny<User>()))
                .Callback<RefreshTokenDTO, User>((refTokenDto, user) => { user.RefreshTokens.Add(new DAL.Entities.Users.RefreshToken() { Token = refTokenDto.Token }); });

            _ccokieServiceMock.Setup(x => x.AppendCookiesToResponseAsync(It.IsAny<HttpResponse>()));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(result.IsSuccess);
                Assert.Equal(userDto, result.Value);
            });
        }
    }
}
