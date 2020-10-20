using System.Collections.Generic;
using StrategyTester.Common.Enums;
using StrategyTester.Data.Repository;

namespace StrategyTester.Data.Entities
{
    public class StockItemHistory : IEntityModel
    {
        public string Title { get; set; }

        public CandlePeriod Period { get; set; }

        public ICollection<Candle> Data { get; set; }
    }
}