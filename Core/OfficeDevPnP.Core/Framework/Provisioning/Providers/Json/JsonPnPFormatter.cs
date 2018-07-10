﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Provisioning.Providers.Json
{
    public class JsonPnPFormatter : ITemplateFormatter
    {
        private TemplateProviderBase _provider;

        public void Initialize(TemplateProviderBase provider)
        {
            this._provider = provider;
        }

        public bool IsValid(System.IO.Stream template)
        {
            // We do not provide JSON validation capabilities
            return true;
        }

        public System.IO.Stream ToFormattedTemplate(Model.ProvisioningTemplate template)
        {
            var jsonString = JsonConvert.SerializeObject(template, new BasePermissionsConverter());
            var jsonBytes = System.Text.Encoding.Unicode.GetBytes(jsonString);
            var jsonStream = new MemoryStream(jsonBytes);
            jsonStream.Position = 0;

            return jsonStream;
        }

        public Model.ProvisioningTemplate ToProvisioningTemplate(System.IO.Stream template)
        {
            return this.ToProvisioningTemplate(template, null);
        }

        public Model.ProvisioningTemplate ToProvisioningTemplate(System.IO.Stream template, string identifier)
        {
            var sr = new StreamReader(template, Encoding.Unicode);
            var jsonString = sr.ReadToEnd();
            var result = JsonConvert.DeserializeObject<Model.ProvisioningTemplate>(jsonString, new BasePermissionsConverter());
            return result;
        }
    }
}
