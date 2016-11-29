using System;
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

                var handManager = new HandManager();
                var ourHand = handManager.EvaluateHand(game.OurPlayer.hole_cards);

                if (ourHand.Hand == Hand.Pair)
                {
                    return game.MaxBet;
                }
                else
                {
                    return 0;
                }
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

