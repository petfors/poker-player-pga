using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Simple.Model
{
    public class HandManager
    {
        public HandResult EvaluateHand(IEnumerable<Card> cards)
        {
            var evaluatedCard = cards.Select(c => new EvaluatedCard(c));
            var maxPair = Pair(evaluatedCard);

            if (maxPair.Any())
            {
                return new HandResult() {Cards = maxPair, Hand = Hand.Pair};
            }

            return new HandResult()
            {
                Cards = new List<EvaluatedCard>() { evaluatedCard.OrderByDescending(c => c.RankValue).First() },
                Hand = Hand.HighCard
            };
        }

        private IEnumerable<EvaluatedCard> Pair(IEnumerable<EvaluatedCard> cards)
        {
            var orderedRankGroups = cards.GroupBy(g => g.RankValue).OrderByDescending(g => g.Key);
            var orderedPairGroups = orderedRankGroups.Where(g => g.ToList().Count == 2);

            if (orderedPairGroups.Any())
            {
                var maxPair = orderedPairGroups.First();
                return maxPair.ToList();
            }

            return new List<EvaluatedCard>();
        }

    }
}
