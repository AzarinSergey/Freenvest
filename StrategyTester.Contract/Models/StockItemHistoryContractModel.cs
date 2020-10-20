using StrategyTester.Common.Enums;
using System.Collections.Generic;

namespace StrategyTester.Contract.Models
{
    public class StockItemHistoryContractModel
    {
        public string Title { get; set; }

        public CandlePeriod Period { get; set; }

        public ICollection<CandleContractModel> Data { get; set; }
    }
}
