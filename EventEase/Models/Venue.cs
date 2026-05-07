using System.ComponentModel.DataAnnotations;
namespace EventEase.Models;

public class Venue
{
    public int VenueId { get; set; }
    [Required] public string Name { get; set; } = "";
    [Required] public string Location { get; set; } = "";
    [Range(1, 5000)] public int Capacity { get; set; }
    public string? ImageUrl { get; set; } // Stores the Azurite link
}