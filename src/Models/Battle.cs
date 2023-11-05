using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.Models
{
    internal class Battle
    {
        //Cannot change players during a battle!!
        private readonly User _player1;
        private readonly User _player2;

        public Battle(User Player1, User Player2)
        {
            _player1 = Player1;
            _player2 = Player2;
        }



    }
}