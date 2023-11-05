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

        }

        [TearDown]
        public void Teardown()
        {
            //Wird nach jedem Test ausgeführt
        }
    }
}
