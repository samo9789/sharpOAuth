using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Diagnostics;

namespace OAuth
{
    public class OAuthRequest : OauthBase
    {
        private OAuthConsumer _consumer;
        
        public OAuthRequest(OAuthConsumer consumer, string debugType) : base (debugType)
        {
            this._consumer = consumer;            
        }

        /// <summary>
        /// Generates an OAuth Nonce
        /// </summary>
        /// <returns>a string to be used as the unique nonce</returns>
        private string _generateNonce() 
        {
            Random random = new Random();
            return random.Next(123400, 9999999).ToString();
        }

        /// <summary>
        /// Generates the OAuth timestamp in number of seconds since January 1st 1970
        /// </summary>
        /// <returns>a string to be used as the query timestamp</returns>
        private string _generateTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// Normalizes the query parameters by sorting them according to the OAuth specification
        /// Generates the signature base
        /// </summary>
        /// <param name="url">the base url of the service called</param>
        /// <param name="parameters">the list of query parameters passed</param>
        /// <param name="consumerSecret">consumer secret</param>        
        /// <param name="normalizedUrl">the normalized Url (returned)</param>
        /// <param name="normalizedRequestParameters">the normalized parameters (returned)</param>
        /// <returns>a string to be used as the request signature base</returns>
        private string _generateSignatureBase(Uri url, List<QueryParameter> parameters, string consumerSecret, out string normalizedUrl, out string normalizedRequestParameters)
        {       
            normalizedUrl = null;
            normalizedRequestParameters = null;

            // Sort the parameters
            parameters.Sort(new QueryParameterComparer());

            normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }
            normalizedUrl += url.AbsolutePath;
            normalizedRequestParameters = base.NormalizeRequestParameters(parameters);

            string httpMethod = "GET";
            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{0}&", httpMethod.ToUpper());
            signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl));
            signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));
            
            return signatureBase.ToString();        
        }

        /// <summary>
        /// Generates the request signature using the hash
        /// </summary>
        /// <param name="signatureBase">string base url</param>
        /// <param name="hmacsha1">hash key</param>
        /// <returns>string to be used as the request signature</returns>
        private string _generateSignatureUsingHash(string signatureBase, HMACSHA1 hmacsha1)
        {            
            byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(signatureBase);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Generates the request signature for OAuth call
        /// </summary>
        /// <param name="url">the Url of the service</param>
        /// <param name="parameters">the List of Query Parameters passed to the service</param>
        /// <param name="signatureMethod">the signature method to be used</param>
        /// <param name="consumerSecret">the consumer secret</param>
        /// <param name="oauth_token">the oauth token</param>
        /// <param name="normalizedUrl">the normalized url of the request</param>
        /// <param name="normalizedUrlWithParameters">the normalized url of the request with the parameters</param>
        /// <returns></returns>
        private string _generateSignature(Uri url, List<QueryParameter> parameters, string signatureMethod, string consumerSecret, string oauth_token, out string normalizedUrl, out string normalizedUrlWithParameters)
        {
            string tokenSecret = oauth_token;
            switch (signatureMethod)
            {             
                case "HMAC-SHA1":
                    string signatureBase = this._generateSignatureBase(url, parameters, consumerSecret, out normalizedUrl, out normalizedUrlWithParameters);
                    HMACSHA1 hmacsha1 = new HMACSHA1();
                    hmacsha1.Key = Encoding.UTF8.GetBytes(string.Format("{0}&{1}", UrlEncode(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? "" : UrlEncode(tokenSecret)));                    
                    return this._generateSignatureUsingHash(signatureBase, hmacsha1);
                default:
                    throw new Exception("signature method not yet implemented");
            }   
        }

        /// <summary>
        /// Calls a webservice specified by its url and passing it the parameters        
        /// </summary>
        /// <param name="url">Url of the service to call</param>
        /// <param name="oauthToken">oauth token (if exists)</param>
        /// <param name="oauthTokenSecret">oauth token secret (if exists)</param>
        /// <param name="extraParameters">the list of parameters to send the service</param>
        /// <returns></returns>
        public string request(Uri url, string oauthToken, string oauthTokenSecret, List<QueryParameter> extraParameters)
        {
            string normalizedUrl = String.Empty;
            string normalizedUrlWithParameters = String.Empty;
            
            // Generate the parameters based on the Oauth Configuration of the Consumer
            List<QueryParameter> parameters = new List<QueryParameter>();                        
            parameters.Add(new QueryParameter("oauth_consumer_key", this._consumer.OauthConfig.ConsumerKey));
            parameters.Add(new QueryParameter("oauth_nonce", this._generateNonce()));
            parameters.Add(new QueryParameter("oauth_timestamp", this._generateTimestamp()));
            parameters.Add(new QueryParameter("oauth_signature_method", this._consumer.OauthConfig.OauthSignatureMethod));
            parameters.Add(new QueryParameter("oauth_version", this._consumer.OauthConfig.OauthVersion));
            
            // Add the OauthToken and OAuthTokenSecret if they are valued
            if (oauthToken != null && oauthToken != String.Empty && oauthToken.Length > 0)
            {
                parameters.Add(new QueryParameter("oauth_token", oauthToken));
            }
            if (oauthTokenSecret != null && oauthTokenSecret != String.Empty && oauthTokenSecret.Length > 0)
            {
                parameters.Add(new QueryParameter("oauth_token_secret", oauthTokenSecret));
            }
            if (extraParameters != null)
            {
                foreach (QueryParameter param in extraParameters)
                {                    
                    parameters.Add(param);                    
                }
            }

            // Generate the OAuth Signature and add it to the parameters
            string signature = this._generateSignature(url, parameters, this._consumer.OauthConfig.OauthSignatureMethod, this._consumer.OauthConfig.ConsumerSecret, oauthTokenSecret, out normalizedUrl, out normalizedUrlWithParameters);
            parameters.Add(new QueryParameter("oauth_signature", signature));                 
            
            try
            {                   
                string oauthUrl = normalizedUrl + '?' + normalizedUrlWithParameters + "&oauth_signature=" + signature;   
                System.Net.HttpWebRequest request = System.Net.HttpWebRequest.Create(oauthUrl) as System.Net.HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
                System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
                String content = reader.ReadToEnd();                
                return content;
            }
            catch (WebException e)
            {
                base._debug(e.Message);                
                return "";
            }        
        }
    }
}
