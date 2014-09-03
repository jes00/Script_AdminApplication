exec("./Application_Forms.GUI");
exec("./Application_Review.GUI");
exec("./Application_Denied.GUI");

//Thank you
// clientCmdMessageBoxOk(" Thanks!", "<font:impact:20><just:center>Thank you for your application. It has been sent to the host to be reviewed.", "");

// Pending
// clientCmdMessageBoxOk(" Pending...", "<font:impact:20><just:center>Your application is pending. You will be notified when the host has reviewed it.<br>Thank you for your patients!", "");

function clientCmdAdminApp_Accepted(%status)
{
	%text = "<font:Impact:20><just:center>Your admin application has been<br><color:008000>accepted<color:000000>!";

	if(%status == 1)
	{
		%text = "<font:Impact:20><just:center>Your admin application has been<br><color:008000>accepted<color:000000>!<br>You\'ve been give the position of<br><color:0000ff>Admin<color:000000>.";
	}

	else if(%status == 2)
	{
		%text = "<font:Impact:20><just:center>Your admin application has been<br><color:008000>accepted<color:000000>!<br>You\'ve been give the position of<br><color:0000ff>Super Admin<color:000000>.";
	}

	clientCmdMessageBoxOk(" Accepted!", %text, "");
}

function clientCmdAdminApp_Denied(%reason)
{
	%text = "<font:Impact:20><just:center>Your admin application has been<br><color:FF0000>denied<color:000000>!";

	AdminAppDenied_Body.setText(%text);
}