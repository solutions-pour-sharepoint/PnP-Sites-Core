# OfficeDevPnP.Sites.Core Changelog

*Please do not commit changes to this file, it is maintained by the repo owner.*

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).

## [3.12.1908.0 - August 2019 release]

### Added


### Changed

- Fix: Added Built in CT Html Page layout #2321 [SchauDK]
- Fix: Handle CreateGroupEx with SiteStatus = 1 (provisioning status)
- Fix: Increase default timeout for ALM API calls

## [3.11.1907.0 - July 2019 release]

### Added

### Changed

- Fixed issue when importing a CSV file using ImportTerms where terms where always added to the first termset. [IonutLupsan]
- Fix: fileUniqueId parsing in ReplaceFileUniqueToken when point to Folder #2289 [czullu]
- Fixed version conflict in clientsidepage handler #2255, #2285
- Fix: Made owner mandatory for communication site in app-only context #2297 [gautamdsheth]
- Fix: Support for default header image for client side page based on visual layout #2301 [SchauDK]
- Fix: Ensure ID property is loaded for sitegroup provisioning #2303 [OliverZeiser]
- Fix: "The 'Equals' member cannot be used in the expression" in GetPrincipalUniqueRoleAssignments #2305 [patrikhellgren]
- Fix: Added BaseViewID into ViewCreationInformation #2304 [StaffanNelemans]
- CAML query class improvements #2308 [kirschem]
- Fix tokenization issue with views in root site collection #2295 [jackpoz]
- Fix: NoCrawl web property setting did not always work

## [3.10.1906.0 - June 2019 release ]

### Added

- Added ability to include all client side pages in an extracted template
- Feature: Adds capability to connect existing team site to MS Teams team (teamify) #2265 [gautamdsheth]

### Changed 

- Client side page serialization fix: if web part serverProcessedContent properties contains htmlStrings then these are now correctly serialized in the resulting html
- Update SiteLockState enum to support ReadOnly status #2275 [TomekPi]
- Fix DataRow parameters replacement. #2248 [siata13]
- The specified user {associatedownergroupid} could not be found. #2281 [StaffanNelemans]
- Fix DataRow URL field name: data value URL,Description. #2273 [cgenero]
- Provisioning webhook fixes

## [3.9.1905.3 - May 2019 Intermediate Release 3]

### Changed 

- Reintroduced fieldlink reordering on content types after server side fixes are in place.
- Allow for retrieval of principals via their ID in objectsitesecurity.cs
- Improved 'delta' detection for lists factoring out version changes in the list image url
- Fixed xml comments to reduce compiler warnings [gszdev]

## [3.9.1905.2 - May 2019 Intermediate Release 2]

### Changed 

- Fixed issue with nested tokens in token parser

## [3.9.1905.1 - May 2019 Intermediate Release]

### Changed

- Temporarily removed the functionality to reorder fieldlinks in content types created through the provisioning engine due to server side code issue.

## [3.9.1905.0 - May 2019 release]

### Added

- Support to export terms with a specific lcid in TaxonomyExtensions.ExportTermSet
- Support for new page header and section backgrounds in the modern client side page provisioning [NicolajHedeager]
- Support for provisioning client side page templates
- Support for Provisioning Schema 201903
- Support for provisioning Microsoft Teams
- Support for provisioning Site Header settings
- Support for provisioning Site Footer links
- New PnPProvisioningContext object for security scope management
- New tenant extension method (tenant.EnableCommSite) to to convert the root site collection of a tenant into a communication site
- Added support to extract and provision list propertybag entries #2201 [patrikhellgren]
- A lot of schema 201903 unit tests [s-KaiNet]

### Changed

- Added support to apply an OOTB theme using web.ApplyTheme() [gautamdsheth]
- Added support for setting owners and hubsite id in GroupifySite [gautamdsheth]
- Allow fileuniqueid in NavigationNodes for Group OneNote Url #2150 [czullu]
- fixing client side page existence check sometimes flags page as existing although it isn't #2185 [heinrich-ulbricht]
- Fix loading of ClientSidePage contents if CanvasContent1 property is empty #2199 [heinrich-ulbricht]
- Adding test for saving and loading of ClientSidePage header #2198 [heinrich-ulbricht]
- Fix: conflict when provisioning client side pages with headers #2208 [heinrich-ulbricht]
- Fix: fix for GlobalNavigation serialization in Provisioning Schema #2210 [patrikhellgren]
- Fix: Handling of re-used terms #2176 [heinrich-ulbricht]
- Fix: fixes to the serialization engine #2214 [s-KaiNet]
- Fix: Removing SP groups from role assignments in PnP templates
- Fix: Improved exporting and importing of associated groups #2192 / #2174 [heinrich-ulbricht]
- Fix: NullReferenceException caused by missing cookie #2232 [lafe]
- Fix: tokenization for calculated field formula where one field is present multiple times in the formula #2236 [NicolajHedeager]

## [3.8.1904.0 - April 2019 release]

### Added

- Export and import client side web parts with dynamic data connections #2120 [YannickRe]
- Added SyncToTeams method on the app ALM manager to synchronize an SPFX solution to the Microsoft Teams App Catalog
- Extension method IsCommunicationSite for Site objects

### Changed

- Fix: TokenParser to resolve tokens directly following each other {hosturl}{site} #2111 [czullu]
- Fix: ClientSidePage description provisioning #2112 [czullu]
- Fix: Failure with RatedBy and Ratings #2113 [czullu]
- Ensure Field InternalName is correctly retrieved when logging provisision progress #2140 [NicolajHedeager]
- Feature/Fix: Added ensureSiteAssetsLibrary method to prevent access denied #2129 [gautamdsheth]
- Fix : Skip creation of associated groups if they already exist #2128 [gautamdsheth]
- Fix: Added capability to set the private CDN orgins and policies #2141 [gautamdsheth]
- EnsureUser for external users #2136 [schaudk]
- Don't fail on modern page save when the provided header image url is living in a different web then the client side page
- Correctly save a modern repost page

## [3.7.1903.0 - March 2019 release]

### Added

- Client Side pages API support for SP2019 #2089 [lafe]
- ALM functions for SharePoint 2019 on premises #2074 [lafe]
- Authentication option based upon Azure AD credential flow in combination with the SPO Management Shell Azure AD application

### Changed

- Cloning of ClientContext objects created by AuthenticationManager using one of these methods (GetAppOnlyAuthenticatedContext, GetAzureADCredentialsContext, GetAzureADAppOnlyAuthenticatedContext) now works fine when cloning to different audience (e.g. clone from regular site to tenant admin)
- Create ClientSidePage with double quotes in Title generates wrong LayoutWebpartsContent #2058
- Skip executing EnsureProperty on Principal object when the Principal is null #2066 [schaudk]
- Fixed token handling for resource files having quotes
- Fixed parsing of SchemaXml #2067 [schaudk]
- Fixed typo [lafe]
- Fix #2088 - Provisioning template doesn't publish the app #2090 [gautamdsheth]

## [3.6.1902.0 - February 2019 release]

### Added

- Beta support for SP2019
- Provision and extract associated groups #2020 [jensotto]
- Fix provisioning navigation settings #1883 [phibsi]
- Add support for Kerberos authentication against ADFS #2050 [tmeckel]
- AssociatedGroupId token added + processing of it
- SequenceSiteCollectionId, SequenceSiteGroupId and SequenceSiteId tokens added
- Added support for creating and loading modern pages from sub folders inside the sitepages library
- ZoneID web part property now can be used in SP2016
- MajorVersionLimit and MajorWithMinorVersionsLimit are supported in the minimal (May 2018) version of SP2013 CSOM (Issue 1943) #1994 [tmeckel]
- Enables Web.RequestAccessEmail for on-premises (both 15.0 and 16.0) #1794 [biste5]
- Add token parsing in `targetFileName` property of file object #2036 [stevebeauge]
- Added support to delete search configurations
- Add support for setting default sharing and sharing permissions on tenant extensions
- Added ThemeManager class with support for ApplyTheme extension method on Web objects
- Added delegate for callback on site fully provisioned within the Provisioning Engine
- Added STS#3 base template for SharePoint Online template extraction
- Added support to specify the hubsite id when creating modern sites and to set owners when creating a modern team site/O365 group associated site. [gautamdsheth]
- Added support for hubsiteid when creating modern sites with New-PnPSite
- Added support to set owners when creating a modern team site with New-PnPSite

### Changed

- Feature/make datarow and file properties consistent #1762 [stevebeauge]

## [3.5.1901.0 - January 2019 release]

### Added

- Added support for modern page section backgrounds
- Added new 1st party client side web parts to the client side page API - support for provisioning engine will come with next schema update
- Added support for webparts configured with isDomainIsolated=true - support for provisioning engine will come with next schema update
- ResetFileToPreviousVersion extension method #2030 [skaggej]
  
### Changed

- Fix to make the EveryoneExceptExternalUsers token resolve correctly in all circumstances
- Fix to ensure TLS settings are correctly configured on certain OS versions (e.g. Windows Server 2012 R2)
- Fix throttling Retry-After processing, should be in seconds, not in milliseconds
- Multi-lingual provisioning of list title, extraction of additional navigation node languages #1974 [czullu]
- Updated logging logic #2018 [jensotto]
- Performance optimization on for the client side page save action

### Deprecated

## [3.4.1812.1 - December 2018 release]

### Added

- Added support for handling new page header options

### Changed

### Deprecated

- Deprecated Responsive UI extension methods
  
## [3.4.1812.0 - December 2018 release]

### Added

- Adding support for a 3rd navigation level in provisioning (for modern pages) #1927 [mbruckner]
- Ability to update content type properties #1776 [gautamdsheth]
- Ability to create team with Group #1990 [gautamdsheth]
- Ability to enable/disable comments, likes and view count on modern site pages #1756 [gautamdsheth]
- Added support for themes generation via ThemeUtility.GetThemeAsJSON(primaryColor, bodyTextColor, bodyBackgroundColor) [paolopia]

### Changed

- Stability improvements for updates to RoleDefinition update #1846 [sebastianmattar]
- Prevent access denied exception when provisioning content types #1903 [jensotto]
- Allow parameters in field defaults #1979 [oozoo-solutions]
- Add token parsing when provisioning search settings #1727 [jensotto]
- Fixed issue with calculated fields for non-English site collections #1970 [SchauDK]
- FixLookupField. If target list is not found, just return fieldXml #1977 [SchauDK]
- Current user can't be removed from new SecurableObject role assignments #1584 [jensotto]
- Use Xml token parsing for Xml data #1982 [SchauDK]
- New CSOM throttling implementation
- Fix: Token parser #1968 #1972 [SchauDK] [phawrylak]
- Improve add owner/member on Group creating #1987 #1990 #1991 [sadomovalex] [gautamdsheth]
- Improved handling of CustomSortOrder for terms in Term Store [TeodoraI]
- Improved Tenant and ALM handlers to avoid useless processing [gautamdsheth]

## [3.3.1811.0 - November 2018 release]

### Added

- Added support for the `Visibility` attribute for Unified Groups [devinprejean]
- Added support for language/lcid when creating modern sites using Sites.SiteCollection.CreateAsync method.
- Added support for FieldIdToken to support customers while migrating across sites and keeping field internal name, but changing field Id.
- Added support for Single Page WebPart App pages, will be part of SPFX 1.7
- Added support for Resource Path API in modern pages #1936 [gautamdsheth]

### Changed

- Get classification directly from Unified Group instead of a separate call [devinprejean]
- Removes 60 minute maximum lifetime for Access Tokens in AuthenticationManager #1957 [koskila]
- Fix: MaxVersionLimit set to 0 issue [gautamdsheth]

### Deprecated

## [3.2.1810.0 - October 2018 release]

### Added
- Added support for provisioning a site hierarchy through the provisioning engine based upon the 2018-07 schema.
- Added Tenant.ApplyProvisioningHierarchy extension method
- Added various additional provisioning engine object handlers to support sitehierarchy
- Added ability to set SiteLogo on a modern team site through Sites.SiteCollection.SetGroupImage method.

### Changed

- ClientSide page name now can contain a token [gautamdsheth]
- Fix issue with AssociatedGroupToken loading [gautamdsheth]
- LoginNames are compared case insensitive [tmeckel]
- Allow to create a CustomAction to a ListInstance without specifying a valid XML for the CommandUIExtension [tmeckel]
- Don't create a custom sort order for the HashTags TermSet [tmeckel]
- Use topological sort to order groups before creating them [tmeckel]
- Don't process web hook assignments without having a valid URL [phawrylak]
- Refactored objectterms and objectenant handler to support provisioning hierarchies.
- Don't export the internal _DisplayName field [phawrylak]
- Fixed SetOpenBySitePolicy as it never worked [gautamdsheth]
- Fixed ServerUnauthorizedAccessException when creating web (#1925) [phawrylak]

### Deprecated
- Deprecated all provisioning engine tokens that start with ~, like ~site, etc. Use {site} etc. instead. ~ tokens conflicted with a token system used by SharePoint itself.

## [3.1.1809.0 - September 2018 release]

### Added
- Added support to provision hidden views
- Added support for inviting guest users (AAD B2B) via Microsoft Graph [Vipul Kelkar]

### Changed
- Fixed issue where hidden views created by XsltListView web part where removed on a list during provisioning
- Refactored token parsing for PnP template handling for performance
- Support token replacement for view xml [vonis22]
- Updated CSOM Assemblies to 8029.1200
- Bugfix for token replacement where two tokens where next to each other like {hosturl}{siteid}
- Bugfix and optimizatin for web part listid token replacement
- Make preview link for banner image on modern pages link to the root site to avoid too long url's - and act like the default behaviour
- Fix for updating Unified Groups [Gautam Sheth]
- Extensibility handlers error handling [Jens Otto Hatlevold]
- Fix default client side page header title alignment

### Deprecated
- Marked regex functions in TokenDefinition as obsolete, as they are not needed

## [3.0.1808.0 - August 2018 release]

### Added

### Changed
- Introduced support for ADAL 3.x and JWT 5.x, updated NuGet package reference accordingly
- Client side API - Correctly handle data version: split between canvas and webpart data version + export data vesion using the provisioning engine + improved data version detection
- Bug fix for using SetDefaultColumnValues in lists in subsites [cnesmark]
- Fixed an issue with lookup fields in a list instance, when a template is applied to update a lookup field [antim-mironov]

### Deprecated

## [2.28.1807.0 - July 2018 release]

### Added
- Information management async extension methods #1843 [baywet]
- TimerJob AppOnly authentication in High Trust context #1808 [ypcode]

### Changed
- Added PowerApps client side web part type
- Fix NullReferenceException when parsing client side page header html #1821 [SchauDK]
- Changed multi lookup field provisioning to also handle list url in List #1822 [cebud]
- Don't wrap client side text in P if it already was done as part of the provided text
- Added tokenization of client side page header image url
- Fix #1810 ContentTypeBinding with lowercase ContentTypeID [TeodoraI]
- Fix list attribute for lookup fields #1826 [sebastianmattar]

### Deprecated

## [2.27.1806.0 - June 2018 release]

### Added
- Added optional timeout value on AppManager.Add method
- Support version 1.4 of page header data structure
- Feature/file folder async extension methods [baywet]

### Changed
- ClientComponentId and ClientComponentProperties are now updated when applying a template to a site where the customaction already exists [SchauDK]
- Fixes issue with requiring tenant admin access while not provisioning tenant scoped artifacts
- Fixed issue where a list would not be created based on a list template (TemplateFeatureId)
- Fixes issue with double tokens in content by search webpart provisioning [KEMiCZA]
- Fixes issue with sitedesigns not correctly being associated to web template
- Fixes issue where you could not specify content type in a datarow element in a provisioning template
- Fixes issue where you tried to modify a property of a default modern home page, and all web parts disapeared
- Fixed issue with Security Group names including HTML links [jensotto]
- Fixed issue with UseShared property for Navigation Settings [TheJeffer]
- Fixed issue with not existing links in Navigation Settings [gautamdsheth]
- Updated Microsoft Graph SDK package to version 1.9.0
- Correctly extract modern page title [SchauDK]
- Fixes issue with using culture in page header persisting [guillaume-kizilian]
- Fixes lookup column support by supporting list web relative urls [stevebeauge]
- Fixed ClientSidePageHeaderType enum inconsistency [SchauDK]
- Fixing #1770 issue. Now we are considering Publishing Images field type [luismanez]
- #1804 Incorrect exception thrown while setting multi-valued tax field [gautamdsheth]
- Typo fixes [stwel]

### Deprecated

## [2.26.1805.0 - May 2018 release]

### Added
- Added WebApiPermissions support to provisioning engine.
- Added support to auto populate the BannerImageUrl and Description fields during save of a client side page based on the found web parts and text parts on the page
- Added support for client side page header configuration (no header, header with image, default header)
- Added ClientSidePage Title support in the provisioning engine.
- Added CommentsOnSitePagesDisabled property on web settings element in the provisioning engine.
- Added support for StorageEntities to the Tenant element in the Provisioning Engine. The user applying the template needs appropriate access rights to the tenant scoped App Catalog.
- Added SiteScripts and SiteDesigns elements to the Tenant element in the Provisioning Engine. The user applying the template needs to be tenant administrator.
- Added HubSiteUrl to the WebSettings element for the Provisioning Engine. The user applying the template needs to be tenant administrator.
- Added {SiteScriptId:[script title]} and {SiteDesignId:[design title]} tokens to the provisioning engine. This will only work if the user applying the template is tenant administrator.
- Added {StorageEntityValue:[key]} token to retrieve values from tenant level or (when applicable) site collection level. If a key is present at site collection level this value will take preference over the one from tenant level, following the behavior of the CSOM APIs.
- Added support for loading the classification of a unified group.
- Added GetPrincipalUniqueRoleAssignments web extension method. Get all unique role assignments for a user or a group in a web object and all its descendents down to document or list item level.
- Added support for SystemUpdate of taxonomy fields on list extension and item extension methods.
- Added support for using the ClientWebPart client side web part to host "classic" SharePoint Add-ins on client side pages
- Added support for new schema v.2018-05
- Added support for Web API Permission in schema v.2018-05
- Added support for new schema v.2018-05 ==> 2018-05 is the new default schema
- Added async extension methods for feature handling and property retrieval [baywet]
- Added extension methods to better support property handling on lists [gautamdsheth]
- Added support for the implementation of the provisioning of dependent lookups fields [stevebeauge]

### Changed
- Fixed typo in TimeZone enum, and obsoleted incorrect value [gautamdsheth]
- Web hook server notification url in the provisioning engine now supports tokens [krzysztofziemacki]
- Fixed the setting of the page layout [TheJeffer] 
- Improved detection and configuration of the specific client side web part data version
- Allow webhooks expiration to be updated without specifying the original web hook notification url [tavikukko]
- Fixed detecting of "The object specified does not belong to a list" error in the SetFileProperties extension method [Ralmenar]
- Using ResourcePath.FromDecodedUrl to handle reading files and folders with special characters [gautamdsheth]
- Fix async handling calling ClientSidePage.AvailableClientSideComponents [OliverZeiser]

### Deprecated

## [2.25.1804.0 - April 2018 release]

### Added

- Added async external sharing extension methods [baywet]
- Added ProvisionFieldsToSubWebs option to ProvisioningTemplateApplyingInformation class [jensotto]
- Addition of PnPCore.Tests project for testing of the PnPCore .Net Standard 2.0 library
- Added Scope parameter to ALM Manager methods allowing you to perform application lifecycle management tasks to the site collection scoped app catalog.

### Changed

- Added support for CDN Elements in Provisioning Engine
- Support for FullBleed configuration for adding web parts in "Full Width column" section [OliverZeiser]
- Improvements to ExecuteQueryRetryAsync [OliverZeiser, biste5]
- Improvements to support provisioning engine to be called from non console applications
- Better support for async methods, avoiding deadlocks
- Updated spelling across various files [fowl2]
- Refactored ObjectListHandler [stevebeauge]

### Deprecated

## [2.24.1803.0 - March 2018 release]

### Added

- Added ExecuteQueryRetryAsync method [baywet and SharePointRadi]
- Added EnsureLabel extension method to the taxonomy extensions [paulpascha]
- Added SetDefaultContentType extension methods on List objects. Notice that this method behaves different from the deprecated SetDefaultContentTypeToList method. See the Deprecated section.
- Added AliasExistsAsync extension method to verify if an Office 365 Group alias is available for use
- Added support for taxonomy fields in DataRows at the provisioning engine level. [jensotto]
- Added support for updating owners and members of an Office 365 Group.
- Added support for TermStore DefaultLanguage when retrieving or adding a term. [stevebeauge]
- Added support for getting apps by title [gautamdsheth]
- Added .NET 2.0 Standard project to allow cross-platform use of the PnP Sites Core library

### Changed

- Improved test reliability by scoping out tests that should not execute during app-only test runs
- Correctly set the lookup list for fields of type User [pschaeflein]
- Don't tokenize ~sitecollection in web parts XML [paulpascha]
- Updated base templates for March 2018 release
- Fix #1585 - Correctly handle Overwrite=false with the new pre-create of pages
- TimerJob framework reliability improvements (avoid breaking when clientcontext could not be obtained)
- Fix #1595 - Fixed provisioning issue when the AppCatalog is missing. [gautamdsheth]
- Updated DataRow handler in provisioning engine to not update readonly fields, and to allow for emptying fields by leaving the element value empty of a DataValue element.
- Support extraction of "empty" client side pages when using an extensibility provider that extracts more than the default home page
- Improved detection of illegal characters in folder and file names [aslanovsergey]
- Fix #1509 - Role inheritance can be broken when site security is specified with BreakRoleInheritance set to true without additional RoleAssignments specified [paulpascha]
- Commenting can be enabled/disabled on home page via the ClientSidePages object handler
- RoleDefinitions are now parsed in the SiteSecurity object handler
- WebHook provisioning errors will not stop the provisioning process
- Improved list content type handling [jensotto]
- Exclude ComposedLook handler processing for NoScript sites
- Improved detection of App-Only to support weblogin based use
- SiteName and SiteTitle token updates [jensotto]
- Fix #1059 - SharePoint 2013 on premise issues with ApplyProvisioningTemplate when publishing activated
- Switched to CSOM version 7414.1200
- Groupify method supports the "keep existing homepage" scenario
- Fixed behavior while adding/updating datarows with the Provisioning Engine [craig-blowfield]

### Deprecated

- Marked SetDefaultContentTypeToList extensions methods on List and Web objects as deprecated. This method has some flaws. It was possible to use the ID of a content type at site level to set as a default content type in the list, IF a content type in that list was inheriting from the parent content type. The new method requires you to specify the actual content type that is associated with the list. It will not work to specify a parent content type id.
