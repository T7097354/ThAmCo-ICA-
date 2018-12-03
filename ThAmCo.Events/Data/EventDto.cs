using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Data
{
    public class EventDto
    {
        public int TypeID { get; set; }

        public string TypeName { get; set; }
    }
}
