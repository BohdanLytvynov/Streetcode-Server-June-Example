﻿using FluentResults;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Newss.Create
{
    public record CreateNewsCommand(NewsDTO newNews) : IValidatableRequest<Result<NewsDTO>>;
}
