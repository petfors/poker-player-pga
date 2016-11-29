using System;
using Nancy.Simple.Model;
using Newtonsoft.Json.Linq;

namespace Nancy.Simple
{
    public static class PokerPlayer
    {
        public static readonly string VERSION = "1.0";

        public static int BetRequest(JObject gameState)
        {
            try
            {
                var gameStateObject = gameState.ToObject<GameState>();
                var game = new Game(gameStateObject);
                return game.MinBet;
            }
            catch (Exception ex)
            {
                
                throw;
            }
		}

		public static void ShowDown(JObject gameState)
		{
			//TODO: Use this method to showdown
		}
	}
}

