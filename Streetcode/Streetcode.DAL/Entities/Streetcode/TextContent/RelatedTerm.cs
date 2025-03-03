﻿using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Streetcode.TextContent
{
    [Table("related_terms", Schema = "streetcode")]
    public class RelatedTerm : IEntityId<int>
    {
        public int Id { get; set; }

        public string? Word { get; set; } = string.Empty;

        public int TermId { get; set; }

        public Term? Term { get; set; }
    }
}
