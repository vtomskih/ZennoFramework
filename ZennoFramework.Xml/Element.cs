using System.Collections.Generic;
using ZennoFramework.Extensions;

namespace ZennoFramework.Xml
{
    public class Element
    {
        public string Name { get; set; }
        public string XPath { get; set; }
        public string Comment { get; set; }
        public bool IsParent { get; set; }
        public bool IsCollection { get; set; }
        public bool NoParent { get; set; }
        public bool NoParentXPath { get; set; }
        
        public Element Parent { get; set; }
        public Dictionary<string, Element> Elements { get; } = new Dictionary<string, Element>();
        
        // TODO: to Internal
        public bool IsRoot { get; set; }
        public bool IsImported { get; set; }
        public string Key { get; set; }
        public string ImportedKey { get; set; }
        public string Namespace { get; set; }
        
        // TODO: to Extensions
        public bool HasXPath => !string.IsNullOrWhiteSpace(XPath);

        public IEnumerable<Element> DescendantsAndSelf()
        {
            var elements = new List<Element> {this};
            Elements.Values.ForEach(e => elements.AddRange(e.DescendantsAndSelf()));
            return elements;
        }
    }
}