using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class Package
    {
        public Card[] Cards { get; set; }
        public Package() { 
            Cards = new Card[5];
        }

        public void GenerateCards()
        {

        }


    }
}
