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

        public MonsterCard(Monster monster, int damage, Element element): base(damage, element)
        {
            MonsterType = monster;
            Name = ElementType.ToString()+MonsterType.ToString();
        }

        public MonsterCard? PlayMonsterRound(MonsterCard opponent)
        {

            return null;
        }

        public Card? PlayAgainstSpell(SpellCard opponent)
        {
            return null;
        }

    }
}
