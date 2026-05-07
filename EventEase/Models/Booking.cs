using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public class Booking
{
    public int BookingId { get; set; }

    [Required]
    public int VenueId { get; set; }
    public virtual Venue? Venue { get; set; }

    [Required]
    public int EventId { get; set; }
    public virtual Event? Event { get; set; }

    public DateTime BookingDate { get; set; } = DateTime.Now;
}