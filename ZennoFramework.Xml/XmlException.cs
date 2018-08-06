using System;

namespace ZennoFramework.Xml
{
    public class XmlException : System.Exception
    {
        public int LineNumber { get; }
        public int LinePosition { get; }

        public XmlException(string message) : base(message)
        {
        }

        public XmlException(string message, int lineNumber, int linePosition) : this(message)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }
    }
}