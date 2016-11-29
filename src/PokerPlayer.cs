﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using Nancy.Simple.Model;
using Newtonsoft.Json.Linq;

namespace Nancy.Simple
{
    public static class PokerPlayer
    {
        public static readonly string VERSION = "2.0";

        public static int BetRequest(JObject gameState)
        {
            try
            {
                var gameStateObject = gameState.ToObject<GameState>();
                var game = new Game(gameStateObject);

                var betManager = new BetManager(new HandManager(), game);

                var currentBet = betManager.GetCurrentBet(game.OurCards);

                return currentBet;

                //var handManager = new HandManager();
                //var ourHand = handManager.EvaluateHand(game.OurPlayer.hole_cards);

                //if (ourHand.Hand == Hand.Pair)
                //{
                //    var pairRank = ourHand.Cards.Max(c => c.RankValue);
                //    if (pairRank >= 10)
                //    {
                //        return game.MaxBet;
                //    }
                //    else
                //    {
                //        return game.MinRaise;
                //    }
                //}
                //else if(ourHand.Hand == Hand.HighCard)
                //{
                //    var highestCard = ourHand.Cards.FirstOrDefault();
                //    if (highestCard != null && highestCard.RankValue >= 12)
                //    {
                //        return game.MinRaise;
                //    }

                //    return 0;
                //}
                //else
                //{
                //    return 0;
                //}
            }
            catch (Exception ex)
            {
                Console.Error.Write(String.Concat(ex.Message, ex.StackTrace));
                return 0;
            }
		}

		public static void ShowDown(JObject gameState)
		{
			//TODO: Use this method to showdown
		}
	}
}

