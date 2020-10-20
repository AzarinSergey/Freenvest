using System.Threading;
using System.Threading.Tasks;

namespace StrategyTester.Domain.Infrastructure
{
    internal class QueryProcessor : IQueryProcessor
    {
        public Task<T> ProcessQuery<T>(CancellationToken token, QueryBase<T> stockItemsListQuery)
        {
            stockItemsListQuery.Token = token;
            return stockItemsListQuery.Run();
        }
    }
}