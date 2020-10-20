using System.Threading;
using System.Threading.Tasks;
using StrategyTester.Common.Enums;
using StrategyTester.Contract.Models;

namespace StrategyTester.Contract
{
    public interface IStrategyTester
    {
        Task<string[]> GetStockTitles(CancellationToken token);
        Task<StockItemHistoryContractModel> GetStockHistory(string key, CandlePeriod period, CancellationToken token);
        StrategyResultContractModel ApplyRandomStrategy(CandleContractModel[] candles, CancellationToken token);
    }
}
