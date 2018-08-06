using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ZennoFramework.Api.Common.Messages;
using ZennoFramework.Generator;
using ZennoFramework.Generator.Extensions;
using ZennoFramework.Xml;
using ZennoFramework.Xml.Extensions;
using ZennoFramework.Xml.Validation;

namespace XmlParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"D:\Dev\ZennoFramework\XmlParser\_elements\Elements.xml";

            var docs = Loader.Load(path);
            Validator.CheckDocuments(docs);
            LocatorRenderer.Run(docs);
            var docsString = docs.Values.Select(x => x.Save());
            var data = new GenerationData{Namespace = "", XmlDocuments = docsString};
            var code = CodeGenerator.Generate(data);
            //Console.WriteLine(code);

            foreach (var document in docs.Values)
            {
                WriteElements(document.Root, "  ");
            }

            Console.ReadKey();
        }

        private static void WriteElements(XElement element, string indent = "")
        {
            Console.WriteLine($"{indent}{element.Name.LocalName} [{element.Attribute("Key")}][{element.Attribute("x")}]");

            foreach (var child in element.Elements())
            {
                WriteElements(child, indent + "    ");
            }
        }
    }
}
