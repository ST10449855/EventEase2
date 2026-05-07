using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public class Event
{
    public int EventId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required, DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Required, DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
}