using NUnit.Framework;
using ZennoFramework.Generation.Tests;

namespace ZennoFramework.Xml.Tests.Check.Keys
{
    [TestFixture]
    public class Main
    {
        [Test]
        public void Items()
        {
            var doc = Config.Docs[1];

            for (var i = 1; i <= 12; i++)
            {
                Assert.AreEqual(doc.GetKey($"Items.Item{i}"), $"Main.Items.Item{i}");
            }
        }

        [Test]
        public void Container_1_2_7_8()
        {
            var doc = Config.Docs[1];
            var containers = new[] {"1", "2", "7", "8"};

            foreach (var n in containers)
            {
                Assert.AreEqual(doc.GetKey($"Container{n}"), $"Main.Container{n}");

                for (var i = 1; i <= 12; i++)
                {
                    Assert.AreEqual(doc.GetKey($"Container{n}.Items.Item{i}"), $"Main.Container{n}.Items.Item{i}");
                }
            }
        }

        [Test]
        public void Container_3_5_6()
        {
            var doc = Config.Docs[1];
            var containers = new[] {"3", "5", "6"};

            foreach (var n in containers)
            {
                Assert.AreEqual(doc.GetKey($"Container{n}"), $"Main.Container{n}");

                for (var i = 1; i <= 12; i++)
                {
                    Assert.AreEqual(doc.GetKey($"Container{n}.Items.Item{i}"), $"Main.Items.Item{i}");
                }
            }
        }
    }
}