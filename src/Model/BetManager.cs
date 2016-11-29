using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

        private double MinBetPercentageOfStack(int minBet, int stackSize)
        {
           var percentage = ((double) minBet / (double) stackSize);
            return percentage * 100;
        }

        private bool IsSameSuit(IEnumerable<EvaluatedCard> cards)
        {
            var flushCardGroups = cards.GroupBy(c => c.Suit).Where(g => g.ToList().Count >= 2);
            return flushCardGroups.Any();

        }

        private int GetPreFlopBet(HandResult ourHand, int stackSize, int allInPlayers, int bettingIndex, int activePlayers)
        {
            if (ourHand.Hand == Hand.Pair)
            {
                var pairRank = ourHand.Cards.Max(c => c.RankValue);
                if (pairRank >= 10)
                {
                    return GetMinRaiseTimes(4);
                }
                else
                {
                    if (bettingIndex <= 3)
                    {
                        return _game.MinRaise;
                    }
                    else
                    {
                        return _game.MinBet;
                    }
                }
            }
            if (ourHand.Hand == Hand.HighCard)
            {
                var isSameSuit = IsSameSuit(ourHand.Cards);

                return HighCardBettingStrategy(ourHand, allInPlayers, stackSize, activePlayers, bettingIndex, isSameSuit);
            }

            return 0;
        }

        private int HighCardBettingStrategy(HandResult ourHand, int allInPlayers, int stackSize, int activePlayers, int bettingIndex, bool isSameSuit = false)
        {
            var highestCard = ourHand.Cards.FirstOrDefault();
            var secondHighCard = ourHand.Cards.Skip(1).FirstOrDefault();

            if (highestCard != null && secondHighCard != null)
            {
                if (highestCard.RankValue >= 11 && secondHighCard.RankValue >= 11)
                {
                    if (allInPlayers <= 0 && activePlayers <= 6)
                    {
                        if (bettingIndex <= 3)
                        {
                            if (isSameSuit)
                            {
                                return GetMinRaiseTimes(2);
                            }

                            return _game.MinRaise;
                        }
                        else
                        {
                            return _game.MinBet;
                        }
                    }
                }
            }

            if (highestCard != null && secondHighCard != null && highestCard.RankValue >= 11)
            {
                if (isSameSuit)
                {
                    if (bettingIndex <= 3)
                    {
                        return GetMinRaiseTimes(2);
                    }

                    return _game.MinBet;
                }
            }

            if (highestCard != null && highestCard.RankValue >= 12)
            {
                if (allInPlayers <= 0 && activePlayers <= 6)
                {
                    if (bettingIndex <= 3)
                    {
                        if (isSameSuit)
                        {
                            return GetMinRaiseTimes(2);
                        }

                        var percentageOfStack = MinBetPercentageOfStack(_game.MinBet, stackSize);

                        if (percentageOfStack < 50)
                        {
                            return _game.MinRaise;
                        }
                        else
                        {
                            return _game.MinBet;
                        }
                    }
                    else
                    {
                        return _game.MinBet;
                    }
                }

                return 0;
            }

            return 0;
        }

        private int GetMinRaiseTimes(int times)
        {
            return Math.Min(_game.MaxBet, _game.MinRaise* times);
        }


        private int GetFlopBet(HandResult ourHand, int stackSize, int allInPlayers, int bettingIndex, int activePlayers)
        {
            if (ourHand.Hand == Hand.StraightFlush)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.FourOfAKind)
            {
                return GetMinRaiseTimes(10);
            }
            if (ourHand.Hand == Hand.FullHouse)
            {
                return GetMinRaiseTimes(8);
            }
            if (ourHand.Hand == Hand.Flush)
            {
                if (bettingIndex <= 1)
                {
                    return GetMinRaiseTimes(8);
                }
                else
                {
                    return _game.MinBet;
                }
            }
            if (ourHand.Hand == Hand.Straight)
            {
                if (bettingIndex <= 1)
                {
                    return GetMinRaiseTimes(8);
                }
                else
                {
                    return _game.MinBet;
                }
            }
            if (ourHand.Hand == Hand.ThreeOfAKind)
            {
                if (bettingIndex <= 1)
                {
                    return GetMinRaiseTimes(4);
                }
                else
                {
                    return _game.MinBet;
                }
            }
            if (ourHand.Hand == Hand.TwoPair)
            {
                if (bettingIndex <= 3)
                {
                    return _game.MinRaise;
                }
                return _game.MinBet;
            }
            if (ourHand.Hand == Hand.Pair)
            {
                if (ourHand.Cards.Max(c => c.RankValue) >= 10)
                {
                    if (bettingIndex <= 3)
                    {
                        return _game.MinRaise;
                    }
                }

                return _game.MinBet;
            }
            if (ourHand.Hand == Hand.HighCard)
            {
                return HighCardBettingStrategy(ourHand, allInPlayers, stackSize, activePlayers, bettingIndex, false);
            }

            return 0;
        }

        private int GetTurnBet(HandResult ourHand, int bettingIndex)
        {
            if (ourHand.Hand == Hand.StraightFlush)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.FourOfAKind)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.FullHouse)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.Flush)
            {
                if (bettingIndex <= 1)
                {
                    return GetMinRaiseTimes(10);
                }
                else
                {
                    return _game.MinBet;
                }
            }
            if (ourHand.Hand == Hand.Straight)
            {
                if (bettingIndex <= 1)
                {
                    return GetMinRaiseTimes(10);
                }
                else
                {
                    return _game.MinBet;
                }
            }
            if (ourHand.Hand == Hand.ThreeOfAKind)
            {
                return _game.MinRaise;
            }
            if (ourHand.Hand == Hand.TwoPair)
            {
                if (bettingIndex <= 3)
                {
                    return _game.MinRaise;
                }
                return _game.MinBet;
            }
            if (ourHand.Hand == Hand.Pair)
            {
                if (ourHand.Cards.Max(c => c.RankValue) >= 10)
                {
                    if (bettingIndex <= 3)
                    {
                        return _game.MinRaise;
                    }
                }

                return _game.MinBet;
            }

            return 0;
        }

        private int GetRiverBet(HandResult ourHand, int bettingIndex)
        {
            if (ourHand.Hand == Hand.StraightFlush)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.FourOfAKind)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.FullHouse)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.Flush)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.Straight)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.ThreeOfAKind)
            {
                return _game.MaxBet;
            }
            if (ourHand.Hand == Hand.TwoPair)
            {
                if (bettingIndex <= 3)
                {
                    return _game.MinRaise;
                }
                return _game.MinBet;
            }
            if (ourHand.Hand == Hand.Pair)
            {
                if (ourHand.Cards.Max(c => c.RankValue) >= 10)
                {
                    if (bettingIndex <= 3)
                    {
                        return _game.MinRaise;
                    }
                }

                return _game.MinBet;
            }

            return 0;
        }

        private int GetCurrentBet(BettingRound bettingRound, HandResult ourHand, IEnumerable<Player> allinPlayers)
        {
            if (bettingRound == BettingRound.PreFlop)
            {
                return GetPreFlopBet(ourHand, _game.OurPlayer.stack, allinPlayers.Count(), _game.BettingIndex, _game.ActivePlayers);
            }
            if (bettingRound == BettingRound.Flop)
            {
                return GetFlopBet(ourHand, _game.OurPlayer.stack, allinPlayers.Count(), _game.BettingIndex, _game.ActivePlayers);
            }
            if (bettingRound == BettingRound.Turn)
            {
                return GetTurnBet(ourHand, _game.BettingIndex);
            }
            if (bettingRound == BettingRound.River)
            {
                return GetRiverBet(ourHand, _game.BettingIndex);
            }

            return 0;
        }


        public int GetCurrentBet(IEnumerable<Card> cards)
        {
            var bettingRound = GetBettingRound(cards);
            var ourHand = _handManager.EvaluateHand(cards);
            var allInPlayers = _game.OtherAllInPlayers;

            return GetCurrentBet(bettingRound, ourHand, allInPlayers);
        }
    }
}
