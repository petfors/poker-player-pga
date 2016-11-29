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

        public int BettingIndex
        {
            get { return _gameState.bet_index; }
        }

        public IEnumerable<Card> OurCards
        {
            get { return Enumerable.Concat(OurPlayer.hole_cards, _gameState.community_cards); }
        }

        public int MinBet
        {
            get
            {
                return _gameState.current_buy_in - OurPlayer.bet;
            }
        }

        public int CurrentPotSize
        {
            get { return _gameState.pot; }
        }

        public int ActivePlayers
        {
            get { return _gameState.players.Count(p => p.status == "active"); }
        }

        public IEnumerable<Player> OtherAllInPlayers
        {
            get { return _gameState.players.Where(p => p.stack == 0 && p.status == "active"); }
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
