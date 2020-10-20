using StrategyTester.Common.Enums;
using StrategyTester.Data.Entities;
using StrategyTester.Domain.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("\t==================================\n\n");

            var stockItems = await StockItemsListQueryTest("\t > StockItemsListQueryTest < \n");

            Console.WriteLine("\n\n\t==================================");

            await StockItemHistoryQueryTest("StockItemHistoryQueryTest", stockItems);

            Console.WriteLine("\n\n\t==================================");
        }

        private static async Task StockItemHistoryQueryTest(string testName, List<StockItem> stockItems)
        {
            Console.WriteLine(testName);
            foreach (var item in stockItems)
            {
                foreach (var value in Enum.GetValues(typeof(CandlePeriod)))
                {
                    var query = new StockItemHistoryQuery(item.Title, (CandlePeriod)value);
                    var result = await query.Run();
                    var firstItem = result.Data.First();
                    Console.WriteLine($"{result.Title} ({(CandlePeriod)value}) :\t\t {firstItem.DateTime} " +
                                      $"| Open: {firstItem.Open} | High: {firstItem.High} | Low: {firstItem.Low} " +
                                      $"| Close: {firstItem.Close} | Volume: {firstItem.Volume} | TotalCount: {result.Data.Count}");
                }
            }
        }

        private static async Task<List<StockItem>> StockItemsListQueryTest(string testName)
        {
            Console.WriteLine(testName);
            var query = new StockItemsListQuery();
            var result = await query.Run();

            Console.WriteLine(string.Join("\t", result.Select(x => x.Title)));

            return result;
        }
    }
}
