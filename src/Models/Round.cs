using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class Round
    {
        //Cannot change players during a round!!
        private readonly User _player1;
        private readonly User _player2;
        public User? Winner { get; set; }
        public Round(User player1, User player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public User PlayRound()
        {
            //Set up random cards of players
            Card player1Card = _player1.PlayCard();
            Card player2Card = _player2.PlayCard();


        }

        public string ToString()
        {

        }
    }
}
