using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using OAuth;

namespace sample2_twitterWeb
{


	public partial class Default : System.Web.UI.Page
	{

		public virtual void loginTwitter (object sender, EventArgs args)
		{		
			// Change the button text
			buttonLogin.Text = "Complete authorization with Twitter";
			
			// Create an OAuth config
            OAuthConfig oauthConfig = new OAuthConfig("console");
            oauthConfig.SiteUrl = "http://www.worldgoneweb.com";
            oauthConfig.OauthVersion = "1.0";
            oauthConfig.OauthSignatureMethod = "HMAC-SHA1";
            oauthConfig.ConsumerKey = ConfigurationManager.AppSettings.Get("consumerKey");
            oauthConfig.ConsumerSecret = ConfigurationManager.AppSettings.Get("consumerSecret");
			oauthConfig.RequestTokenUrl = "https://api.twitter.com/oauth/request_token";
			oauthConfig.AccessTokenUrl = "https://api.twitter.com/oauth/access_token";
			oauthConfig.UserAuthorizationUrl = "https://api.twitter.com/oauth/authorize";
			
			// Create an OAuth consumer
            OAuthConsumer oauthConsumer = new OAuthConsumer(oauthConfig, "console");
			
			// Request Token
			oauthConsumer.getRequestToken();
			
			// After the authorization is completed with Twitter, OAuth will redirect you to the callbackURL declared in your application settings at dev.twitter.com
			// Information about the authenticated user is provided to that page in addition to the access token needed to execute further api calls
			
		}
	}
}

