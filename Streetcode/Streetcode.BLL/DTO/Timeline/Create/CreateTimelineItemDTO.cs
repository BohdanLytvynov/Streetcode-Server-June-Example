using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Timeline.Create;

public class CreateTimelineItemDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public DateTime Date { get; set; }
    public DateViewPattern DateViewPattern { get; set; }

    public List<HistoricalContextDTO>? HistoricalContexts { get; set; } = new List<HistoricalContextDTO>();
    
    public int StreetCodeId { get; set; }
}