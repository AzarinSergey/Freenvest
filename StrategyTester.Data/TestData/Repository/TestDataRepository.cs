using StrategyTester.Data.Repository;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace StrategyTester.Data.TestData.Repository
{

    public class TestDataRepository<T> : IDataRepository<T>
        where T : IEntityModel
    {
        
        private readonly ICsvMapper<T> _mapper;
        private const string TestDataFolder = "TestData";

        public TestDataRepository()
        {
            _mapper = CsvMapperFactory.Get<T>();
        }

        public IQueryable<T> Query()
        {
            var result = new ConcurrentBag<T>();
            foreach (var file in Directory.GetFiles(TestDataFolder).AsParallel())
                result.Add(_mapper.Map(File.ReadAllLines(file)));

            return result.AsQueryable();
        }
    }
}
