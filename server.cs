$AdminApp::FilePath = "config/server/AdminApps";

function serverCmdReviewAdminApp(%client, %id, %status, %reason)
{
	if(!%client.isHost())
	{
		return;
	}

	if(!isFile(%file = $AdminApp::FilePath @ "/" @ %id @ ".txt"))
	{
		return;
	}

	%target = findClientByBL_ID(%id);

	if(%target.isSuperAdmin)
	{
		fileDelete(%file);

		return;
	}

	if(%status)
	{
		$Pref::Server::AutoAdminList = removeItemFromList($Pref::Server::AutoAdminList, %id);
		$Pref::Server::AutoSuperAdminList = removeItemFromList($Pref::Server::AutoSuperAdminList, %id);

		if(%status == 1)
		{
			$Pref::Server::AutoAdminList = addItemToList($Pref::Server::AutoAdminList, %id);
		}

		else if(%status == 2)
		{
			$Pref::Server::AutoSuperAdminList = addItemToList($Pref::Server::AutoSuperAdminList, %id);
		}

		export("$Pref::Server::*", "config/server/prefs.cs");

		if(isObject(%target))
		{
			commandToClient(%target, 'AdminApp_Accepted', %status);

			%target.isAdmin = 1;

			if(%status == 1)
			{
				%target.sendPlayerlistUpdate();
				commandToClient(%target, 'setAdminLevel', 1);

				messageAll('MsgAdminForce', '\c2%1 has become Admin (Application)', %target.name);
			}

			else if(%status == 2)
			{
				%target.isSuperAdmin = 1;
				%target.sendPlayerlistUpdate();
				commandToClient(%target, 'setAdminLevel', 2);

				messageAll('MsgAdminForce', '\c2%1 has become Super Admin (Application)', %target.name);
			}
		}

		else
		{
			if(%status == 1)
			{
				messageAll('MsgAdminForce', '\c2%1 has become Admin (Application)', %id);
			}

			else if(%status == 2)
			{
				messageAll('MsgAdminForce', '\c2%1 has become Super Admin (Application)', %id);
			}
		}
	}

	else
	{
		if(isObject(%target))
		{
			commandToClient(%target, 'AdminApp_Denied', %reason);

			%file = new fileObject();
			%file.openForWrite($AdminApp::FilePath @ "/" @ %id @ "_message.cs");

			%file.writeLine("");

			%file.close();
			%file.delete();
		}
	}

	fileDelete(%file);
}

package AdminApps
{
	function gameConnection::autoAdminCheck(%client)
	{
		%parent = parent::autoAdminCheck(%client);

		if(isFile(%file = $AdminApp::FilePath @ "/" @ %client.BL_ID @ "_message.cs"))
		{
			if(%client.isSuperAdmin)
			{
				commandToClient(%target, 'AdminApp_Accepted', 2);
			}

			else if(%client.isAdmin)
			{
				commandToClient(%target, 'AdminApp_Accepted', 1);
			}

			fileDelete(%file);
		}

		return %parent;
	}
};
activatePackage(AdminApps);
