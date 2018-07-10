using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Provisioning.Providers.Json
{
    internal class BasePermissionsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Microsoft.SharePoint.Client.BasePermissions).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result =
                new Microsoft.SharePoint.Client.BasePermissions();

            var token = JToken.Load(reader);
            var basePermissionString = token.ToString();

            if (!String.IsNullOrEmpty(basePermissionString))
            {
                // Is it an int value (for backwards compability)?
                var permissionInt = 0;
                if (int.TryParse(basePermissionString, out permissionInt))
                {
                    result.Set((Microsoft.SharePoint.Client.PermissionKind)permissionInt);
                }
                else
                {
                    foreach (var pk in basePermissionString.Split(new char[] { ',' }))
                    {
                        var permissionKind =
                            Microsoft.SharePoint.Client.PermissionKind.AddAndCustomizePages;
                        if (Enum.TryParse(basePermissionString, out permissionKind))
                        {
                            result.Set(permissionKind);
                        }
                    }
                }
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            String jsonValue = null;

            var basePermissions = 
                value as Microsoft.SharePoint.Client.BasePermissions;
            if (basePermissions != null)
            {
                var permissions = new List<String>();
                foreach (var pk in (Microsoft.SharePoint.Client.PermissionKind[])Enum.GetValues(typeof(Microsoft.SharePoint.Client.PermissionKind)))
                {
                    if (basePermissions.Has(pk) && pk !=
                        Microsoft.SharePoint.Client.PermissionKind.EmptyMask)
                    {
                        permissions.Add(pk.ToString());
                    }
                }
                jsonValue = string.Join(",", permissions.ToArray());
            }

            writer.WriteValue(jsonValue);
        }
    }
}
