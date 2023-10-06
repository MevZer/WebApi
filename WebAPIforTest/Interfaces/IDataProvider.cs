using WebAPIforTest.Models;


namespace WebAPIforTest.Interfaces
{
    public interface IDataProvider
    {
        public List<Counter> GetCounters();

        public void CreateEntry(Counter counter);

        public List<TableByKeys> GetRecordsByKeyAndValue();

        public int GetCountRecords();


    }
}
