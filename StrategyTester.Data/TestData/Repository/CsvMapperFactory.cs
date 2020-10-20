using System;
using System.Collections.Generic;
using StrategyTester.Data.Entities;
using StrategyTester.Data.Repository;

namespace StrategyTester.Data.TestData.Repository
{
    public class CsvMapperFactory
    {
        static CsvMapperFactory()
        {
            Mappers = new Dictionary<Type, object>
            {
                { typeof(StockItemHistory), new StockItemHistoryMapper() }
            };
        }

        public static Dictionary<Type, object> Mappers { get; set; }

        public static ICsvMapper<TEntity> Get<TEntity>() 
            where TEntity : IEntityModel
        {
            return (ICsvMapper<TEntity>) Mappers[typeof(TEntity)];
        }
    }
}