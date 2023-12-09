using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.Models
{
    public class Battle
    {
        //Cannot change players during a battle!!
        private readonly User _player1;
        private readonly User _player2;

        public int Player1Wins { get; set; } = 0;
        public int Player2Wins { get; set; } = 0;
        public string BattleLog { get; set; } = "";

        public Battle(User Player1, User Player2)
        {
            _player1 = Player1;
            _player2 = Player2;
        }

        public User PlayBattle()
        {
            for(int i = 0; i < 100; i++)
            {
                Round newRound = new Round(_player1, _player2);

                newRound.PlayRound();
                //Draw
                if(newRound.Winner == null)
                {
                    continue;
                }
                //Decide Winner
                if(newRound.Winner == _player1)
                {
                    Player1Wins++;
                }
                else if(newRound.Winner == _player2)
                {
                    Player2Wins++;
                }
                //Update Battle log with round
                UpdateBattleLog(newRound.RoundLog);
            }

        }

        public void UpdateBattleLog(string roundLog)
        {
            BattleLog += roundLog + "\n";
        }

        public bool PlayerLost()
        {
            if(_player1.)
            return false;
        }

    }
}