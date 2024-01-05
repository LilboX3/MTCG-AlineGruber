using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class Package
    {
        public Card?[] Cards { get; set; }
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
            int id = rand.Next(10, 10000);
            
            MonsterCard RandMonster = new MonsterCard(monster, damage, id.ToString(), element);
            return RandMonster;
        }

        public SpellCard GenerateRandomSpell()
        {
            Random rand = new Random();

            //get amount of element enums, generate a random element
            var elementCount = Enum.GetNames(typeof(Element)).Length;
            int elementIndex = rand.Next(0, elementCount);
            Element element = (Element)elementIndex;

            //multiplied by 5, max of 100 damage?
            int damage = rand.Next(1, 21) * 5;
            int id = rand.Next(10, 10000);

            SpellCard RandSpell = new SpellCard(damage, id.ToString(), element);
            return RandSpell;
        }

        public Card GenerateCard()
        {
            Random rand = new Random();
            int isSpell = rand.Next(0, 2);

            if(isSpell == 0) {
                return GenerateRandomSpell();
            }
            else
            {
                return GenerateRandomMonster();
            }

        }

        public void AddCard(Card card)
        {
            if (Cards.Length > 5)
            {
                throw new Exception("Package too big?");
            }

            for (var i = 0; i < Cards.Length; i++)
            {
                Cards[i] ??= card;
            }
        }

        public void GeneratePackage()
        {
            for(int i= 0; i < Cards.Length; i++)
            {
                Cards[i] = GenerateCard();
            }
        }


    }
}
