using Microsoft.SharePoint.Client;
using System;
using System.Xml.Linq;

namespace OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.Utilities
{
    public static class FieldUtilities
    {
        public static string FixLookupField(string fieldXml, Web web, TokenParser parser)
        {
            var fieldElement = XElement.Parse(parser.ParseString( fieldXml));
            var fieldType = (string)fieldElement.Attribute("Type");
            if (fieldType == "Lookup" || fieldType == "LookupMulti")
            {
                var listAttr = (string)fieldElement.Attribute("List");
                Guid g;
                if (!Guid.TryParse(listAttr, out g))
                {
                    var targetList = web.GetList($"{web.EnsureProperty(w => w.ServerRelativeUrl).TrimEnd('/')}/{listAttr}");
                    fieldElement.SetAttributeValue("List", targetList.EnsureProperty(l => l.Id).ToString("B"));
                    return fieldElement.ToString();
                }
            }

            return fieldXml;
        }
    }
}
