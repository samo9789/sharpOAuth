<%@ Page Language="C#" Inherits="sample2_twitterWeb.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<title>Default</title>
</head>
<body>
	<h1>Twitter Web Client test</h1>
	<p>Click the button below to login with Twitter</p>
	<form id="form1" runat="server">
		<asp:Button id="buttonLogin" runat="server" Text="Login with Twitter" OnClick="loginTwitter" />
	</form>
</body>
</html>
