using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class RuleHandler
    {
        public Card player1Card { get; set; }
        public Card player2Card { get; set;}
        private Card? _winningCard;

        public RuleHandler(Card player1, Card player2)
        {
            if (player1 == null || player2 == null)
            {
                throw new ArgumentNullException("A card is null!");
            }
            this.player1Card = player1;
            this.player2Card = player2;
            _winningCard = null;
        }

        public bool MonsterRuleApplies()
        {
            var p1 = player1Card as MonsterCard;
            var p2 = player2Card as MonsterCard;

            if (p1.MonsterType == Monster.Goblin && p2.MonsterType == Monster.Dragon)
            {
                _winningCard = p2;
                return true;
            }
            else if (p1.MonsterType == Monster.Dragon && p2.MonsterType == Monster.Goblin)
            {
                _winningCard = p1;
                return true;
            }
            else if (p1.MonsterType == Monster.Ork && p2.MonsterType == Monster.Wizzard)
            {
                _winningCard = p2;
                return true;
            }
            else if (p1.MonsterType == Monster.Wizzard && p2.MonsterType == Monster.Ork)
            {
                _winningCard = p1;
                return true;
            }
            else if (p1.MonsterType == Monster.Dragon && (p2.MonsterType == Monster.Elf && p2.ElementType == Element.Fire))
            {
                _winningCard = p2;
                return true;
            }
            else if ((p1.MonsterType == Monster.Elf&&p1.ElementType==Element.Fire) && p2.MonsterType == Monster.Dragon)
            {
                _winningCard = p1;
                return true;
            }
            
            return false;
        }

        public bool MixedRuleApplies()
        {
            if(player1Card is MonsterCard) {
                var p1 = player1Card as MonsterCard;
                var p2 = player2Card as SpellCard;

            } 
            else
            {
                var p1 = player1Card as SpellCard;
                var p2 = player2Card as MonsterCard;
            }

            return false;
        }

        public MonsterCard? PlayMonsterRule()
        {   
            return _winningCard as MonsterCard;
        }

        public Card? PlayMixedRule()
        {
            if(_winningCard is MonsterCard)
            {
                return _winningCard as MonsterCard;
            }

            return _winningCard as SpellCard;
            
        }

    }
}
