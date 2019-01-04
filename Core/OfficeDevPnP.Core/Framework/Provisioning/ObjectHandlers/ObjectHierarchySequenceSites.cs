﻿#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Diagnostics;
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.TokenDefinitions;
using OfficeDevPnP.Core.Sites;
using OfficeDevPnP.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers
{
    internal class ObjectHierarchySequenceSites : ObjectHierarchyHandlerBase
    {
        private List<TokenDefinition> _additionalTokens = new List<TokenDefinition>();
        public override string Name => "Sequences";

        public override ProvisioningHierarchy ExtractObjects(Tenant tenant, ProvisioningHierarchy hierarchy, ProvisioningTemplateCreationInformation creationInfo)
        {
            throw new NotImplementedException();
        }

        public override TokenParser ProvisionObjects(Tenant tenant, Model.ProvisioningHierarchy hierarchy, string sequenceId, TokenParser tokenParser, ProvisioningTemplateApplyingInformation applyingInformation)
        {
            using (var scope = new PnPMonitoredScope(CoreResources.Provisioning_ObjectHandlers_Provisioning))
            {
                var sequence = hierarchy.Sequences.FirstOrDefault(s => s.ID == sequenceId);
                if (sequence != null)
                {
                    var siteUrls = new Dictionary<Guid, string>();

                    TokenParser siteTokenParser = null;


                    foreach (var sitecollection in sequence.SiteCollections)
                    {
                        ClientContext siteContext = null;

                        switch (sitecollection)
                        {
                            case TeamSiteCollection t:
                                {
                                    TeamSiteCollectionCreationInformation siteInfo = new TeamSiteCollectionCreationInformation()
                                    {
                                        Alias = tokenParser.ParseString(t.Alias),
                                        DisplayName = tokenParser.ParseString(t.Title),
                                        Description = tokenParser.ParseString(t.Description),
                                        Classification = tokenParser.ParseString(t.Classification),
                                        IsPublic = t.IsPublic
                                    };
                                    
                                    var groupSiteInfo = Sites.SiteCollection.GetGroupInfo(tenant.Context as ClientContext, siteInfo.Alias).GetAwaiter().GetResult();
                                    if (groupSiteInfo == null)
                                    {
                                        WriteMessage($"Creating Team Site {siteInfo.Alias}", ProvisioningMessageType.Progress);
                                        siteContext = Sites.SiteCollection.Create(tenant.Context as ClientContext, siteInfo);
                                    }
                                    else
                                    {
                                        if (groupSiteInfo.ContainsKey("siteUrl"))
                                        {
                                            WriteMessage($"Using existing Team Site {siteInfo.Alias}", ProvisioningMessageType.Progress);
                                            siteContext = (tenant.Context as ClientContext).Clone(groupSiteInfo["siteUrl"], applyingInformation.AccessTokens);
                                        }
                                    }
                                    if (t.IsHubSite)
                                    {
                                        RegisterAsHubSite(tenant, siteContext.Url, t.HubSiteLogoUrl);
                                    }
                                    if (!string.IsNullOrEmpty(t.Theme))
                                    {
                                        var parsedTheme = tokenParser.ParseString(t.Theme);
                                        tenant.SetWebTheme(parsedTheme, siteContext.Url);
                                        tenant.Context.ExecuteQueryRetry();
                                    }
                                    siteUrls.Add(t.Id, siteContext.Url);
                                    if (!string.IsNullOrEmpty(t.ProvisioningId))
                                    {
                                        _additionalTokens.Add(new SequenceSiteUrlUrlToken(null, t.ProvisioningId, siteContext.Url));
                                    }
                                    break;
                                }
                            case CommunicationSiteCollection c:
                                {
                                    var siteUrl = tokenParser.ParseString(c.Url);
                                    if (!siteUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        var rootSiteUrl = tenant.GetRootSiteUrl();
                                        tenant.Context.ExecuteQueryRetry();
                                        siteUrl = UrlUtility.Combine(rootSiteUrl.Value, siteUrl);
                                    }
                                    CommunicationSiteCollectionCreationInformation siteInfo = new CommunicationSiteCollectionCreationInformation()
                                    {
                                        ShareByEmailEnabled = c.AllowFileSharingForGuestUsers,
                                        Classification = tokenParser.ParseString(c.Classification),
                                        Description = tokenParser.ParseString(c.Description),
                                        Lcid = (uint)c.Language,
                                        Owner = tokenParser.ParseString(c.Owner),
                                        Title = tokenParser.ParseString(c.Title),
                                        Url = siteUrl
                                    };
                                    if (Guid.TryParse(c.SiteDesign, out Guid siteDesignId))
                                    {
                                        siteInfo.SiteDesignId = siteDesignId;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(c.SiteDesign))
                                        {
                                            siteInfo.SiteDesign = (CommunicationSiteDesign)Enum.Parse(typeof(CommunicationSiteDesign), c.SiteDesign);
                                        }
                                        else
                                        {
                                            siteInfo.SiteDesign = CommunicationSiteDesign.Showcase;
                                        }
                                    }
                                    // check if site exists
                                    if (tenant.SiteExists(siteInfo.Url))
                                    {
                                        WriteMessage($"Using existing Communications Site at {siteInfo.Url}", ProvisioningMessageType.Progress);
                                        siteContext = (tenant.Context as ClientContext).Clone(siteInfo.Url, applyingInformation.AccessTokens);
                                    }
                                    else
                                    {
                                        WriteMessage($"Creating Communications Site at {siteInfo.Url}", ProvisioningMessageType.Progress);
                                        siteContext = Sites.SiteCollection.Create(tenant.Context as ClientContext, siteInfo);
                                    }
                                    if (c.IsHubSite)
                                    {
                                        RegisterAsHubSite(tenant, siteInfo.Url, c.HubSiteLogoUrl);
                                    }
                                    if (!string.IsNullOrEmpty(c.Theme))
                                    {
                                        var parsedTheme = tokenParser.ParseString(c.Theme);
                                        tenant.SetWebTheme(parsedTheme, siteInfo.Url);
                                        tenant.Context.ExecuteQueryRetry();
                                    }
                                    siteUrls.Add(c.Id, siteInfo.Url);
                                    if (!string.IsNullOrEmpty(c.ProvisioningId))
                                    {
                                        _additionalTokens.Add(new SequenceSiteUrlUrlToken(null, c.ProvisioningId, siteInfo.Url));
                                    }
                                    break;
                                }
                            case TeamNoGroupSiteCollection t:
                                {
                                    SiteEntity siteInfo = new SiteEntity()
                                    {
                                        Lcid = (uint)t.Language,
                                        Template = "STS#3",
                                        TimeZoneId = t.TimeZoneId,
                                        Title = tokenParser.ParseString(t.Title),
                                        Url = tokenParser.ParseString(t.Url),
                                        SiteOwnerLogin = tokenParser.ParseString(t.Owner),
                                    };
                                    WriteMessage($"Creating Team Site with no Office 365 group at {siteInfo.Url}", ProvisioningMessageType.Progress);
                                    if (tenant.SiteExists(t.Url))
                                    {
                                        WriteMessage($"Using existing Team Site at {siteInfo.Url}", ProvisioningMessageType.Progress);
                                        siteContext = (tenant.Context as ClientContext).Clone(t.Url, applyingInformation.AccessTokens);
                                    }
                                    else
                                    {
                                        tenant.CreateSiteCollection(siteInfo, false, true);
                                        siteContext = tenant.Context.Clone(t.Url, applyingInformation.AccessTokens);
                                    }
                                    if (t.IsHubSite)
                                    {
                                        RegisterAsHubSite(tenant, siteContext.Url, t.HubSiteLogoUrl);
                                    }
                                    if (!string.IsNullOrEmpty(t.Theme))
                                    {
                                        var parsedTheme = tokenParser.ParseString(t.Theme);
                                        tenant.SetWebTheme(parsedTheme, siteContext.Url);
                                        tenant.Context.ExecuteQueryRetry();
                                    }
                                    siteUrls.Add(t.Id, siteContext.Url);
                                    if (!string.IsNullOrEmpty(t.ProvisioningId))
                                    {
                                        _additionalTokens.Add(new SequenceSiteUrlUrlToken(null, t.ProvisioningId, siteContext.Url));
                                    }
                                    break;
                                }
                        }

                        var web = siteContext.Web;

                        foreach (var subsite in sitecollection.Sites)
                        {
                            var subSiteObject = (TeamNoGroupSubSite)subsite;
                            web.EnsureProperties(w => w.Webs.IncludeWithDefaultProperties(), w => w.ServerRelativeUrl);
                            siteTokenParser = CreateSubSites(hierarchy, siteTokenParser, sitecollection, siteContext, web, subSiteObject);
                        }
                    }

                    // System.Threading.Thread.Sleep(TimeSpan.FromMinutes(10));

                    WriteMessage("Applying templates", ProvisioningMessageType.Progress);

                    var provisioningTemplateApplyingInformation = new ProvisioningTemplateApplyingInformation();
                    provisioningTemplateApplyingInformation.AccessTokens = applyingInformation.AccessTokens;
                    provisioningTemplateApplyingInformation.MessagesDelegate = applyingInformation.MessagesDelegate;
                    provisioningTemplateApplyingInformation.ProgressDelegate = applyingInformation.ProgressDelegate;
                    
                    foreach (var sitecollection in sequence.SiteCollections)
                    {
                        siteUrls.TryGetValue(sitecollection.Id, out string siteUrl);
                        if (siteUrl != null)
                        {
                            using (var clonedContext = tenant.Context.Clone(siteUrl, applyingInformation.AccessTokens))
                            {
                                var web = clonedContext.Web;
                                foreach (var templateRef in sitecollection.Templates)
                                {
                                    var provisioningTemplate = hierarchy.Templates.FirstOrDefault(t => t.Id == templateRef);
                                    if (provisioningTemplate != null)
                                    {
                                        provisioningTemplate.Connector = hierarchy.Connector;
                                        if (siteTokenParser == null)
                                        {
                                            siteTokenParser = new TokenParser(web, provisioningTemplate, applyingInformation);
                                            foreach(var token in _additionalTokens)
                                            {
                                                siteTokenParser.AddToken(token);
                                            }
                                        }
                                        else
                                        {
                                            siteTokenParser.Rebase(web, provisioningTemplate);
                                        }
                                        WriteMessage($"Applying Template", ProvisioningMessageType.Progress);
                                        new SiteToTemplateConversion().ApplyRemoteTemplate(web, provisioningTemplate, provisioningTemplateApplyingInformation, true, siteTokenParser);
                                    }
                                    else
                                    {
                                        WriteMessage($"Referenced template ID {templateRef} not found", ProvisioningMessageType.Error);
                                    }

                                }

                                if (siteTokenParser == null)
                                {
                                    siteTokenParser = new TokenParser(tenant, hierarchy, applyingInformation);
                                    foreach(var token in _additionalTokens)
                                    {
                                        siteTokenParser.AddToken(token);
                                    }
                                }

                                foreach (var subsite in sitecollection.Sites)
                                {
                                    var subSiteObject = (TeamNoGroupSubSite)subsite;
                                    web.EnsureProperties(w => w.Webs.IncludeWithDefaultProperties(), w => w.ServerRelativeUrl);
                                    siteTokenParser = ApplySubSiteTemplates(hierarchy, siteTokenParser, sitecollection, clonedContext, web, subSiteObject, provisioningTemplateApplyingInformation);
                                }
                            }
                        }
                    }
                }
                return tokenParser;
            }
        }

        private static void RegisterAsHubSite(Tenant tenant, string siteUrl, string logoUrl)
        {
            var hubSiteProperties = tenant.GetHubSitePropertiesByUrl(siteUrl);
            tenant.Context.Load<HubSiteProperties>(hubSiteProperties);
            tenant.Context.ExecuteQueryRetry();
            if (hubSiteProperties.ServerObjectIsNull == true)
            {
                hubSiteProperties = tenant.RegisterHubSite(siteUrl);
                tenant.Context.Load(hubSiteProperties);
                tenant.Context.ExecuteQueryRetry();
            }
            if(!string.IsNullOrEmpty(logoUrl))
            {
                hubSiteProperties.LogoUrl = logoUrl;
                hubSiteProperties.Update();
                tenant.Context.ExecuteQueryRetry();
            }
        }

        private TokenParser CreateSubSites(ProvisioningHierarchy hierarchy, TokenParser tokenParser, Model.SiteCollection sitecollection, ClientContext siteContext, Web web, TeamNoGroupSubSite subSiteObject)
        {
            var url = tokenParser.ParseString(subSiteObject.Url);

            var subweb = web.Webs.FirstOrDefault(t => t.ServerRelativeUrl.Equals(UrlUtility.Combine(web.ServerRelativeUrl, "/", url.Trim(new char[] { '/' }))));
            if (subweb == null)
            {
                subweb = web.Webs.Add(new WebCreationInformation()
                {
                    Language = subSiteObject.Language,
                    Url = url,
                    Description = tokenParser.ParseString(subSiteObject.Description),
                    Title = tokenParser.ParseString(subSiteObject.Title),
                    UseSamePermissionsAsParentSite = subSiteObject.UseSamePermissionsAsParentSite,
                    WebTemplate = "STS#3"
                });
                WriteMessage($"Creating Sub Site with no Office 365 group at {url}", ProvisioningMessageType.Progress);
                siteContext.Load(subweb);
                siteContext.ExecuteQueryRetry();
            }
            else
            {
                WriteMessage($"Using existing Sub Site with no Office 365 group at {url}", ProvisioningMessageType.Progress);
            }

            if (subSiteObject.Sites.Any())
            {
                foreach (var subsubSite in subSiteObject.Sites)
                {
                    var subsubSiteObject = (TeamNoGroupSubSite)subsubSite;
                    tokenParser = CreateSubSites(hierarchy, tokenParser, sitecollection, siteContext, subweb, subsubSiteObject);
                }
            }

            return tokenParser;
        }

        private TokenParser ApplySubSiteTemplates(ProvisioningHierarchy hierarchy, TokenParser tokenParser, Model.SiteCollection sitecollection, ClientContext siteContext, Web web, TeamNoGroupSubSite subSiteObject, ProvisioningTemplateApplyingInformation provisioningTemplateApplyingInformation)
        {
            var url = tokenParser.ParseString(subSiteObject.Url);

            var subweb = web.Webs.FirstOrDefault(t => t.ServerRelativeUrl.Equals(UrlUtility.Combine(web.ServerRelativeUrl, "/", url.Trim(new char[] { '/' }))));

            foreach (var templateRef in sitecollection.Templates)
            {
                var provisioningTemplate = hierarchy.Templates.FirstOrDefault(t => t.Id == templateRef);
                if (provisioningTemplate != null)
                {
                    provisioningTemplate.Connector = hierarchy.Connector;
                    if (tokenParser == null)
                    {
                        tokenParser = new TokenParser(subweb, provisioningTemplate);
                    }
                    else
                    {
                        tokenParser.Rebase(subweb, provisioningTemplate);
                    }
                    new SiteToTemplateConversion().ApplyRemoteTemplate(subweb, provisioningTemplate, provisioningTemplateApplyingInformation, true, tokenParser);
                }
                else
                {
                    WriteMessage($"Referenced template ID {templateRef} not found", ProvisioningMessageType.Error);
                }
            }

            if (subSiteObject.Sites.Any())
            {
                foreach (var subsubSite in subSiteObject.Sites)
                {
                    var subsubSiteObject = (TeamNoGroupSubSite)subsubSite;
                    tokenParser = ApplySubSiteTemplates(hierarchy, tokenParser, sitecollection, siteContext, subweb, subsubSiteObject, provisioningTemplateApplyingInformation);
                }
            }

            return tokenParser;
        }


        public override bool WillExtract(Tenant tenant, Model.ProvisioningHierarchy hierarchy, string sequenceId, ProvisioningTemplateCreationInformation creationInfo)
        {
            throw new NotImplementedException();
        }

        public override bool WillProvision(Tenant tenant, Model.ProvisioningHierarchy hierarchy, string sequenceId, ProvisioningTemplateApplyingInformation applyingInformation)
        {
            return hierarchy.Sequences.Count > 0;
        }
    }
}
#endif