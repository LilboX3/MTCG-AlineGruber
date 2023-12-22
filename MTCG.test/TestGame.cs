using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.test
{
    [TestFixture]
    internal class TestGame
    {
        User player1;
        User player2;

        [SetUp]
        public void Setup()
        {
            player1 = new User("name1", "password1");
            player2 = new User("name2", "password2");
            player1.BuyPackage();
            player2.BuyPackage();
            player1.FillDeckRandom();
            player2.FillDeckRandom();
        }

        [Test]
        public void TestRoundWinner()
        {
            Round newRound = new Round(player1, player2);
            newRound.PlayRound();
            User? winner = newRound.Winner;
            Assert.That(winner == player1 || winner == player2 || winner == null);
        }

        [Test]
        public void TestRoundExists()
        {
            Round round = new Round(player1, player2);
            Assert.That(round, Is.Not.Null);
        }

        [Test]
        public void TestBattleExists()
        {
            Battle battle = new Battle(player1, player2);
            Assert.That(battle, Is.Not.Null);
        }

        [Test]
        public void TestBattleWinner()
        {
            Battle battle = new Battle(player1, player2);
            battle.PlayBattle();
            User? winner = battle.DecideWinner();
            Assert.That(winner == player1 || winner == player2 || winner == null);
        }

        [Test]
        public void TestGoblinDragonRule()
        {
            MonsterCard goblin = new MonsterCard(Monster.Goblin, 90, Element.Regular);
            MonsterCard dragon = new MonsterCard(Monster.Dragon, 40, Element.Regular);
            RuleHandler rule = new RuleHandler(goblin, dragon);
            Assert.Multiple(() =>
            {
                Assert.That(rule.MonsterRuleApplies(), Is.EqualTo(true));
                Assert.That(rule.PlayMonsterRule(), Is.EqualTo(dragon));
            });
        }

        [Test]
        public void TestOrkWizzardRule()
        {
            MonsterCard wizzard = new MonsterCard(Monster.Wizzard, 10, Element.Regular);
            MonsterCard ork = new MonsterCard(Monster.Ork, 100, Element.Regular);
            RuleHandler rule = new RuleHandler(wizzard, ork);
            Assert.Multiple(() =>
            {
                Assert.That(rule.MonsterRuleApplies(), Is.EqualTo(true));
                Assert.That(rule.PlayMonsterRule(), Is.EqualTo(wizzard));
            });
        }

        [Test]
        public void TestKnightWaterSpellRule()
        {
            MonsterCard knight = new MonsterCard(Monster.Knight, 100, Element.Regular);
            SpellCard waterSpell = new SpellCard(10, Element.Water);
            RuleHandler rule = new RuleHandler(knight, waterSpell);
            Assert.Multiple(() =>
            {
                Assert.That(rule.MixedRuleApplies(), Is.EqualTo(true));
                Assert.That(rule.PlayMixedRule(), Is.EqualTo(waterSpell));
            });
        }

        [Test]
        public void TestKrakenImmunity()
        {
            MonsterCard kraken = new MonsterCard(Monster.Kraken, 10, Element.Regular);
            SpellCard spell = new SpellCard(500, Element.Fire);
            RuleHandler rule = new RuleHandler(kraken, spell);
            Assert.Multiple(() => 
            {
                Assert.That(rule.MixedRuleApplies(), Is.EqualTo(true));
                Assert.That(rule.PlayMixedRule(), Is.EqualTo(kraken));
            });
        }

        [Test]
        public void TestDragonFireElfRule()
        {
            MonsterCard fireElf = new MonsterCard(Monster.Elf, 10, Element.Fire);
            MonsterCard dragon = new MonsterCard(Monster.Dragon, 500, Element.Water);
            RuleHandler rule = new RuleHandler(fireElf, dragon);
            Assert.Multiple(() =>
            {
                Assert.That(rule.MonsterRuleApplies(), Is.EqualTo(true));
                Assert.That(rule.PlayMonsterRule(), Is.EqualTo(fireElf));
            });
        }
    }
}
