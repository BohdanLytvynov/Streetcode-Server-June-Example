﻿using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline.Update;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Update
{
    public record UpdateTimelineItemCommand(UpdateTimelineItemDTO updatedTimeLine) : IRequest<Result<UpdateTimelineItemDTO>>;
}
