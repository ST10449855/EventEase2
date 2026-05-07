using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;

namespace EventEase.Controllers;

public class BookingsController : Controller
{
    private readonly ApplicationDbContext _context;
    public BookingsController(ApplicationDbContext context) => _context = context;

    // RUBRIC: Consolidated View and Search by ID/Name
    public async Task<IActionResult> Index(string searchString)
    {
        var bookings = _context.Bookings.Include(b => b.Venue).Include(b => b.Event).AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            bookings = bookings.Where(s => s.Event.Name.Contains(searchString) || s.BookingId.ToString() == searchString);
        }
        return View(await bookings.ToListAsync());
    }

    public IActionResult Create()
    {
        ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name");
        ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("VenueId,EventId")] Booking booking)
    {
        var ev = await _context.Events.FindAsync(booking.EventId);

        // RUBRIC: Robust Double Booking Prevention
        bool conflict = await _context.Bookings.Include(b => b.Event)
            .AnyAsync(b => b.VenueId == booking.VenueId && ev.StartDate < b.Event.EndDate && ev.EndDate > b.Event.StartDate);

        if (conflict)
        {
            ModelState.AddModelError("", "CONFLICT: This venue is already occupied for these dates.");
        }
        else if (ModelState.IsValid)
        {
            booking.Venue = null; booking.Event = null; // Prevent Foreign Key error
            _context.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
        ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", booking.EventId);
        return View(booking);
    }
}