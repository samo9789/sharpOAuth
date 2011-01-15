using System;
using OAuth;

namespace sample1_twitterClient
{
	
	class TwitterOAuthTest
	{
		
		// Mandatory
		// Edit the consumerKey and consumerSecret entries below
		private const string _consumerKey = "";
		private const string _consumerSecret = "";
		
		// Optional
		// If you with to run test2,
		// Edit the accessToken and accessTokenSecret entries below
		// Note: To obtain these infromation, log in to dev.twitter.com, edit your application detail and click on "my access" in the sidebar
		private const string _accessToken = "";
		private const string _accessTokenSecret = "";
		
		/// <summary>
		/// With this test, the application will request an oauth_access token for the current user. The user will have to authorize the app (this code sample) to access his twitter account.
		/// A combination of keys will be generated for him : an oauth access token.
		/// </summary>
		public void test_twitterOauthClient() 
		{
			// Create an OAuth config
            OAuthConfig oauthConfig = new OAuthConfig("console");
            oauthConfig.SiteUrl = "http://www.worldgoneweb.com";
            oauthConfig.OauthVersion = "1.0";
            oauthConfig.OauthSignatureMethod = "HMAC-SHA1";
            oauthConfig.ConsumerKey = TwitterOAuthTest._consumerKey;
            oauthConfig.ConsumerSecret = TwitterOAuthTest._consumerSecret;
			oauthConfig.RequestTokenUrl = "https://api.twitter.com/oauth/request_token";
			oauthConfig.AccessTokenUrl = "https://api.twitter.com/oauth/access_token";
			oauthConfig.UserAuthorizationUrl = "https://api.twitter.com/oauth/authorize";
			
			// Create an OAuth consumer
            OAuthConsumer oauthConsumer = new OAuthConsumer(oauthConfig, "console");
			
			// Request Token
			oauthConsumer.getRequestToken();
			
			// Enter the Pin Code
			Console.WriteLine("Enter the pin code:");
			string pincode = Console.ReadLine();
			
            // Request Access Token            
            oauthConsumer.getAccessToken(pincode);
			
			// Make an API Call (call the home_timeline status) and debug the response
			string response = (string)oauthConsumer.request("http://api.twitter.com/1/statuses/home_timeline.xml", "GET", null, "PLAIN");
			Console.WriteLine(response);
		}
	
	
		/// <summary>
		/// With this test, the oauth access token created for the requester of the API key will be used and no authorization process will take place
		/// </summary>
		public void test_twitterOauthSuperClient() {
			// Create an OAuth config
	        OAuthConfig oauthConfig = new OAuthConfig("console");
	        oauthConfig.SiteUrl = "http://www.worldgoneweb.com";
	        oauthConfig.OauthVersion = "1.0";
	        oauthConfig.OauthSignatureMethod = "HMAC-SHA1";
	        oauthConfig.ConsumerKey = TwitterOAuthTest._consumerKey;
	        oauthConfig.ConsumerSecret = TwitterOAuthTest._consumerSecret;
			oauthConfig.OauthToken = TwitterOAuthTest._accessToken;
			oauthConfig.OauthTokenSecret = TwitterOAuthTest._accessTokenSecret;
			
			// Create an OAuth consumer
	        OAuthConsumer oauthConsumer = new OAuthConsumer(oauthConfig, "console");
			
			// Make an API Call (call the home_timeline status) and debug the response
			string response = (string)oauthConsumer.request("http://api.twitter.com/1/statuses/home_timeline.xml", "GET", null, "PLAIN");
			Console.WriteLine(response);
		}
	}
	
	class MainClass
	{
		public static void Main (string[] args)
		{
			TwitterOAuthTest twitterOauthTest = new TwitterOAuthTest();
			//twitterOauthTest.test_twitterOauthClient();
			twitterOauthTest.test_twitterOauthSuperClient();
		}
	}
}

