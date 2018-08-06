using System;
using System.Linq;
using System.Reflection;
using ZennoFramework.Exceptions;

namespace ZennoFramework.Infrastructure.Elements.Internal
{
    internal class ElementCollectionChecker
    {
        private readonly Type _collectionItemtype;

        public ElementCollectionChecker(Type collectionItemtype)
        {
            _collectionItemtype = collectionItemtype;
        }

        public void ItemRequiredCtor(out int ctorParametersCount)
        {
            if (!IsTElementContainsRequiredCtor(_collectionItemtype, out ctorParametersCount))
            {
                var elementTypeName = _collectionItemtype.FullName;
                var collectionTypeName = $"ElementCollection<{elementTypeName}>";
                var error = $"Не удается создать экземпляр коллекции типа {collectionTypeName}." +
                            $"У элемента коллекции {elementTypeName} не найден конструктор, " +
                            $"содержащий 1 обязательный параметр типа {nameof(BotContext)}.";
                throw new ZennoFrameworkException(error);
            }
        }

        public void ItemDescendantOfElement()
        {
            if (!IsTElementDescendantOfElement(_collectionItemtype))
            {
                var elementTypeName = _collectionItemtype.FullName;
                var collectionTypeName = $"ElementCollection<{elementTypeName}>";
                var baseElementTypeName = "Element<TElement>";
                var error = $"Не удается создать экземпляр коллекции типа {collectionTypeName}." +
                            $"Элемент коллекции {elementTypeName} должен являться потомком {baseElementTypeName}.";
                throw new ZennoFrameworkException(error);
            }
        }

        private bool IsTElementContainsRequiredCtor(Type type, out int ctorParametersCount)
        {
            ctorParametersCount = 0;
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var ctors = type.GetConstructors(flags);

            foreach (var ctor in ctors)
            {
                var parameters = ctor.GetParameters();

                if (parameters.Length > 0)
                {
                    var paramType = parameters.First().ParameterType;
                    var firstParamIsContext = paramType == typeof(BotContext);

                    if (!firstParamIsContext)
                    {
                        while (paramType.BaseType != null)
                        {
                            paramType = paramType.BaseType;
                            if (paramType == typeof(BotContext))
                            {
                                firstParamIsContext = true;
                                break;
                            }
                        }
                    }

                    if (!firstParamIsContext)
                        continue;

                    if (parameters.Length == 1)
                    {
                        ctorParametersCount = 1;
                        return true;
                    }

                    if (parameters.Length > 1)
                    {
                        var secondParam = parameters[1];
                        if (secondParam.HasDefaultValue)
                        {
                            ctorParametersCount = parameters.Length;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool IsTElementDescendantOfElement(Type type)
        {
            var baseType = type.BaseType;

            while (baseType != null && baseType != typeof(Elements.Element))
            {
                baseType = baseType.BaseType;
            }

            return baseType != null;
        }
    }
}