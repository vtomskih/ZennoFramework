using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using ZennoFramework.Xml.Validation;

namespace ZennoFramework.Xml.Tests
{
    [SetUpFixture]
    public class Config
    {
        public static List<XDocument> Docs { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            var docs = Loader.Load(@"D:\Dev\ZennoFramework\ZennoFramework.Xml.Tests\_data\Elements.xml");
            Validator.CheckDocuments(docs);
            LocatorRenderer.Run(docs);

            Docs = docs.Values.ToList();
        }
    }
}