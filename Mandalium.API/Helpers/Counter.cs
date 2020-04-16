using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Mandalium.API.Helpers
{
    public class CountOfEntry
    {
        public int Id { get; set; }
        public int Count { get; set; } = 1;
        public bool WriterEntry { get; set; } = false;
    }
    public class Counter
    {

        public static Dictionary<DateTime, List<CountOfEntry>> WeeklyCounter;
        Counter()
        {
            if (WeeklyCounter == null)
            {
                WeeklyCounter = new Dictionary<DateTime, List<CountOfEntry>>();
            }

        }


        //TODO Read all text bütün doyayı okuyor onun yerine jsontextreader kullan okuduğu anda yapıyor.
        public static void Add(DateTime day, int id, bool writerEntry)
        {
            if (WeeklyCounter == null)
            {

                WeeklyCounter = JsonConvert.DeserializeObject<Dictionary<DateTime, List<CountOfEntry>>>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Helpers\\weeklyData.json")));
            }

            if (WeeklyCounter.ContainsKey(day))
            {
                WeeklyCounter.TryGetValue(day, out List<CountOfEntry> list);
                if (list != null)
                {
                    CountOfEntry blogEntry = list.FirstOrDefault(x => x.Id == id);
                    if (blogEntry != null)
                    {
                        blogEntry.Count += 1;
                        list[list.FindIndex(x => x.Id == id)] = blogEntry;
                        WeeklyCounter[day] = list;
                    }
                    else
                    {
                        list.Add(new CountOfEntry
                        {
                            Id = id,
                            WriterEntry = writerEntry,
                        });
                        WeeklyCounter[day] = list;
                    }
                }
            }
            else
            {
                var list = new List<CountOfEntry>();
                list.Add(new CountOfEntry
                {
                    Id = id,
                    WriterEntry = writerEntry
                });
                WeeklyCounter.Add(day, list);
            }

            //TODO bu sildiği datayı başka bir dosyaya yazdır.
            foreach (var key in WeeklyCounter.Keys)
            {
                if (key <= DateTime.Now.Date.AddDays(-8))
                {
                    var wholeData = JsonConvert.DeserializeObject<Dictionary<DateTime, List<CountOfEntry>>>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Helpers\\wholeData.json")));
                    wholeData.Add(key, WeeklyCounter[key]);
                     OrganizeData(wholeData);
                    var wholeDataJson = JsonConvert.SerializeObject(wholeData);
                    File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"Helpers\\wholeData.json"), wholeDataJson);
                  
                    WeeklyCounter.Remove(key);
                }
            }



            OrganizeData(WeeklyCounter);

            var json = JsonConvert.SerializeObject(WeeklyCounter);

            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"Helpers\\weeklyData.json"), json);

        }

        private static void OrganizeData(Dictionary<DateTime, List<CountOfEntry>> dict)
        {

            Dictionary<DateTime, List<CountOfEntry>> newWeeklyCounter = new Dictionary<DateTime, List<CountOfEntry>>();
            foreach (var key in dict.Keys)
            {
                dict.TryGetValue(key, out var list);
                if (list != null)
                {
                    CountOfEntry blogEntry;
                    List<CountOfEntry> centry = new List<CountOfEntry>();
                    foreach (var item in list)
                    {
                        var y = list.FindAll(y => y.Id == item.Id);
                        if (centry.FirstOrDefault(x => x.Id == item.Id) != null)
                        {
                            continue;
                        }
                        if (y.Count > 1)
                        {
                            blogEntry = new CountOfEntry()
                            {
                                Id = y.FirstOrDefault().Id,
                                Count = 0
                            };
                            for (int i = 0; i < y.Count; i++)
                            {
                                blogEntry.Count += y[i].Count;
                            }
                            centry.Add(blogEntry);
                        }
                        else
                        {
                            centry.Add(y.FirstOrDefault());
                        }
                    }
                    newWeeklyCounter.Add(key, centry);
                }
            }
            dict = newWeeklyCounter;
        }

        public static Dictionary<DateTime, List<CountOfEntry>> GetMostRead()
        {

            WeeklyCounter = JsonConvert.DeserializeObject<Dictionary<DateTime, List<CountOfEntry>>>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Helpers\\weeklyData.json")));


            Dictionary<DateTime, List<CountOfEntry>> newWeeklyCounter = new Dictionary<DateTime, List<CountOfEntry>>();
            List<CountOfEntry> centry = new List<CountOfEntry>();
            foreach (var key in WeeklyCounter.Keys)
            {
                WeeklyCounter.TryGetValue(key, out var list);
                var a = centry.Concat(list);
                centry = a.ToList();
            }
            newWeeklyCounter.Add(DateTime.Now.Date, centry);
            OrganizeData(newWeeklyCounter);
            return newWeeklyCounter;
        }


    }
}