using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Newtonsoft.Json;

namespace Mandalium.API.Helpers
{
    public class CountList
    {
        public int Id { get; set; }
        public int Count { get; set; } = 1;
        public bool WriterEntry { get; set; } = false;
    }
    public class Counter
    {

        public static Dictionary<DateTime, List<CountList>> WeeklyCounter;
        Counter()
        {
            if (WeeklyCounter != null)
            {
                return;
            }
            else
            {
                WeeklyCounter = new Dictionary<DateTime, List<CountList>>();
            }
        }

        public static void Add(DateTime day, int id, bool writerEntry)
        {
            if (WeeklyCounter == null)
            {
                WeeklyCounter = JsonConvert.DeserializeObject<Dictionary<DateTime, List<CountList>>>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Helpers\\weeklyData.json")));
            }
            
            if (WeeklyCounter.ContainsKey(day))
            {
                List<CountList> list;
                WeeklyCounter.TryGetValue(day, out list);
                if (list != null)
                {
                    var blogEntry = list.FirstOrDefault(x => x.Id == id);
                    if (blogEntry != null)
                    {
                        blogEntry.Count += 1;
                        list[list.FindIndex(x => x.Id == id)] = blogEntry;
                        WeeklyCounter[day] = list;
                    }
                    else
                    {
                        list.Add(new CountList
                        {
                            Id = id,
                            WriterEntry = writerEntry
                        });
                        WeeklyCounter[day] = list;
                    }
                }
            }
            else
            {
                var list = new List<CountList>();
                list.Add(new CountList
                {
                    Id = id,
                    WriterEntry = writerEntry
                });
                WeeklyCounter.Add(day, list);
            }

            if (WeeklyCounter.ContainsKey(DateTime.Now.Date.AddDays(-8)))
            {
                WeeklyCounter.Remove(DateTime.Now.Date.AddDays(-8));
            }


            var json = JsonConvert.SerializeObject(WeeklyCounter);
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"Helpers\\weeklyData.json"), json);
        }


        public static Dictionary<DateTime, List<CountList>> GetMostRead()
        {
            return WeeklyCounter = JsonConvert.DeserializeObject<Dictionary<DateTime, List<CountList>>>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Helpers\\weeklyData.json")));
        }

        // public void AddtoDatabase(object sender, ElapsedEventArgs e)
        // {
        //     //silme işlemleri


        //     var json = JsonConvert.SerializeObject(WeeklyCounter);
        //     File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"Helpers\\weeklyData.json"), json);
        // }






    }
}