using System;

namespace ZennoFramework.Generator.Internal
{
    internal static class Header
    {
        public static string Create() =>
$@"/*-----------------------------------------------------
 Этот код был сгенерирован автоматически.
 Все изменения будут утеряны при следующей генерации.
 Дата: {DateTime.Now} 
 ------------------------------------------------------*/

using System.Collections.Generic;
using ZennoFramework;
using ZennoFramework.Infrastructure.Elements;
using ZennoFramework.Xml.Extensions;
";
    }
}