using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryScheduleApp.Models
{
    public class StoreSchedule
    {
        public string StoreNumber { get; set; }
        public List<Delivery> Deliveries { get; set; }
    }
}
