using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth
{
    public class OAuthConfig : OauthBase
    {

        private string _siteUrl;     
        private string _consumerKey;
        private string _consumerSecret;
        private string _tokenKey;
        private string _tokenSecret;
        private string _oauthToken;       
        private string _oauthTokenSecret;
        private string _oauthTokenTtl;
        private string _userAuthorizationUrl;
        private string _accessTokenUrl;
        private string _oauthVersion;
        private string _oauthSignatureMethod;

        public string SiteUrl
        {
            get { return _siteUrl; }
            set { _siteUrl = value; }
        }

        public string ConsumerKey
        {
            get { return _consumerKey; }
            set { _consumerKey = value; }
        }        

        public string ConsumerSecret
        {
            get { return _consumerSecret; }
            set { _consumerSecret = value; }
        }

        public string TokenKey
        {
            get { return _tokenKey; }
            set { 
                _tokenKey = value;
                _oauthToken = TokenKey;
            }
        }

        public string TokenSecret
        {
            get { return _tokenSecret; }
            set { 
                _tokenSecret = value;
                _oauthTokenSecret = _tokenSecret;
            }
        }

        public string OauthToken
        {
            get { return _oauthToken; }
            set { _oauthToken = value; }
        }

        public string OauthTokenSecret
        {
            get { return _oauthTokenSecret; }
            set { _oauthTokenSecret = value; }
        }

        public string OauthTokenTtl
        {
            get { return _oauthTokenTtl; }
            set { _oauthTokenTtl = value; }
        }

        public string UserAuthorizationUrl
        {
            get { return _userAuthorizationUrl; }
            set { _userAuthorizationUrl = value; }
        }

        public string AccessTokenUrl
        {
            get { return _accessTokenUrl; }
            set { _accessTokenUrl = value; }
        }
        
        public string OauthVersion
        {
            get { return _oauthVersion; }
            set { _oauthVersion = value; }
        }

        public string OauthSignatureMethod
        {
            get { return _oauthSignatureMethod; }
            set { _oauthSignatureMethod = value; }
        }

        /**
         * Constructor
         */
        public OAuthConfig(string debugType) : base(debugType)
        {            
            // nothing to do
        }

    }
}
