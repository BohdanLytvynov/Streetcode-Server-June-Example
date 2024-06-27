﻿using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Base;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Entities.Streetcode;

public class StreetcodeArt : IEntity
{
    public int Index { get; set; }

    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; }

    public int ArtId { get; set; }

    public Art? Art { get; set; }
}