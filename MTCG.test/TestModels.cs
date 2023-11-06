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
            //Wird vor jedem test ausgeführt

        }

        [Test]
        public void TestRandomCards()
        {
            Package testPackage = new Package();

            Card randCard = testPackage.GenerateCard();

            Console.WriteLine(randCard.ToString());

            Assert.That(randCard, Is.Not.Null);

        }

        [Test]
        public void TestPackage()
        {
            Package package = new Package();
            for(int i = 0; i<package.Cards.Length; i++)
            {
                Console.WriteLine(package.Cards[i].ToString());
                Assert.That(package.Cards[i], Is.Not.Null);
            }
        }

        [Test]
        public void TestStack()
        {
            Stack stack = new Stack();
            Package package = new Package();

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
