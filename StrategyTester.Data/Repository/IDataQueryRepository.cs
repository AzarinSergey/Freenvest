using System.Linq;

namespace StrategyTester.Data.Repository
{
    public interface IDataQueryRepository<T>
    {
        IQueryable<T> Query();
    }
}