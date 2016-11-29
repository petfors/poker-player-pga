using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Simple.Model
{
    public class Game
    {
        private GameState _gameState;

        public Game(GameState gameState)
        {
            _gameState = gameState;
        }

        public Player OurPlayer
        {
            get
            {
                return _gameState.players[_gameState.in_action];
            }
        }

        public int MinBet
        {
            get
            {
                return _gameState.current_buy_in - OurPlayer.bet;
            }
        }
       

        public int MaxBet
        {
            get
            {
                return OurPlayer.stack;
            }
        }

        public int MinRaise
        {
            get { return _gameState.current_buy_in - OurPlayer.bet + _gameState.minimum_raise; }
        }
    }
}
