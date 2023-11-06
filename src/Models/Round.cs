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
    }
}
