using StrategyTester.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StrategyTester.Common.Enums;
using StrategyTester.Contract.Models;
using StrategyTester.Domain.Infrastructure;

namespace StrategyTester.Domain.Query
{
    internal class StockItemHistoryQuery : QueryBase<StockItemHistoryContractModel>
    {
        private readonly string _key;
        private readonly CandlePeriod? _period;

        public StockItemHistoryQuery(string key, CandlePeriod? period)
        {
            _key = key;
            _period = period;
        }

        public override Task<StockItemHistoryContractModel> Run()
        {
            var dbItem = GetRepository<StockItemHistory>().Query().First(x => x.Title == _key);

            if (_period == null || _period == CandlePeriod.HalfOfHour)
                return Task.FromResult(new StockItemHistoryContractModel
                {
                    Period = dbItem.Period,
                    Title = dbItem.Title,
                    Data = dbItem.Data.Select(x => new CandleContractModel
                    {
                        Close = x.Close,
                        Open = x.Open,
                        Low = x.Open,
                        Volume = x.Volume,
                        High = x.High,
                        DateTime = x.Date.Add(x.Time)
                    }).ToList()
                });

            var allCandles = dbItem.Data.ToArray();
            var newPeriodTimespan = GetPeriodTimeSpan(_period.Value);
            var candlesToUpdate = allCandles.SkipWhile(x => x.Date.DayOfWeek != DayOfWeek.Monday).ToArray();

            var result = new List<Candle>();
            for (var i = 0; i < candlesToUpdate.Length;)
            {
                var endDate = candlesToUpdate[i].Date.Add(candlesToUpdate[i].Time).Add(newPeriodTimespan);

                var period = candlesToUpdate.Skip(i).TakeWhile(x => x.Date.Add(x.Time) < endDate).ToList();
                i += period.Count;

                result.Add(new Candle
                {
                    Date = period.Select(x => x.Date).Last(),
                    Time = period.Select(x => x.Time).Last(),

                    High = period.Max(x => x.High),
                    Low = period.Min(x => x.Low),

                    Open = period.Select(x => x.Open).First(),
                    Close = period.Select(x => x.Close).Last(),

                    Volume = period.Sum(x => x.Volume)
                });
            }

            return Task.FromResult(new StockItemHistoryContractModel
            {
                Title = dbItem.Title,
                Period = dbItem.Period,
                Data = result.Select(x => new CandleContractModel
                {
                    Close = x.Close,
                    Open = x.Open,
                    Low = x.Open,
                    Volume = x.Volume,
                    High = x.High,
                    DateTime = x.Date.Add(x.Time)
                }).ToList()
            });
        }

        private TimeSpan GetPeriodTimeSpan(CandlePeriod p)
        {
            return p switch
            {
                CandlePeriod.Hour => TimeSpan.FromHours(1),
                CandlePeriod.TwoHours => TimeSpan.FromHours(2),
                CandlePeriod.TreeHours => TimeSpan.FromHours(3),
                CandlePeriod.ForHours => TimeSpan.FromHours(4),
                CandlePeriod.Day => TimeSpan.FromDays(1),
                CandlePeriod.Week => TimeSpan.FromDays(7),
                CandlePeriod.HalfOfHour => TimeSpan.FromMinutes(30),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}