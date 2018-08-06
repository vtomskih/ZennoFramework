using System.Collections.Generic;
using ZennoFramework.Xml.Extensions;
using ZennoFramework.Xml.Validation;

namespace ZennoFramework.Xml
{
    public class XmlParser
    {
        public static Dictionary<string, string> GetKeysAndXPaths(string path)
        {
            var docs = Loader.Load(path);
            Validator.CheckDocuments(docs);
            LocatorRenderer.Run(docs);

            var elements = new List<Element>();
            var result = new Dictionary<string, string>();

            foreach (var doc in docs.Values)
                elements.AddRange(doc.ToGenerationElements().DescendantsAndSelf());

            foreach (var element in elements)
            {
                if (!result.ContainsKey(element.Key))
                    result.Add(element.Key, element.XPath);
            }

            return result;
        }
    }
}