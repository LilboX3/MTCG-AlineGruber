using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.test
{
    [TestFixture]
    internal class TestUser
    {
        [SetUp]
        public void Setup () 
        {
            //Wird vor jedem test ausgeführt

        }

        [TearDown]
        public void Teardown ()
        {
            //Wird nach jedem Test ausgeführt
        }

        
        [Test]
        public void DeinammaTest()
        {
            Assert.Pass();
        }

    }
}
