using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Staff
    {
        public int Id { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required, Display(Name = "Forename")]
        public string FirstName { get; set; }

        [Required, DataType(DataType.EmailAddress), Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "First Aider")]
        public bool FirstAider { get; set; }

        [Display(Name = "Staff Bookings")]
        public List<StaffBooking> StaffBookings { get; set; }

        public string FullName()
        {
            return FirstName + Surname;
        }
    }
}
