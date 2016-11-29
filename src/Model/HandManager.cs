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


            var fullHouse = FullHouse(evaluatedCard);
            if (fullHouse.Any())
            {
                return new HandResult() { Cards = fullHouse, Hand = Hand.FullHouse};
            }

            var flush = Flush(evaluatedCard);
            if (flush.Any())
            {
                return new HandResult() {Cards = flush, Hand = Hand.Flush};
            }

            var straight = Straight(evaluatedCard);
            if (straight.Any())
            {
                return new HandResult() { Cards = straight, Hand = Hand.Straight };
            }


            var maxFourOfAKind = FourOfAKind(evaluatedCard);
            if (maxFourOfAKind.Any())
            {
                return new HandResult() { Cards = maxFourOfAKind, Hand = Hand.FourOfAKind };
            }

            var maxThreeOfAKind = ThreeOfAKind(evaluatedCard);
            if (maxThreeOfAKind.Any())
            {
                return new HandResult() { Cards = maxFourOfAKind, Hand = Hand.ThreeOfAKind };
            }

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

        private IEnumerable<EvaluatedCard> FullHouse(IEnumerable<EvaluatedCard> cards)
        {
            var threeOfAKind = ThreeOfAKind(cards);
            if (threeOfAKind.Any())
            {
                var remainingCards = cards.Except(threeOfAKind);
                var pair = Pair(remainingCards);
                if (pair.Any())
                {
                    return Enumerable.Concat(threeOfAKind, pair);
                }
            }

            return new List<EvaluatedCard>();
        }

        private IEnumerable<EvaluatedCard> Flush(IEnumerable<EvaluatedCard> cards)
        {
            var flushCardGroups = cards.GroupBy(c => c.Suit).Where(g => g.ToList().Count >= 5);
            if (flushCardGroups.Any())
            {
                return flushCardGroups.First().ToList();
            }

            return new List<EvaluatedCard>();
        }

        private IEnumerable<EvaluatedCard> Straight(IEnumerable<EvaluatedCard> cards)
        {
            var orderedStraightCards = cards.OrderByDescending(c => c.RankValue);
            var straightCards = new List<EvaluatedCard>();
            straightCards.Add(orderedStraightCards.First());
            var remainingOrderedCards = orderedStraightCards.Skip(1);

            foreach (var currentCard in remainingOrderedCards)
            {
                if (currentCard.RankValue == (straightCards.Last().RankValue - 1))
                {
                    straightCards.Add(currentCard);
                }
                else
                {
                    straightCards.Clear();
                    straightCards.Add(currentCard);
                }
            }

            if (straightCards.Count >= 5)
            {
                return straightCards;
            }         

            return new List<EvaluatedCard>();
        }

        private IEnumerable<EvaluatedCard> FourOfAKind(IEnumerable<EvaluatedCard> cards)
        {
            var orderedRankGroups = cards.GroupBy(g => g.RankValue).OrderByDescending(g => g.Key);
            var orderedPairGroups = orderedRankGroups.Where(g => g.ToList().Count == 4);

            if (orderedPairGroups.Any())
            {
                var maxFourOfAKind = orderedPairGroups.First();
                return maxFourOfAKind.ToList();
            }

            return new List<EvaluatedCard>();
        }

        private IEnumerable<EvaluatedCard> ThreeOfAKind(IEnumerable<EvaluatedCard> cards)
        {
            var orderedRankGroups = cards.GroupBy(g => g.RankValue).OrderByDescending(g => g.Key);
            var orderedPairGroups = orderedRankGroups.Where(g => g.ToList().Count == 3);

            if (orderedPairGroups.Any())
            {
                var maxThreeOfAKind = orderedPairGroups.First();
                return maxThreeOfAKind.ToList();
            }

            return new List<EvaluatedCard>();
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
