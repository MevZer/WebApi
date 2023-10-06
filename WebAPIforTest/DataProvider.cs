using Microsoft.EntityFrameworkCore;
using System.Data;
using WebAPIforTest.Models;
using Microsoft.Extensions.Caching.Memory;

namespace WebAPIforTest
{
    public class DataProvider
    {
        ApplicationContext db;
        IMemoryCache cache;
        public DataProvider(ApplicationContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
        }

        public List<Counter> GetCounters()
        {
            var counters = db.Counters.ToList();
            return counters;
        }

        public void CreateEntry(Counter counter)
        {
            db.Counters.Add(counter);
            db.SaveChangesAsync();
            cache.Remove("CountColumn");
        }

        public List<TableByKeys> GetRecordsByKeyAndValue()
        {
            var records = db.Counters.GroupBy(x => x.Key)
                .Select(g => new TableByKeys
                {
                    Key = g.Key,
                    Count = g.Count(),
                    CountMoreThen = g.Where(x => x.Value > 1).Count()
                }).ToList();
            return records;
        }

        public int GetCountRecords()
        {
            cache.TryGetValue("CountColumn", out int count);

            if(count == 0)
            {
                count = db.Counters.Count();
                if(count != 0)
                {
                    Console.WriteLine($"{count} извлечено из базы");
                    cache.Set("CountColumn", count, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(300)));
                }
            }
            else
            {
                Console.WriteLine($"{count} извлечено из кэша");
            }
            return count;
        }


    }
}
