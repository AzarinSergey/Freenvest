using StrategyTester.Contract.Models;

namespace StrategyTester.Domain.Strategy
{
    internal class RandomStrategy
    {
        private readonly CandleContractModel[] _history;

        public RandomStrategy(CandleContractModel[] history)
        {
            _history = history;
        }

        public StrategyResultContractModel Apply()
        {
            return new StrategyResultContractModel
            {
                BuyIndexes = new[] {_history.Length - 20}, 
                SellIndexes = new[] {_history.Length - 1}
            };
        }
    }
}
