using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.Models
{
    public class MonsterCard: Card
    {
        public Monster MonsterType { get; set; }

        public MonsterCard(Monster monster, int damage, string id, Element element): base(damage, element, id)
        {
            MonsterType = monster;
            Name = ElementType.ToString()+MonsterType.ToString();
        }

    }
}
