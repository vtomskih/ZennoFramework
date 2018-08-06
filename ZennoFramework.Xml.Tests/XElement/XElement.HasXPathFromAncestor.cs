using System.Linq;
using NUnit.Framework;
using ZennoFramework.Xml.Extensions;

namespace ZennoFramework.Xml.Tests.XElement
{
    [TestFixture]
    public class XElement_HasXPathFromAncestor
    {
        [Test]
        public void Elements()
        {
            var elements = Config.Docs[0].Root.Descendants().ToList();

            Assert.False(elements[0].HasXPathFromAncestor());
            Assert.False(elements[1].HasXPathFromAncestor());
        }

        [Test]
        public void Popups()
        {
            var elements = Config.Docs[2].Root.Descendants().ToList();

            Assert.False(elements[0].HasXPathFromAncestor());
            Assert.True(elements[1].HasXPathFromAncestor());
            Assert.True(elements[2].HasXPathFromAncestor());
        }

        [Test]
        public void Buttons()
        {
            var elements = Config.Docs[3].Root.Descendants().ToList();

            Assert.False(elements[0].HasXPathFromAncestor());
            Assert.False(elements[1].HasXPathFromAncestor());
            Assert.False(elements[2].HasXPathFromAncestor());
            Assert.True(elements[3].HasXPathFromAncestor());
            Assert.True(elements[4].HasXPathFromAncestor());
            Assert.True(elements[5].HasXPathFromAncestor());
        }

        [Test]
        public void Wall()
        {
            var elements = Config.Docs[4].Root.Descendants().ToList();

            Assert.False(elements[0].HasXPathFromAncestor());
            Assert.False(elements[1].HasXPathFromAncestor());
            Assert.False(elements[2].HasXPathFromAncestor());
            Assert.False(elements[3].HasXPathFromAncestor());
        }

        [Test]
        public void Comments()
        {
            var elements = Config.Docs[5].Root.Descendants().ToList();

            Assert.False(elements[0].HasXPathFromAncestor());
            Assert.True(elements[1].HasXPathFromAncestor());
            Assert.False(elements[2].HasXPathFromAncestor());
            Assert.False(elements[3].HasXPathFromAncestor());
            Assert.False(elements[4].HasXPathFromAncestor());
            Assert.True(elements[5].HasXPathFromAncestor());
            Assert.True(elements[6].HasXPathFromAncestor());
            Assert.True(elements[7].HasXPathFromAncestor());
        }
    }
}