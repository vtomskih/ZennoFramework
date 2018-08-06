using NUnit.Framework;
using ZennoFramework.Generation.Tests;

namespace ZennoFramework.Xml.Tests.Check.XPath
{
    [TestFixture]
    public class Main
    {
        [Test]
        public void Items()
        {
            var doc = Config.Docs[1];

            Assert.AreEqual(doc.GetXPath("Items.Item1"), "//Items//Item1");
            Assert.AreEqual(doc.GetXPath("Items.Item2"), "//Items//Item2");
            Assert.AreEqual(doc.GetXPath("Items.Item3"), "//Item3");
            Assert.AreEqual(doc.GetXPath("Items.Item4"), "//Item4");
            Assert.AreEqual(doc.GetXPath("Items.Item5"), "//Items//Item5");
            Assert.AreEqual(doc.GetXPath("Items.Item6"), "//Items//Item6");
            Assert.AreEqual(doc.GetXPath("Items.Item7"), "//Item7");
            Assert.AreEqual(doc.GetXPath("Items.Item8"), "//Item8");
            Assert.AreEqual(doc.GetXPath("Items.Item9"), "//Items//Item9");
            Assert.AreEqual(doc.GetXPath("Items.Item10"), "//Items//Item10");
            Assert.AreEqual(doc.GetXPath("Items.Item11"), "//Item11");
            Assert.AreEqual(doc.GetXPath("Items.Item12"), "//Item12");
        }
        
        [Test]
        public void Container_1_2_7_8()
        {
            var doc = Config.Docs[1];
            var containers = new[] {1, 2, 7, 8};

            foreach (var n in containers)
            {
                var container = $"Container{n}";

                Assert.AreEqual(doc.GetXPath($"{container}"), $"//{container}");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item1"), $"//{container}//Items//Item1");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item2"), $"//{container}//Items//Item2");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item3"), $"//{container}//Item3");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item4"), $"//{container}//Item4");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item5"), $"//{container}//Items//Item5");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item6"), $"//{container}//Items//Item6");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item7"), $"//{container}//Item7");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item8"), $"//{container}//Item8");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item9"), $"//{container}//Items//Item9");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item10"), $"//{container}//Items//Item10");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item11"), $"//{container}//Item11");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item12"), $"//{container}//Item12");
            }
        }

        [Test]
        public void Container_3_5_6()
        {
            var doc = Config.Docs[1];
            var containers = new[] {"Container3", "Container5", "Container6"};

            foreach (var container in containers)
            {
                Assert.AreEqual(doc.GetXPath($"{container}"), $"//{container}");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item1"), "//Items//Item1");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item2"), "//Items//Item2");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item3"), "//Item3");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item4"), "//Item4");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item5"), "//Items//Item5");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item6"), "//Items//Item6");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item7"), "//Item7");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item8"), "//Item8");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item9"), "//Items//Item9");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item10"), "//Items//Item10");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item11"), "//Item11");
                Assert.AreEqual(doc.GetXPath($"{container}.Items.Item12"), "//Item12");
            }
        }
    }
}