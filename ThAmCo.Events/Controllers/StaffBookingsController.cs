using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Controllers
{
    public class StaffBookingsController : Controller
    {
        private readonly EventsDbContext _context;

        public StaffBookingsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: StaffBookings
        public async Task<IActionResult> Index(int? id)
        {
            var eventsDbContext = (id == null) ? _context.StaffBooking.Include(g => g.Staff).Include(g => g.Event) : _context.StaffBooking.Include(g => g.Staff).Include(g => g.Event).Where(e => e.EventId == id);
            return View(await eventsDbContext.OrderBy(a => a.EventId).ToListAsync());
        }

        // GET: StaffBookings/Details/5
        public async Task<IActionResult> Details(int? id, int? id2)
        {
            if (id == null || id2 == null)
            {
                return NotFound();
            }

            var staffBooking = await _context.StaffBooking
                .Include(s => s.Staff)
                .Include(s => s.Event)
                .Where(e => e.EventId == id)
                .FirstOrDefaultAsync(m => m.StaffId == id2);
            if (staffBooking == null)
            {
                return NotFound();
            }

            return View(staffBooking);
        }

        // GET: StaffBookings/Create
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList((from s in _context.Staff select new
            {
                Id = s.Id,
                FullName = s.FirstName + " " + s.Surname
            }),
                "Id",
                    "FullName");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title");
            return View();
        }

        // POST: StaffBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,EventId")] StaffBooking staffBooking)
        {
            if (ModelState.IsValid)
            {
                if (!StaffBookingExists(staffBooking.StaffId, staffBooking.EventId))
                {
                    _context.Add(staffBooking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Debug.WriteLine("Guest Booking already Exists");
                }
            }
            ViewData["StaffId"] = new SelectList((from s in _context.Staff
                    select new
                    {
                        Id = s.Id,
                        FullName = s.FirstName + " " + s.Surname
                    }),
                "Id",
                "FullName", staffBooking.StaffId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", staffBooking.EventId);
            return View(staffBooking);
        }

        // GET: StaffBookings/Edit/5
        public async Task<IActionResult> Edit(int? id, int? id2)
        {
            if (id == null || id2 == null)
            {
                return NotFound();
            }

            var staffBooking = await _context.StaffBooking.FindAsync(id2, id);
            if (staffBooking == null)
            {
                return NotFound();
            }
            ViewData["StaffId"] = new SelectList((from s in _context.Staff
                    select new
                    {
                        Id = s.Id,
                        FullName = s.FirstName + " " + s.Surname
                    }),
                "Id",
                "FullName", staffBooking.StaffId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", staffBooking.EventId);
            return View(staffBooking);
        }

        // POST: StaffBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int id2, [Bind("StaffId,EventId")] StaffBooking staffBooking)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staffBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffBookingExists(staffBooking.StaffId, staffBooking.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StaffId"] = new SelectList((from s in _context.Staff
                    select new
                    {
                        Id = s.Id,
                        FullName = s.FirstName + " " + s.Surname
                    }),
                "Id",
                "FullName", staffBooking.StaffId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", staffBooking.EventId);
            return View(staffBooking);
        }

        // GET: StaffBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffBooking = await _context.StaffBooking
                .Include(s => s.Event)
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staffBooking == null)
            {
                return NotFound();
            }

            return View(staffBooking);
        }

        // POST: StaffBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staffBooking = await _context.StaffBooking.FindAsync(id);
            _context.StaffBooking.Remove(staffBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffBookingExists(int sId, int eId)
        {
            foreach (StaffBooking booking in _context.StaffBooking)
            {
                if (booking.StaffId == sId && booking.EventId == eId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
