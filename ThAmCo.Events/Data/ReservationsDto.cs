using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Data
{
    public class ReservationsDto
    {
        public string Reference { get; set; }

        public DateTime EventDate { get; set; }

        public string VenueCode { get; set; }

        //public Availability Availability { get; set; }

        public DateTime WhenMade { get; set; }

        public string StaffId { get; set; }
    }
}
