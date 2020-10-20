using StrategyTester.Common.Enums;
using StrategyTester.Data.Entities;
using System;
using System.Globalization;
using System.Linq;

namespace StrategyTester.Data.TestData.Repository
{
    public class StockItemHistoryMapper : ICsvMapper<StockItemHistory>
    {
        public StockItemHistory Map(string[] csvFileRows)
        {
            if(csvFileRows.Length < 2)
                throw new ArgumentException(nameof(csvFileRows));
            
            var csvSeparator = ',';
            var firstStringItems = csvFileRows[1].Split(csvSeparator);

            if (firstStringItems.Length < 2)
                csvSeparator = ':';

            return new StockItemHistory
            {
                Title = firstStringItems[0],
                Period = GetPeriod(firstStringItems[1]),
                Data = csvFileRows.Skip(1).Select((x, i) =>
                {
                    var stringItems = x.Split(csvSeparator);

                    if(!DateTime.TryParseExact( stringItems[2], "ddMMyy",null, DateTimeStyles.None, out var date))
                        throw new FormatException($"Known format 'ddMMyy', try parse value '{stringItems[2]}' in string {i} for 'Candle.Date' property.");
                    if(!TimeSpan.TryParseExact( stringItems[3], "hhmmss", null, TimeSpanStyles.None, out var time))
                        throw new FormatException($"Known format 'hhmmss', try parse value '{stringItems[3]}' in string {i} for 'Candle.Time' property.");
                    return new Candle
                    {
                        Date = date,
                        Time = time,
                        Open = double.Parse(stringItems[4], CultureInfo.InvariantCulture),
                        High = double.Parse(stringItems[5], CultureInfo.InvariantCulture),
                        Low = double.Parse(stringItems[6], CultureInfo.InvariantCulture),
                        Close = double.Parse(stringItems[7], CultureInfo.InvariantCulture),
                        Volume = double.Parse(stringItems[8], CultureInfo.InvariantCulture),
                    };

                }).ToList()
            };
        }

        private CandlePeriod GetPeriod(string period)
        {
            switch (period)
            {
                case "30": return CandlePeriod.HalfOfHour;
                default: throw new NotSupportedException($"Period '{period}' not supported!");
            }
        }
    }
}