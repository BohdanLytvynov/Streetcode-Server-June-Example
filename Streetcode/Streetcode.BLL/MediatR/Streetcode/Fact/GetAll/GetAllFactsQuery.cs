﻿using FluentResults;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Services.CacheService;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;

public record GetAllFactsQuery : ICachibleQueryBehavior<Result<IEnumerable<FactDto>>>
{
    public Result<IEnumerable<FactDto>>? CachedResponse { get; set; }
}
