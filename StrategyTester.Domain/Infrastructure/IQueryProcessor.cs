using System.Threading;
using System.Threading.Tasks;

namespace StrategyTester.Domain.Infrastructure
{
    public interface IQueryProcessor
    {
        Task<T> ProcessQuery<T>(CancellationToken token, QueryBase<T> stockItemsListQuery);
    }
}