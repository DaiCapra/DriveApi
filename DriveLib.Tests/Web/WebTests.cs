using DriveLib.Ioc;
using NUnit.Framework;

namespace DriveLib.Tests.Web
{
    [TestFixture]
    public class WebTests
    {
        [SetUp]
        public void Setup()
        {
            var installer = new Installer();
            installer.Install();
        }

        [Test]
        public void Test1()
        {
            Assert.True(true);
        }
    }
}