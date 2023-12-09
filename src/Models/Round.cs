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
        public User? Loser { get; set; }
        public string RoundLog { get; set; }
        public Round(User player1, User player2)
        {
            if(_player1 == null || _player2 == null)
            {
                throw new ArgumentNullException("player is null!");
            }
            _player1 = player1;
            _player2 = player2;
            RoundLog = "";
        }
        //TODO: Gewinner bekommt Karte
        //TODO: Round log schreiben
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

            //Loser loses his card
            if(Winner != null)
            {
                CardGain();
            }

        }
        //Winner gets card from loser
        public void CardGain()
        {
            if (Loser == null)
            {
                throw new ArgumentNullException("Loser is null!");
            }
            Card lostCard = Loser.LoseCard();
            Winner.WinCard(lostCard);
        }

        //Normal spell round
        public User? SpellRound(SpellCard player1Card, SpellCard player2Card)
        {
            SpellCard? winnerCard;

            winnerCard = PlaySpellRound(player1Card, player2Card);

            //Decide winning player
            return WinningPlayer(winnerCard, player1Card, player2Card);
        }

        private SpellCard? PlaySpellRound(SpellCard player1Card, SpellCard player2Card)
        {
            int card1Damage = player1Card.CalcDamageAgainst(player2Card.ElementType);
            int card2Damage = player2Card.CalcDamageAgainst(player1Card.ElementType);

            if(card1Damage > card2Damage)
            {
                return player1Card;
            }
            else if(card2Damage > card1Damage)
            {
                return player2Card;
            }
            //same damage
            return null;
        }

        public User? MixedRound(Card player1Card, Card player2Card)
        {
            Card? winnerCard;
            RuleHandler ruleHandler = new RuleHandler(player1Card, player2Card);
            
            if (ruleHandler.MixedRuleApplies())
            {
                winnerCard = ruleHandler.PlayMixedRule();
            } else
            {
                winnerCard = PlayMixedRound(player1Card, player2Card);
            }
            //Decide winning player
            return WinningPlayer(winnerCard, player1Card, player2Card);
        }

        private Card? PlayMixedRound(Card player1Card, Card player2Card)
        {
            int card1Damage = player1Card.CalcDamageAgainst(player2Card.ElementType);
            int card2Damage = player2Card.CalcDamageAgainst(player1Card.ElementType);

            if (card1Damage > card2Damage)
            {
                return player1Card;
            }
            else if (card2Damage > card1Damage)
            {
                return player2Card;
            }
            //same damage
            return null;
        }

        public User? MonsterRound(MonsterCard player1Card, MonsterCard player2Card)
        {
            MonsterCard? winnerCard;
            RuleHandler ruleHandler = new RuleHandler(player1Card, player2Card);
            //Apply extra rules
            if (ruleHandler.MonsterRuleApplies())
            {
                winnerCard = ruleHandler.PlayMonsterRule();
            } else
            {
                winnerCard = PlayMonsterRound(player1Card, player2Card);
            }

            //Decide winning player
            return WinningPlayer(winnerCard, player1Card, player2Card);
        }
        //Normal Monster round
        private MonsterCard? PlayMonsterRound(MonsterCard player1Card, MonsterCard player2Card)
        {
            if(player1Card.Damage > player2Card.Damage)
            {
                return player1Card;
            }
            else if(player2Card.Damage > player1Card.Damage)
            {
                return player2Card;
            }
            //Same damage: draw
            return null;
        }

        public User? WinningPlayer(Card? winnerCard, Card player1Card, Card player2Card)
        {
            //If draw
            if(winnerCard == null)
            {
                return null;
            }
            //Decide winning player
            else if (winnerCard == player1Card)
            {
                Loser = _player2;
                return _player1;
            }
            else if (winnerCard == player2Card)
            {
                Loser = _player1;
                return _player2;
            }

            return null;
        }


    }
}
