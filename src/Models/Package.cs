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

        public MonsterCard GenerateRandomMonster()
        {
            Random rand = new Random();

            //get amount of element enums, generate a random element
            var elementCount = Enum.GetNames(typeof(Element)).Length;
            int elementIndex = rand.Next(0, elementCount);
            Element element = (Element)elementIndex;

            //get amount of monster enums, generate a random monster
            var monsterCount = Enum.GetNames(typeof(Monster)).Length;
            int monsterIndex = rand.Next(0, monsterCount);
            Monster monster = (Monster)monsterIndex;

            //multiplied by 5, max of 100 damage?
            int damage = rand.Next(1, 21)*5;
            
            MonsterCard RandMonster = new MonsterCard(monster, damage, element);
            return RandMonster;
        }

        public SpellCard GenerateRandomSpell()
        {

        }

        public Card GenerateCard()
        {
            Random rand = new Random();
            int isSpell = rand.Next(0, 2);

            if(isSpell == 0) {
                return GenerateRandomSpell();
            } else
            {
                return GenerateRandomMonster();
            }

        }


    }
}
