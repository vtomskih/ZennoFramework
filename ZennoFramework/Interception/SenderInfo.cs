using System;
using System.Collections.Generic;

namespace ZennoFramework.Interception
{
    public class SenderInfo
    {
        public string FullName { get; set; }
        public List<Type> StackTraceTypes { get; set; }
    }
}