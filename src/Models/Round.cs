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

        //Null if theres a draw
        public User? Winner { get; set; }
        public string RoundLog { get; set; }
        public Round(User player1, User player2)
        {
            _player1 = player1;
            _player2 = player2;
            RoundLog = "";
        }

        public void PlayRound()
        {
            //Set up random cards of players
            Card player1Card = _player1.PlayCard();
            Card player2Card = _player2.PlayCard();

            if(player1Card == null || player2Card == null)
            {
                throw new Exception("Player card is null");
            }

            if(player1Card is SpellCard && player2Card is SpellCard)
            {
                Winner = SpellRound((SpellCard)player1Card, (SpellCard)player2Card);
            }
            else if(player1Card is MonsterCard && player2Card is MonsterCard)
            {
                Winner = MonsterRound((MonsterCard)player1Card,(MonsterCard)player2Card);
            }
            else
            {
                Winner = MixedRound(player1Card, player2Card);
            }

        }

        public User? SpellRound(SpellCard player1Card, SpellCard player2Card)
        {
            
           
        }

        public User? MixedRound(Card player1Card, Card player2Card)
        {
            Card? winnerCard = null;
            RuleHandler ruleHandler = new RuleHandler(player1Card, player2Card);
            //TODO: 
            if (ruleHandler.MixedRuleApplies())
            {
                winnerCard = ruleHandler.PlayMixedRule();
            } else
            {
                
            }
            return null;
        }

        public User? MonsterRound(MonsterCard player1Card, MonsterCard player2Card)
        {
            MonsterCard? winnerCard = null;
            RuleHandler ruleHandler = new RuleHandler(player1Card, player2Card);
            //Apply extra rules
            if (ruleHandler.MonsterRuleApplies())
            {
                winnerCard = ruleHandler.PlayMonsterRule();
            } else
            {
                winnerCard = player1Card.PlayMonsterRound(player2Card);
            }

            //Decide winning player
            if(winnerCard == player1Card)
            {
                return _player1;
            } 
            else if(winnerCard == player2Card)
            {
                return _player2;
            }
            //If draw
            return null;
        }


    }
}
