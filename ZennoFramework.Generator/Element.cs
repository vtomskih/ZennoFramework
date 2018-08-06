using System.Collections.Generic;
using System.Linq;

namespace ZennoFramework.Generator
{
    internal class Element
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
        public bool IsOverridedXPath => IsImported && Key != ImportedKey;

        public string GetRef()
        {
            var key = ImportedKey;
            var keys = key.Split('.');
            key = keys.Length == 1 ? keys.First() : string.Empty;

            for (var i = 0; i < keys.Length - 1; i++)
            {
                key += keys[i] + "Elements.";
            }

            //if (keys.Length > 1)
            //    key += keys.Last();

            return key;
        }
    }
}