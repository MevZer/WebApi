using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebAPIforTest.Models;

namespace WebAPIforTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private DataProvider dataProvider;

        public DataController(ApplicationContext context, IMemoryCache memoryCache)
        {
            dataProvider = new DataProvider(context, memoryCache);
        }

        [HttpGet]
        public IEnumerable<Counter> GetAllData() => 
            dataProvider.GetCounters();


        [HttpPost]
        public IActionResult AddData(Counter counter)
        {
            var records = counter;
            dataProvider.CreateEntry(records);
            return Ok(records);
        }

        [HttpPost("AddRecord")]
        public IActionResult AddDataBody([FromBody] Counter counter) => 
            AddData(counter);


        [HttpGet("key")]
        public IEnumerable<TableByKeys> GetTableByKeys() => 
            dataProvider.GetRecordsByKeyAndValue();

        [HttpGet("allcount")]
        public int GetAllCount() => 
            dataProvider.GetCountRecords();
    }
}
