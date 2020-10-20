using StrategyTester.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StrategyTester.Domain.Infrastructure;

namespace StrategyTester.Domain.Query
{
    internal class StockItemsListQuery : QueryBase<List<StockItem>>
    {
        public override Task<List<StockItem>> Run()
        {
            return Task.FromResult(GetRepository<StockItemHistory>().Query().Select(x => new StockItem
            {
                Title = x.Title
            }).ToList());
        }
    }
}
