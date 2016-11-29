using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Simple.Model
{
    public class BetManager
    {
        private HandManager _handManager;
        private Game _game;

        public BetManager(HandManager handManager, Game game)
        {
            _handManager = handManager;
            _game = game;
        }

        private BettingRound GetBettingRound(IEnumerable<Card> cards)
        {
            if(cards.Count() <= 2)
                return BettingRound.PreFlop;
            if(cards.Count() == 5)
                return BettingRound.Flop;
            if(cards.Count() == 6)
                return BettingRound.Turn;
            if(cards.Count() == 7)
                return BettingRound.River;

            return BettingRound.PreFlop;
        }

        private int GetPreFlopBet(HandResult ourHand)
        {
            if (ourHand.Hand == Hand.Pair)
            {
                var pairRank = ourHand.Cards.Max(c => c.RankValue);
                if (pairRank >= 10)
                {
                    return _game.MaxBet;
                }
                else
                {
                    return _game.MinRaise;
                }
            }
            if (ourHand.Hand == Hand.HighCard)
            {
                var highestCard = ourHand.Cards.FirstOrDefault();
                if (highestCard != null && highestCard.RankValue >= 12)
                {
                    return _game.MinRaise;
                }

                return 0;
            }

            return 0;
        }

        private int GetFlopBet(HandResult ourHand)
        {
            if (ourHand.Hand == Hand.FourOfAKind)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.Flush)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.ThreeOfAKind)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.Pair)
            {
                if (ourHand.Cards.Max(c => c.RankValue) >= 10)
                {
                    return _game.MinRaise;
                }

                return _game.MinBet;
            }

            return 0;
        }

        private int GetTurnBet(HandResult ourHand)
        {
            if (ourHand.Hand == Hand.FourOfAKind)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.Flush)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.ThreeOfAKind)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.Pair)
            {
                if (ourHand.Cards.Max(c => c.RankValue) >= 10)
                {
                    return _game.MinRaise;
                }

                return _game.MinBet;
            }

            return 0;
        }

        private int GetRiverBet(HandResult ourHand)
        {
            if (ourHand.Hand == Hand.FourOfAKind)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.Flush)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.ThreeOfAKind)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.Pair)
            {
                if (ourHand.Cards.Max(c => c.RankValue) >= 10)
                {
                    return _game.MinRaise;
                }

                return 0;
            }

            return 0;
        }

        private int GetCurrentBet(BettingRound bettingRound, HandResult ourHand)
        {
            if (bettingRound == BettingRound.PreFlop)
            {
                return GetPreFlopBet(ourHand);
            }
            if (bettingRound == BettingRound.Flop)
            {
                return GetFlopBet(ourHand);
            }
            if (bettingRound == BettingRound.Turn)
            {
                return GetTurnBet(ourHand);
            }
            if (bettingRound == BettingRound.River)
            {
                return GetRiverBet(ourHand);
            }

            return 0;
        }


        public int GetCurrentBet(IEnumerable<Card> cards)
        {
            var bettingRound = GetBettingRound(cards);
            var ourHand = _handManager.EvaluateHand(cards);

            return GetCurrentBet(bettingRound, ourHand);
        }
    }
}
