using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Likes;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Streetcode;

public class StreetcodeDTO
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Title { get; set; } = string.Empty;
    public string DateString { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string TransliterationUrl { get; set; } = string.Empty;
    public StreetcodeStatus Status { get; set; }
    public DateTime EventStartOrPersonBirthDate { get; set; }
    public DateTime? EventEndOrPersonDeathDate { get; set; }
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<StreetcodeTagDTO>? Tags { get; set; }
    public string Teaser { get; set; } = string.Empty;
    public StreetcodeType StreetcodeType { get; set; }
    public List<LikeDTO>? Likes { get; set; }
    public int LikesCount { get; set; }
}
