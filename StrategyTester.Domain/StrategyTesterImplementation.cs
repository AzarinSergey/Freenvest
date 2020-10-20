using StrategyTester.Common.Enums;
using StrategyTester.Contract;
using StrategyTester.Contract.Models;
using StrategyTester.Domain.Query;
using StrategyTester.Domain.Strategy;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using StrategyTester.Domain.Infrastructure;

[assembly: InternalsVisibleTo("ConsoleApp")]

namespace StrategyTester.Domain
{
    public class StrategyTesterImplementation : IStrategyTester
    {
        private readonly IQueryProcessor _processor;

        public StrategyTesterImplementation(IQueryProcessor processor)
        {
            _processor = processor;
        }

        public async Task<string[]> GetStockTitles(CancellationToken token)
        {
            var result = await _processor.ProcessQuery(token, new StockItemsListQuery());

            return result.Select(x => x.Title).ToArray();
        }

        public Task<StockItemHistoryContractModel> GetStockHistory(string key, CandlePeriod period, CancellationToken token)
        {
            return _processor.ProcessQuery(token, new StockItemHistoryQuery(key, period));
        }

        public StrategyResultContractModel ApplyRandomStrategy(CandleContractModel[] candles, CancellationToken token)
        {
            var strategy = new RandomStrategy(candles);

            return strategy.Apply();
        }
    }
}
