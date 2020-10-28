using System.Xml.Linq;

namespace Elevenworks
{
    public static class XElementExtensions
    {
        public static bool HasAttribute(this XElement target, string name)
        {
            return target.Attribute(name) != null;
        }

        public static string GetAttribute(this XElement target, string name)
        {
            var attribute = target.Attribute(name);
            return attribute?.Value;
        }

        public static void SetAttribute(this XElement target, string name, string value)
        {
            target.SetAttributeValue(name, value);
        }
    }
}