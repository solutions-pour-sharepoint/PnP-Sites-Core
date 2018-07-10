using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace OfficeDevPnP.Core.IdentityModel.TokenProviders.ADFS
{
    /// <summary>
    /// Base class for active SAML based authentication
    /// </summary>
    public class BaseProvider
    {
        /// <summary>
        /// Transforms the retrieved SAML token into a FedAuth cookie value by calling into the SharePoint STS
        /// </summary>
        /// <param name="samlToken">SAML token obtained via active authentication to ADFS</param>
        /// <param name="samlSite">Url of the SAML secured SharePoint site</param>
        /// <param name="relyingPartyIdentifier">Identifier of the ADFS relying party that we're hitting</param>
        /// <returns>The FedAuth cookie value</returns>
        internal string TransformSamlTokenToFedAuth(string samlToken, string samlSite, string relyingPartyIdentifier)
        {
            samlToken = WrapInSoapMessage(samlToken, relyingPartyIdentifier);

            var samlServer = samlSite.EndsWith("/") ? samlSite : samlSite + "/";
            var samlServerRoot = new Uri(samlServer);

            var sharepointSite = new
            {
                Wctx = samlServer + "_layouts/Authenticate.aspx?Source=%2F",
                Wtrealm = samlServer,
                Wreply = $"{samlServerRoot.Scheme}://{samlServerRoot.Host}/_trust/"
            };

            var stringData = $"wa=wsignin1.0&wctx={HttpUtility.UrlEncode(sharepointSite.Wctx)}&wresult={HttpUtility.UrlEncode(samlToken)}";

            var sharepointRequest = WebRequest.Create(sharepointSite.Wreply) as HttpWebRequest;
            sharepointRequest.Method = "POST";
            sharepointRequest.ContentType = "application/x-www-form-urlencoded";
            sharepointRequest.CookieContainer = new CookieContainer();
            sharepointRequest.AllowAutoRedirect = false; // This is important

            var newStream = sharepointRequest.GetRequestStream();
            var data = Encoding.UTF8.GetBytes(stringData);
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            string fedAuthCookieValue;
            using (HttpWebResponse webResponse = (HttpWebResponse)sharepointRequest.GetResponse())
            {
                fedAuthCookieValue = webResponse.Cookies["FedAuth"].Value;
            }

            return fedAuthCookieValue;
        }

        /// <summary>
        /// Wrap SAML token in RequestSecurityTokenResponse soap message
        /// </summary>
        /// <param name="stsResponse">SAML token obtained via active authentication to ADFS</param>
        /// <param name="relyingPartyIdentifier">Identifier of the ADFS relying party that we're hitting</param>
        /// <returns>RequestSecurityTokenResponse soap message</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Xml.XmlDocument.CreateTextNode(System.String)")]
        private string WrapInSoapMessage(string stsResponse, string relyingPartyIdentifier)
        {
            var samlAssertion = new XmlDocument();
            samlAssertion.PreserveWhitespace = true;
            samlAssertion.LoadXml(stsResponse);

            //Select the book node with the matching attribute value.
            var notBefore = samlAssertion.DocumentElement.FirstChild.Attributes["NotBefore"].Value;
            var notOnOrAfter = samlAssertion.DocumentElement.FirstChild.Attributes["NotOnOrAfter"].Value;

            var soapMessage = new XmlDocument();
            var soapEnvelope = soapMessage.CreateElement("t", "RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust");
            soapMessage.AppendChild(soapEnvelope);
            var lifeTime = soapMessage.CreateElement("t", "Lifetime", soapMessage.DocumentElement.NamespaceURI);
            soapEnvelope.AppendChild(lifeTime);
            var created = soapMessage.CreateElement("wsu", "Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            var createdValue = soapMessage.CreateTextNode(notBefore);
            created.AppendChild(createdValue);
            lifeTime.AppendChild(created);
            var expires = soapMessage.CreateElement("wsu", "Expires", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            var expiresValue = soapMessage.CreateTextNode(notOnOrAfter);
            expires.AppendChild(expiresValue);
            lifeTime.AppendChild(expires);
            var appliesTo = soapMessage.CreateElement("wsp", "AppliesTo", "http://schemas.xmlsoap.org/ws/2004/09/policy");
            soapEnvelope.AppendChild(appliesTo);
            var endPointReference = soapMessage.CreateElement("wsa", "EndpointReference", "http://www.w3.org/2005/08/addressing");
            appliesTo.AppendChild(endPointReference);
            var address = soapMessage.CreateElement("wsa", "Address", endPointReference.NamespaceURI);
            var addressValue = soapMessage.CreateTextNode(relyingPartyIdentifier);
            address.AppendChild(addressValue);
            endPointReference.AppendChild(address);
            var requestedSecurityToken = soapMessage.CreateElement("t", "RequestedSecurityToken", soapMessage.DocumentElement.NamespaceURI);
            var samlToken = soapMessage.ImportNode(samlAssertion.DocumentElement, true);
            requestedSecurityToken.AppendChild(samlToken);
            soapEnvelope.AppendChild(requestedSecurityToken);
            var tokenType = soapMessage.CreateElement("t", "TokenType", soapMessage.DocumentElement.NamespaceURI);
            var tokenTypeValue = soapMessage.CreateTextNode("urn:oasis:names:tc:SAML:1.0:assertion");
            tokenType.AppendChild(tokenTypeValue);
            soapEnvelope.AppendChild(tokenType);
            var requestType = soapMessage.CreateElement("t", "RequestType", soapMessage.DocumentElement.NamespaceURI);
            var requestTypeValue = soapMessage.CreateTextNode("http://schemas.xmlsoap.org/ws/2005/02/trust/Issue");
            requestType.AppendChild(requestTypeValue);
            soapEnvelope.AppendChild(requestType);
            var keyType = soapMessage.CreateElement("t", "KeyType", soapMessage.DocumentElement.NamespaceURI);
            var keyTypeValue = soapMessage.CreateTextNode("http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey");
            keyType.AppendChild(keyTypeValue);
            soapEnvelope.AppendChild(keyType);

            return soapMessage.OuterXml;
        }

        /// <summary>
        /// Returns the DateTime when then received saml token will expire
        /// </summary>
        /// <param name="stsResponse">saml token</param>
        /// <returns>DateTime holding the expiration date. Defaults to DateTime.MinValue if there's no valid datetime in the saml token</returns>
        internal DateTime SamlTokenExpiresOn(string stsResponse)
        {
            var samlAssertion = new XmlDocument();
            samlAssertion.PreserveWhitespace = true;
            samlAssertion.LoadXml(stsResponse);

            var notOnOrAfter = samlAssertion.DocumentElement.FirstChild.Attributes["NotOnOrAfter"].Value;
            var toDate = DateTime.MinValue;
            if (DateTime.TryParse(notOnOrAfter, out toDate))
            {
                return toDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Returns the SAML token life time
        /// </summary>
        /// <param name="stsResponse">saml token</param>
        /// <returns>TimeSpan holding the token lifetime. Defaults to TimeSpan.Zero is case of problems</returns>
        internal TimeSpan SamlTokenlifeTime(string stsResponse)
        {
            var samlAssertion = new XmlDocument();
            samlAssertion.PreserveWhitespace = true;
            samlAssertion.LoadXml(stsResponse);

            var notOnOrAfter = samlAssertion.DocumentElement.FirstChild.Attributes["NotOnOrAfter"].Value;
            var notBefore = samlAssertion.DocumentElement.FirstChild.Attributes["NotBefore"].Value;

            var toDate = DateTime.MinValue;
            if (DateTime.TryParse(notOnOrAfter, out toDate))
            {
                var fromDate = DateTime.MinValue;
                if (DateTime.TryParse(notBefore, out fromDate))
                {
                    return toDate - fromDate;
                }
            }

            return TimeSpan.Zero;
        }
    }
}
