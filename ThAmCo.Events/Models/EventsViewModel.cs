﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class EventsViewModel
    {
        public int Id { get; set; }

        [Required, Display(Name = "Event Name")]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        [Required, Display(Name = "Event Type")]
        public string TypeId { get; set; }

        [Display(Name = "Guest Bookings")]
        public List<GuestBooking> Bookings { get; set; }

        [Display(Name = "Staff Bookings")]
        public List<StaffBooking> StaffBookings { get; set; }

        [Display(Name = "Event Type")]
        public string TypeValue { get; set; }

        [Display(Name = "Venue Code")]
        public string VenueCode { get; set; }
    }
}
