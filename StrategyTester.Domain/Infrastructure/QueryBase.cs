using System.Threading;
using System.Threading.Tasks;
using StrategyTester.Data.Repository;
using StrategyTester.Data.TestData.Repository;

namespace StrategyTester.Domain.Infrastructure
{
    public abstract class QueryBase<T>
    {
        public abstract Task<T> Run();

        internal CancellationToken Token { get; set; }

        protected IDataQueryRepository<TEntity> GetRepository<TEntity>() where TEntity : IEntityModel
        {
            return new TestDataRepository<TEntity>();
        }
    }
}