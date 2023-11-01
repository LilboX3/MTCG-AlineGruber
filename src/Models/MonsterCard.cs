using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.Models
{
    internal class MonsterCard: Card
    {
        public Monster MonsterType { get; set; }

        public MonsterCard(): base()
        {

        }
    }
}
