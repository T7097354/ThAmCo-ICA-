using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Data
{
    public class StaffBooking
    {
        [Display(Name = "Staff Name")]
        public int StaffId { get; set; }

        public Staff Staff { get; set; }

        [Display(Name = "Event Name")]
        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}
