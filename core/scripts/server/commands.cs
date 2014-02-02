//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Misc. server commands avialable to clients
//-----------------------------------------------------------------------------

function serverCmdonClientTrigger(%client, %trig, %val)
{
  %client.getControlObject().getDatablock().onTrigger(%client.getControlObject(), %trig, %val);
}

//-----------------------------------------------------------------------------

function serverCmdsetZoomLvl(%client, %dir)
{
  if (isobject(%client.camera) && isobject(%client.player))
  {
    //%oldPos = %client.camera.gettransform();
    %client.camera.zoomLevel(%client.camera.zoomIncs * %dir);
  }
}

//-----------------------------------------------------------------------------

function serverCmdobjectSelected(%client, %class, %pos)
{
   InitContainerRadiusSearch(%pos, 1, $DefaultObjectMask);
   %obj = ContainerSearchNext();

   (%class).onSelect(%obj);
}

function serverCmdobjectDeselected(%client, %class, %pos)
{
   InitContainerRadiusSearch(%pos, 1, $DefaultObjectMask);
   %obj = ContainerSearchNext();

   (%class).onDeselect(%obj);
}

//-----------------------------------------------------------------------------

function serverCmdinteractObject(%client)
{
  if (%client.player.interactObj)
    %client.getControlObject().interact(%client.player.interactObj);
}

//-----------------------------------------------------------------------------


function serverCmdrequestDataforGui(%client, %id, %dlg, %func, %var1)
{
  switch$(%dlg)
  {
      case "prgConsoleDlg":
          %obj = %client.player.interactObj;
          if (%obj)
          {
            %client.programmingObject = %obj;
            %ship = %client.getBuildingGroup();
            if (%ship)
            {
              if (%func $= "")
              {
                %arg1 = 0;
                %pGroup = %ship.getAssetGroup();
                for (%i = 0; %i < %pGroup.getCount(); %i++)
                {
                  %gen = %pGroup.getObject(%i);
                  if (%gen)
                    %arg2 = trim(%arg2 SPC %gen.nameBase);
                }
                for (%b = 0; %b < %pGroup.branch[max]; %b++)
                {
                  %good = %pGroup.getBranchState(%b);
                  %arg3 = trim(%arg3 SPC %good);
                }
                %arg4 = %pGroup.branch[max];

              }
              else if (%func == 1)
              {
                %arg1 = 1;
                %arg2 = %obj.pwrBranch;
                %arg3 = %obj.controllingG;
                %arg4 = %obj.controllingB;
                %arg5 = %obj.toggle;
                %arg6 = %obj.latching;
              }
            }
          }
          
      case "prgDoorCtrlDlg":
          %obj = %client.player.interactObj;
          if (%obj)
          {

            %client.programmingObject = %obj;
            %ship = %client.getBuildingGroup();
            if (%ship)
            {
              %pGroup = %ship.getAssetGroup();
              for (%i = 0; %i < %pGroup.getCount(); %i++)
              {
                 %test = %pGroup.getObject(%i);
                 if (%test.getDatablockName() $= "Door")
                   %arg2 = trim(%arg2 SPC %test.doorName);
              }

              %arg3 = %obj.pwrBranch;
              %arg1 = %pGroup.branch[max];
              %arg4 = %obj.door ? %obj.door : false;
            }
          }

      case "prgFireCtrlDlg":
          %obj = %client.player.interactObj;
          if (%obj)
          {

            %client.programmingObject = %obj;
            %ship = %client.getBuildingGroup();
            if (%ship)
            {
              %pGroup = %ship.getAssetGroup();
              for (%i = 0; %i < %pGroup.getCount(); %i++)
              {
                 %test = %pGroup.getObject(%i);
                 //if (%test.getDatablockName() $= "smallTurretBase")
                 //  %arg2 = trim(%arg2 SPC %test.turretName);
                 if (%test.getDatablockName() $= "mediumShipTurret")
                   %arg2 = trim(%arg2 SPC %test.turretName);
                 //if (%test.getDatablockName() $= "largeTurretBase")
                 //  %arg2 = trim(%arg2 SPC %test.turretName);
              }

              %arg3 = %obj.pwrBranch;
              %arg1 = %pGroup.branch[max];
              %arg4 = %obj.turret ? %obj.turret : false;
            }
          }

      case "prgPowerDlg":
        %ship = %client.getBuildingGroup();
        if (%ship)
        {
            %pGroup = %ship.getAssetGroup();
            for (%i = 0; %i < %pGroup.getCount(); %i++)
            {
              %obj = %pGroup.getObject(%i);
              if (%obj.isGenerator())
              {
                if (%obj.isEnabled())
                {
                  %arg1 = Trim(%arg1 SPC %obj.getDatablock().powerOut);
                  %pwrOut += %obj.getDatablock().powerOut;
                }
                else
                  %arg1 = Trim(%arg1 SPC 0);

                %code = %obj.isEnabled() ? "\c5" : "\c4";
                %arg2 = Trim(%arg2 SPC %code @ %obj.nameBase);
              }
            }
            %arg3 = %pwrOut - %pGroup.getPowerLoad();
            %arg4 = %pGroup.branch[max];
            for (%i = 0; %i < %pGroup.branch[max]; %i++)
               %arg5 = Trim(%arg5 SPC %pGroup.getBranchState(%i));
        }
        
      case "insGeneratorDlg":
          %obj = %client.player.interactObj;
          if (%obj.className() $= "Generator")
          {
            %arg1 = %obj.nameBase;
            %arg2 = %obj.isEnabled() ? %obj.getDatablock().powerOut : 0;
            %arg3 = %obj.getDamageState();
          }

      case "insHelmConsoleDlg":
          %obj = %client.player.interactObj;
          if (%obj.getDatablock().getName() $= "HelmConsole")
          {

            %drive = %obj.getGroup().getGroup().getHyperDrive();
            %arg2 = %drive.isEnabled() ? %drive.GetEnergyLevel() / %drive.getDatablock().maxEnergy : 0;
            %arg3 = %drive.getDatablock().getMaxJumpDist(%drive);

            if (%func)
            {
              %arg1 = 1;

              %gObj = %obj.getGroup().getGroup().getGameObject();
              %sObj = %obj.getGroup().getGroup().getShipObject();
              %arg4 = %gObj.bookmarkNames;

              for (%i = 0; %i < 0; %i++)
              {
                %dist = vectorDist(%sObj.getCenterPoint(), getField(%gObj.bookmarks, %i));
                %arg5 = trim(%arg5 SPC %dist);
              }

              %arg6 = %gObj.errCodeToText();
            }
          }

      case "prgAssetPowerDlg":
          %ship = %client.getBuildingGroup();
          if (%ship)
          {
            %obj = %client.player.interactObj;
            %pGroup = %ship.getAssetGroup();

            for (%i = 0; %i < %pGroup.branch[max]; %i++)
               %arg1 = Trim(%arg1 SPC %pGroup.getBranchState(%i));
            %arg2 = %obj.pwrRequired;
            %arg3 = %obj.pwrBranch;
          }

      case "shipDataDlg":
          %ship = %client.getBuildingGroup();
          if (%ship)
          {
            %obj = %client.player.interactObj;
            %aGroup = %ship.getAssetGroup();
            %pGroup = %ship.getPieceGroup();
            
            for (%i = 0; %i < %aGroup.branch[max]; %i++)
               %arg1 = Trim(%arg1 SPC %aGroup.getBranchState(%i));

            %arg2 = 0;
            for (%i = 0; %i < %aGroup.getCount(); %i++)
            {
              %obj = %aGroup.getObject(%i);
              if (%obj.className() $= "Lights")
              {
                %arg2+=%obj.pwrRequired;
                %arg4 = %obj.pwrBranch;
              }
            }

            %arg3 = 0;
            for (%i = 0; %i < %pGroup.getCount(); %i++)
              if (%pGroup.getObject(%i) != %pGroup-->exterior)
                %arg3+=5;

            for (%i = 0; %i < %aGroup.getCount(); %i++)
            {
              %obj = %aGroup.getObject(%i);
              if (%obj.className() $= "Elevator")
                %arg6 = %obj.pwrBranch;
            }

            %arg5 = %aGroup.gravityBranch;
            %arg7 = 0;
          }
  }

  %client.sendGuiDatastream(%id, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8);
}

//-----------------------------------------------------------------------------

function serverCmdreceiveUploadDataStream(%client, %id, %dlg, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6)
{
  %player = %client.player;
  if (!isObject(%player))
    return;
//  error("%arg1" SPC %arg1);
//  error("%arg2" SPC %arg2);
//  error("%arg3" SPC %arg3);
//  error("%arg4" SPC %arg4);
//  error("%arg5" SPC %arg5);
//  error("%arg6" SPC %arg6);

  switch$(%dlg)
  {
      case "prgConsoleDlg":
          if (%client.programmingObject)
          {
            %obj = %client.programmingObject;
            %obj.setPwrBranch( %arg1 );

            %obj.setControl( %arg2, %arg3, %arg4, %arg5 );
          }

      case "prgPowerDlg":

          %ship = %client.getBuildingGroup();
          if (%ship)
          {
            switch(%arg1)
            {
               case 0:
                 %gen = %ship.getGenerator(%arg2);
                 if (isObject(%gen))
                   %gen.setHandle( %arg3 );

               case 1:
                 %ship.getAssetGroup().createBranch();

               case 2:
                 %pGroup = %ship.getAssetGroup();
                 %pGroup.setBranchState(%arg2, %pGroup.branch[%arg2] ? 0 : 1);
            }
            serverCmdrequestDataforGui(%client, %id, %dlg);
          }

      case "spawnerDlg":

          %player.testDeploy(%arg1, %arg2);

      case "insGeneratorDlg":

          %obj = %player.interactObj;
          if (isObject(%obj))
            %obj.setPowering(%obj.isEnabled() ? 0 : 1);
          serverCmdrequestDataforGui(%client, %id, %dlg);

      case "insHelmConsoleDlg":

          %obj = %player.interactObj;
          if (isObject(%obj))
          {
//            warn(%arg2);
            %gObj = %obj.getGroup().getGroup().getGameObject();
            switch(%arg1)
            {
               case 0:

                  %drive = %obj.getGroup().getGroup().getHyperDrive();
                  %coord = getField(%gObj.bookmarks, %arg1);

                  if (%hd.powering)
                    %gObj.setErrCode( %drive, 5 );
                  else if (getWordCount(%gObj.setJumpCoords(%coord)) == 1)
                    %gObj.setErrCode( %drive, 4 );
                  else
                    %drive.getDatablock().onInteract(%drive);

               case 1:

                  %cp = %obj.getGroup().getGroup().getShipObject().getCenterPoint();
                  %drive = %obj.getGroup().getGroup().getHyperDrive();

                  if (%hd.powering)
                    %gObj.setErrCode( %drive, 5 );
                  else if (getWordCount(%gObj.setJumpCoords(vectorAdd(%arg2, %cp))) == 1)
                    %gObj.setErrCode( %drive, 4 );
                  else
                    %drive.getDatablock().onInteract(%drive);


               case 2:

                  %cp = %obj.getGroup().getGroup().getShipObject().getCenterPoint();
                  %gObj.addBookmark(%cp, %arg2);
            }
         }

      case "prgAssetPowerDlg":

          %obj = %player.interactObj;
          if (isObject(%obj))
            %obj.setPwrBranch(%arg1);
          serverCmdrequestDataforGui(%client, %id, %dlg);

      case "shipDataDlg":
//          error(%arg1);
//          error(%arg2);
          %sObject = %client.getBuildingGroup().getShipObject();
          if (%sObject)
          {
            if (%arg1 == 0)
            {
              switch(%arg2)
              {
                 case "0":  // Lights
                     %sObject.interiorLighting(%sObject.lights ? 0 : 1);

                 case "1":  // Gravity
                     %sObject.gravityMode(%sObject.gravityMode ? 0 : 1);

                 case "2":  // Elevator Gravity Field
                     %sObject.elevatorMode(%sObject.elevatorMode ? 0 : 1);
              }
            }
            else if (%arg1 == 1)
            {
              %aGroup = %client.getBuildingGroup().getAssetGroup();
              if (%aGroup)
              {
                %aGroup.setLightingBranch(%arg2);
                %aGroup.setGravityBranch(%arg3);
                %aGroup.setElevatorBranch(%arg4);
              }
            }
          }
          serverCmdrequestDataforGui(%client, %id, %dlg);

      case "prgDoorCtrlDlg":

          %obj = %player.interactObj;
          if (isObject(%obj))
            %obj.setPwrBranch(%arg1);

          %obj.door = %arg2;

      case "prgFireCtrlDlg":

          %obj = %player.interactObj;
          if (isObject(%obj))
            %obj.setPwrBranch(%arg1);

          if (!%obj.getGroup().getCtrlrByTurret(%arg2))
            %obj.turret = %arg2;
  }
}

//----------------------------------------------------------------------------
// Tool commands
//----------------------------------------------------------------------------

function serverCmdsetToolMode( %client, %tool, %arg1 )
{
  switch$(%tool)
  {
      case "textureGun":
        %client.tool["textureGun"] = %arg1;

  }
}

//----------------------------------------------------------------------------
// Inventory
//----------------------------------------------------------------------------

function serverCmdreceiveInvFav(%client, %index, %list)
{
  %client.favLoadout[%index] = %list;

  validateLoadout(%client, %index, %list);
}

function serverCmdupdateInvFav(%client, %index, %list)
{
  serverCmdreceiveInvFav(%client, %index, %list);

  %client.sendGuiDatastream("InventoryDlg", %index, %list);
}

//----------------------------------------------------------------------------

function serverCmdselectLoadout(%client, %index)
{
  if (%index >= 0 && %index < 6)
  {
    %client.selFav = %index;
    messageClient(%client, 'invMsg', "Loadout " @ %index );
  }
}

//----------------------------------------------------------------------------
// Flashlight
//----------------------------------------------------------------------------

function serverCmdtoggleFlashlight(%client)
{
 %player = %client.getControlObject();
 if (%player.isPlayer())
 {
   %player.flashLight = %player.flashLight ? 0 : 1;
   %player.flashLight( %player.flashLight );
 }
}

//----------------------------------------------------------------------------
// Debug commands
//----------------------------------------------------------------------------

function serverCmdNetSimulateLag( %client, %msDelay, %packetLossPercent )
{
   if ( %client.isAdmin )
      %client.setSimulatedNetParams( %packetLossPercent / 100.0, %msDelay );   
}

//----------------------------------------------------------------------------
// Camera commands
//----------------------------------------------------------------------------

function serverCmdTogglePathCamera(%client, %val)
{
   if(%val)
   {
      %control = %client.PathCamera;
   }
   else
   {
      %control = %client.camera;
   }
   %client.setControlObject(%control);
   clientCmdSyncEditorGui();
}

function serverCmdToggleCamera(%client)
{
   if (%client.getControlObject() == %client.player)
   {
      %client.camera.setVelocity("0 0 0");
      %control = %client.camera;
   }
   else
   {
      %client.player.setVelocity("0 0 0");
      %control = %client.player;
   }
   %client.setControlObject(%control);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraPlayer(%client)
{
   // Switch to Player Mode
   %client.player.setVelocity("0 0 0");
   %client.setControlObject(%client.player);
   ServerConnection.setFirstPerson(1);
   $isFirstPersonVar = 1;

   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraPlayerThird(%client)
{
   // Swith to Player Mode
   %client.player.setVelocity("0 0 0");
   %client.setControlObject(%client.player);
   ServerConnection.setFirstPerson(0);
   $isFirstPersonVar = 0;

   clientCmdSyncEditorGui();
}

function serverCmdDropPlayerAtCamera(%client)
{
   // If the player is mounted to something (like a vehicle) drop that at the
   // camera instead. The player will remain mounted.
   %obj = %client.player.getObjectMount();
   if (!isObject(%obj))
      %obj = %client.player;

   %obj.setTransform(%client.camera.getTransform());
   %obj.setVelocity("0 0 0");

   %client.setControlObject(%client.player);
   clientCmdSyncEditorGui();
}

function serverCmdDropCameraAtPlayer(%client)
{
   %client.camera.setTransform(%client.player.getEyeTransform());
   %client.camera.setVelocity("0 0 0");
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdCycleCameraFlyType(%client)
{
   if(%client.camera.getMode() $= "Fly")
	{
		if(%client.camera.newtonMode == false) // Fly Camera
		{
			// Switch to Newton Fly Mode without rotation damping
			%client.camera.newtonMode = "1";
			%client.camera.newtonRotation = "0";
			%client.camera.setVelocity("0 0 0");
		}
		else if(%client.camera.newtonRotation == false) // Newton Camera without rotation damping
		{
			// Switch to Newton Fly Mode with damped rotation
			%client.camera.newtonMode = "1";
			%client.camera.newtonRotation = "1";
			%client.camera.setAngularVelocity("0 0 0");
		}
		else // Newton Camera with rotation damping
		{
			// Switch to Fly Mode
			%client.camera.newtonMode = "0";
			%client.camera.newtonRotation = "0";
		}
		%client.setControlObject(%client.camera);
		clientCmdSyncEditorGui();
	}
}

function serverCmdSetEditorCameraStandard(%client)
{
   // Switch to Fly Mode
   %client.camera.setFlyMode();
   %client.camera.newtonMode = "0";
   %client.camera.newtonRotation = "0";
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraNewton(%client)
{
   // Switch to Newton Fly Mode without rotation damping
   %client.camera.setFlyMode();
   %client.camera.newtonMode = "1";
   %client.camera.newtonRotation = "0";
   %client.camera.setVelocity("0 0 0");
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraNewtonDamped(%client)
{
   // Switch to Newton Fly Mode with damped rotation
   %client.camera.setFlyMode();
   %client.camera.newtonMode = "1";
   %client.camera.newtonRotation = "1";
   %client.camera.setAngularVelocity("0 0 0");
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorOrbitCamera(%client)
{
   %client.camera.setEditOrbitMode();
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorFlyCamera(%client)
{
   %client.camera.setFlyMode();
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdEditorOrbitCameraSelectChange(%client, %size, %center)
{
   if(%size > 0)
   {
      %client.camera.setValidEditOrbitPoint(true);
      %client.camera.setEditOrbitPoint(%center);
   }
   else
   {
      %client.camera.setValidEditOrbitPoint(false);
   }
}

function serverCmdEditorCameraAutoFit(%client, %radius)
{
   %client.camera.autoFitRadius(%radius);
   %client.setControlObject(%client.camera);
  clientCmdSyncEditorGui();
}
//----------------------------------------------------------------------------
// Weapons
//----------------------------------------------------------------------------




//----------------------------------------------------------------------------
// Server admin
//----------------------------------------------------------------------------

function serverCmdSAD( %client, %password )
{
   if( %password !$= "" && %password $= $ServerPref::AdminPassword)
   {
      %client.isAdmin = true;
      %client.isSuperAdmin = true;
      %name = getTaggedString( %client.playerName );
      MessageAll( 'MsgAdminForce', "\c2" @ %name @ " has become Admin by force.", %client );   
   }
}

function serverCmdSADSetPassword(%client, %password)
{
   if(%client.isSuperAdmin)
      $ServerPref::AdminPassword = %password;
}


//----------------------------------------------------------------------------
// Server chat message handlers
//----------------------------------------------------------------------------

function serverCmdTeamMessageSent(%client, %text)
{
   if(strlen(%text) >= $ServerPref::MaxChatLen)
      %text = getSubStr(%text, 0, $ServerPref::MaxChatLen);
   chatMessageTeam(%client, %client.team, '\c3%1: %2', %client.playerName, %text);
}

function serverCmdMessageSent(%client, %text)
{
   if(strlen(%text) >= $ServerPref::MaxChatLen)
      %text = getSubStr(%text, 0, $ServerPref::MaxChatLen);
   chatMessageAll(%client, '\c4%1: %2', %client.playerName, %text);
}

