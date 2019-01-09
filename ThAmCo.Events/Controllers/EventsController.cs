﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;

        public EventsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var eventTypeInfo = new List<EventDto>().AsEnumerable();

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:23652/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            HttpResponseMessage response = await client.GetAsync("api/eventtypes");

            if (response.IsSuccessStatusCode)
            {
                eventTypeInfo = await response.Content.ReadAsAsync<IEnumerable<EventDto>>();
            }
            else
            {
                throw new Exception();
            }

            var eventGuestsDbContext = _context.Events
                .Include(g => g.Bookings)
                .Include(sb => sb.StaffBookings)
                .Select(g => new EventsViewModel {
                    Id = g.Id,
                    Title = g.Title,
                    Date = g.Date,
                    Duration = g.Duration,
                    TypeId = g.TypeId,
                    Bookings = g.Bookings,
                    TypeValue = eventTypeInfo.Where(h => h.id == g.TypeId).Select(n => n.title).FirstOrDefault(),
                    VenueCode = g.VenueCode,
                    StaffBookings = g.StaffBookings
                });

            return View(await eventGuestsDbContext.ToListAsync());
        }
                
        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            var eventTypeInfo = new List<EventDto>().AsEnumerable();

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:23652/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            HttpResponseMessage response = await client.GetAsync("api/eventtypes");

            if (response.IsSuccessStatusCode)
            {
                eventTypeInfo = await response.Content.ReadAsAsync<IEnumerable<EventDto>>();
            }
            else
            {
                throw new Exception();
            }

            ViewData["TypeId"] = new SelectList(eventTypeInfo.ToList(), "id", "title", eventTypeInfo.Select(h => h.id == "CON"));
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Date,Duration,TypeId,VenueCode")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();

                var content = new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("reference",@event.VenueCode + @event.Date)
                    }
                );
                HttpClient client = new HttpClient();
                client.BaseAddress = new System.Uri("http://localhost:23652/");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                HttpResponseMessage response = await client.PostAsync("api/Reservations", content);

                return RedirectToAction(nameof(Index));
            }

            return View(@event);
        }

        public async Task<IEnumerable<VenueDto>> getVenues([FromQuery, MinLength(3), MaxLength(3), Required] string eventCode, [FromQuery, Required] string datePassed)
        {
            var eventTypeInfo = new List<VenueDto>().AsEnumerable();

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:23652/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            HttpResponseMessage response = await client.GetAsync("api/Availability?eventType=" + eventCode + "&beginDate=" + datePassed + "&endDate=" + datePassed);

            if (response.IsSuccessStatusCode)
            {
                eventTypeInfo = await response.Content.ReadAsAsync<IEnumerable<VenueDto>>();
            }
            else
            {
                throw new Exception();
            }
            
            return eventTypeInfo;
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Date,Duration,TypeId")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
