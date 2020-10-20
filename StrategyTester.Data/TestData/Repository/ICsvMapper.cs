using StrategyTester.Data.Repository;

namespace StrategyTester.Data.TestData.Repository
{
    public interface ICsvMapper<out T>
        where T :IEntityModel
    {
        T Map(string[] csvFileRows);
    }
}