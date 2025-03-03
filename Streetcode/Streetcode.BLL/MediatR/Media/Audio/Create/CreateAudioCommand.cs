﻿using FluentResults;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Media.Audio.Create;

public record CreateAudioCommand(AudioFileBaseCreateDTO Audio) : IValidatableRequest<Result<AudioDTO>>;
