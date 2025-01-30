using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace DeliveryScheduleApp.Models
{
    internal static class Db
    {
        public static List<StoreSchedule> GetSchedulesFromFile()
        {
            var schedules = new List<StoreSchedule>();
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\schedule.txt");

            foreach (var line in lines)
            {
                if (line[0]  == '/') continue;
                var parts = line.Split(',');
                var deliveryTime = DateTime.Parse(parts[0].Trim(), CultureInfo.InvariantCulture);

                foreach (var cutoff in parts.Skip(1))
                {
                    var storeCutoff = cutoff.Trim().Split(' ');
                    if (storeCutoff.Length != 2) continue;
                    string storeNum = storeCutoff[0].Trim();

                    // get cutoff time
                    var cutoffTimeStr = storeCutoff[1].Trim();
                    var isPreviousDay = cutoffTimeStr.EndsWith("*");
                    if (isPreviousDay) cutoffTimeStr = cutoffTimeStr.TrimEnd('*').Trim();
                    var cutoffTime = DateTime.Parse(cutoffTimeStr, CultureInfo.InvariantCulture);
                    if (isPreviousDay)
                    {
                        cutoffTime = cutoffTime.AddDays(-1);
                    }

                    var delivery = new Delivery
                    {
                        Cutoff = cutoffTime,
                        IsPreviousDay = isPreviousDay,
                        Time = deliveryTime
                    };

                    var store = schedules.Where(x => x.StoreNumber == storeNum).FirstOrDefault();
                    if (store != null)
                    {
                        store.Deliveries.Add(delivery);
                    } 
                    else
                    {
                        schedules.Add(new StoreSchedule
                        {
                            StoreNumber = storeNum,
                            Deliveries = new List<Delivery> { delivery }
                        });
                    }
                }
            }

            return schedules;
        }
    }
}
