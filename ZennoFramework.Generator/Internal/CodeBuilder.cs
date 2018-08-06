using ZennoFramework.Generator.Extensions;

namespace ZennoFramework.Generator.Internal
{
    internal class CodeBuilder
    {
        private string _code;
        private Element _element;

        public string Build() => _code;

        public CodeBuilder With(Element element)
        {
            _element = element;
            return this;
        }
        
        public CodeBuilder AddElementCollection()
        {
            if (_element.IsCollection)
                _code += ElementCollectionBuilder.Create(_element).NewLine().NewLine();
            return this;
        }

        public CodeBuilder AddClass()
        {
            if (!string.IsNullOrEmpty(_element.Comment))
                _code = _code.NewLineSafe() + Comment.GetComment(_element.Comment).NewLine();
            _code += ClassBuilder.CreateClass(_element);
            return this;
        }

    }
}