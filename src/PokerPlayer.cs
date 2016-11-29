using System;
using System.Linq;
using System.Runtime.InteropServices;
using Nancy.Simple.Model;
using Newtonsoft.Json.Linq;

namespace Nancy.Simple
{
    public static class PokerPlayer
    {
        public static readonly string VERSION = "5.0";

        public static int BetRequest(JObject gameState)
        {
            try
            {
                var gameStateObject = gameState.ToObject<GameState>();
                var game = new Game(gameStateObject);

                var betManager = new BetManager(new HandManager(), game);

                var currentBet = betManager.GetCurrentBet(game.OurCards);

                return currentBet;
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

