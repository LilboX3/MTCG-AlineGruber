using NUnit.Framework.Internal;
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
            if(player1 == null || player2 == null)
            {
                throw new ArgumentNullException("player is null!");
            }
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
            //Debugging try
            Console.WriteLine("**********Loser Deck BEFORE**********\n" );
            Loser.PrintBattleDeck();
            Card lostCard = Loser.LoseCard();
            Console.WriteLine("**********Loser Deck AFTER**********\n");
            Loser.PrintBattleDeck();
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
            float card1Damage = player1Card.CalcDamageAgainst(player2Card.ElementType);
            float card2Damage = player2Card.CalcDamageAgainst(player1Card.ElementType);
            SpellCard? winnerCard = null;

            if(card1Damage > card2Damage)
            {
                winnerCard = player1Card;
            }
            else if(card2Damage > card1Damage)
            {
                winnerCard = player2Card;
            }
            BuildElementLog(winnerCard, player1Card, player2Card, card1Damage, card2Damage);
            //same damage
            return winnerCard;
        }

        public User? MixedRound(Card player1Card, Card player2Card)
        {
            Card? winnerCard;
            RuleHandler ruleHandler = new RuleHandler(player1Card, player2Card);
            
            if (ruleHandler.MixedRuleApplies())
            {
                winnerCard = ruleHandler.PlayMixedRule();
                BuildElementLog(winnerCard, player1Card, player2Card, player1Card.Damage, player2Card.Damage);
            } else
            {
                winnerCard = PlayMixedRound(player1Card, player2Card);
            }
            //Decide winning player
            return WinningPlayer(winnerCard, player1Card, player2Card);
        }

        private Card? PlayMixedRound(Card player1Card, Card player2Card)
        {
            float card1Damage = player1Card.CalcDamageAgainst(player2Card.ElementType);
            float card2Damage = player2Card.CalcDamageAgainst(player1Card.ElementType);
            Card? winnerCard = null;

            if (card1Damage > card2Damage)
            {
                winnerCard = player1Card;
            }
            else if (card2Damage > card1Damage)
            {
                winnerCard = player2Card;
            }
            BuildElementLog(winnerCard, player1Card, player2Card, card1Damage, card2Damage);
            //same damage
            return winnerCard;
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
            BuildMonsterLog(winnerCard, player1Card, player2Card);

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

        public void BuildElementLog(Card? winnerCard, Card player1Card, Card player2Card, float actualDamage1, float actualDamage2)
        {
            string damage1 = player1Card.Damage.ToString();
            string damage2 = player2Card.Damage.ToString();

            string playerA = "PlayerA: " + player1Card.Name + "(" + damage1 + " Damage)";
            string playerB = "PlayerB: " + player2Card.Name + "("+ damage2 + " Damage)";
            string winner = "";

            if(winnerCard != null)
            {
                winner = winnerCard.Name+" wins";
            } else
            {
                winner = "Draw (no action)";
            }

            string log = playerA + " vs " + playerB + " => "+ damage1 + " VS " + damage2 + " -> " + actualDamage1 + " VS " + actualDamage2 + " => " + winner;
            RoundLog = log;
        }

        public void BuildMonsterLog(MonsterCard? winnerCard, MonsterCard player1Card, MonsterCard player2Card)
        {
            string damage1 = player1Card.Damage.ToString();
            string damage2 = player2Card.Damage.ToString();

            string playerA = "PlayerA: " + player1Card.Name + "(" + damage1 + " Damage)";
            string playerB = "PlayerB: " + player2Card.Name + "(" + damage2 + " Damage)";
            string winner = "";

            if (winnerCard != null)
            {
                string winnerMonster = winnerCard.MonsterType.ToString();
                string losingMonster = winnerCard == player1Card ? player2Card.MonsterType.ToString() : player1Card.MonsterType.ToString();
                winner = winnerMonster+ " defeats "+losingMonster;
            }
            else
            {
                winner = "Draw (no action)";
            }

            string log = playerA + " vs " + playerB + " => " + winner;
            RoundLog = log;
        }


    }
}
