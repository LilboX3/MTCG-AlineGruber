using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG;
using MTCG.Models;

namespace MTCG.test
{
    [TestFixture]
    internal class TestModels
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestUserCreation()
        {
            Credentials credentials = new Credentials("testuser", "testpassword");
            User testUser = new User(credentials.Username, credentials.Password);

            Assert.That(testUser, Is.Not.Null);

            Console.WriteLine("User: "+testUser.UserCredentials.Username+" password: "+testUser.UserCredentials.Password);
            Console.WriteLine("Credentials: " + credentials.Username + " Password: " + credentials.Password);

            Assert.That(testUser.UserCredentials, Is.EqualTo(credentials));
        }

        [Test]
        public void TestPackage()
        {
            Package package = new Package();
            Assert.That(package.Cards, Is.Not.Null);
        }

        [Test]
        public void TestMonsterCard()
        {
            MonsterCard card = new MonsterCard(Monster.Knight, 100, "123", Element.Fire);
            Assert.That(card.Name.Equals("FireKnight"));
            Assert.That(card.Damage, Is.EqualTo(100));
        }

        [Test]
        public void TestSpellCard()
        {
            SpellCard card = new SpellCard(100, "123", Element.Water);
            Assert.That(card.Name.Equals("WaterSpell"));
            Assert.That(card.Damage, Is.EqualTo(100));
        }

        [Test]
        public void TestCardId()
        {
            Card card = new MonsterCard(Monster.Elf, 100, "thisisanid", Element.Regular);
            Assert.That(card.Id , Is.EqualTo("thisisanid"));
        }

        [Test]
        public void TestStack()
        {
            Stack stack = new Stack();
            Package package = new Package();
            package.GeneratePackage();

            for (int i = 0; i < package.Cards.Length; i++)
            {
                stack.AddCard(package.Cards[i]);
            }

            foreach(Card card in stack.UserStack)
            {
                Console.WriteLine(card.ToString());
                Assert.That(card, Is.Not.Null);
            }
        }

        [TearDown]
        public void Teardown()
        {
            //Wird nach jedem Test ausgeführt
        }
    }
}
