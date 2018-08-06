using System;
using System.Text.RegularExpressions;
using ZennoFramework.Generator.Extensions;

namespace ZennoFramework.Generator.Internal
{
    internal static class ElementCollectionBuilder
    {
        public static string Create(Element element)
        {
            var hasParent = !element.IsRoot && !element.NoParent;
            var collectionName = Regex.Replace(element.Name, @"[Ii]tem$", string.Empty) + "Collection";

            var baseName = element.IsImported ? element.GetRef() : string.Empty;
            baseName = element.IsImported && element.IsOverridedXPath ? baseName + collectionName : "ElementCollection";

            var className = $"public partial class {collectionName}<TElement> : {baseName}<TElement>".NewLine();
            var genericConstraints = "    where TElement: class";
            className += genericConstraints;

            return GenerateElementCollectionClass(collectionName, className, element.Key, hasParent);
        }

        private static string GenerateElementCollectionClass(string collectionName, string className, string xpathKey,
            bool hasParent)
        {
            return string.Format(@"{1}
{{{3}
	public {0}(BotContext context{4}) : base(context)
	{{{5}
	}}
    
	protected override ElementCollection Find()
	{{
		return Context.CreateCollection(""{2}""{6});
	}}
}}", collectionName, className, xpathKey,
                hasParent ? Environment.NewLine + "    private readonly Element _parent;".NewLine() : string.Empty,
                hasParent ? ", Element parent = null" : string.Empty,
                hasParent ? Environment.NewLine + "        _parent = parent;" : string.Empty,
                hasParent ? ", _parent" : string.Empty);
        }
    }
}