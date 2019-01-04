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
                if (!Guid.TryParse(listAttr, out Guid g))
                {
                    var targetList = web.GetListByUrl($"/{listAttr}");
                    if (targetList != null)
                    {
                        fieldElement.SetAttributeValue("List", targetList.Id.ToString("B"));
                        return fieldElement.ToString();
                    }
                }
            }

            return fieldXml;
        }
    }
}
