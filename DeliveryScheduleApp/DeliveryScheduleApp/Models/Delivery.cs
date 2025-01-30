using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryScheduleApp.Models
{
    public class Delivery
    {
        public DateTime Time { get; set; }
        public DateTime Cutoff { get; set; }
        public bool IsPreviousDay { get; set; }
        public bool IsNextCutoff { get; set; }
    }
}
