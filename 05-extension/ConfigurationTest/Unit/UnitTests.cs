using NUnit.Framework;

namespace ConfigurationTest.Unit
{
    [TestFixture]
    public class UnitTests
    {
        [TestFixtureSetUp]
        public void IniciallizeTests()
        {
            
        }

        [Test]
        public void TryNUnit()
        {
            Assert.IsTrue(true);
        }

        [TestFixtureTearDown]
        public void ClearTestData()
        {
            
        }
    }
}
