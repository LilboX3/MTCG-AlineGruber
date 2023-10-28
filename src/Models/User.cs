﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    internal class User
    {
        public int Coins { get; set; }
        public string Name { get; set; }
        public int EloValue { get; set; }

        private Deck _userDeck;
            
        
        public User(string Username) {
            Coins = 20;
            Name = Username;
            EloValue = 100;
        }

        public void PayCard()
        {
            Coins -= 5;
        }


    }
}