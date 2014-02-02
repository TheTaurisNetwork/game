//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//-----------------------------------------------------------------------------

if ( isObject( moveMap ) )
   moveMap.delete();
new ActionMap(moveMap);


//------------------------------------------------------------------------------
// Non-remapable binds
//------------------------------------------------------------------------------

function escapeFromGame()
{
   if ( $Server::ServerType $= "SinglePlayer" )
      MessageBoxYesNo( "Exit", "Exit from this Mission?", "disconnect();", "");
   else
      MessageBoxYesNo( "Disconnect", "Disconnect from the server?", "disconnect();", "");
}

moveMap.bindCmd(keyboard, "escape", "", "handleEscape();");

//------------------------------------------------------------------------------
// Movement Keys
//------------------------------------------------------------------------------

$movementSpeed = 1; // m/s

function setSpeed(%speed)
{
   if(%speed)
      $movementSpeed = %speed;
}

function moveleft(%val)
{
   $mvLeftAction = %val * $movementSpeed;
}

function moveright(%val)
{
   $mvRightAction = %val * $movementSpeed;
}

function moveforward(%val)
{
   $mvForwardAction = %val * $movementSpeed;
}

function movebackward(%val)
{
   $mvBackwardAction = %val * $movementSpeed;
}

function moveup(%val)
{
   //%object = ServerConnection.getControlObject();
   
   //if(%object.isInNamespaceHierarchy("Camera"))
   $mvUpAction = %val * $movementSpeed;
}

function movedown(%val)
{
   //%object = ServerConnection.getControlObject();
   
   //if(%object.isInNamespaceHierarchy("Camera"))
   $mvDownAction = %val * $movementSpeed;
}

function turnLeft( %val )
{
   $mvYawRightSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function turnRight( %val )
{
   $mvYawLeftSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function panUp( %val )
{
   $mvPitchDownSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function panDown( %val )
{
   $mvPitchUpSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function getMouseAdjustAmount(%val)
{
   // based on a default camera FOV of 90'
   return(%val * ($cameraFov / 90) * 0.01) * $pref::Input::LinkMouseSensitivity;
}

function yaw(%val)
{
    %yawAdj = getMouseAdjustAmount(%val);

    if(ServerConnection.isControlObjectRotDampedCamera())
    {
      // Clamp and scale
      %yawAdj = mClamp(%yawAdj, -m2Pi()+0.01, m2Pi()-0.01);
      %yawAdj /= 2;
    }

    if ($Lmouse != $mvFreeLook)
       freeLook($Lmouse);

    //%mod = %yawAdj >= 0 ? 1 : -1;

    //$mvYaw += (0.1 * %mod);
    $mvYaw += %yawAdj;
}

function pitch(%val)
{
    %pitchAdj = getMouseAdjustAmount(%val);

    if(ServerConnection.isControlObjectRotDampedCamera())
    {
      // Clamp and scale
      %pitchAdj = mClamp(%pitchAdj, -m2Pi()+0.01, m2Pi()-0.01);
      %pitchAdj /= 2;
    }

    if ($Lmouse != $mvFreeLook)
       freeLook($Lmouse);

    //%mod = %pitchAdj >= 0 ? 1 : -1;

    //$mvPitch += (0.1 * %mod);
    $mvPitch += %pitchAdj;
}

function jump(%val)
{
   $mvTriggerCount2++;
}


moveMap.bind( keyboard, a, moveleft );
moveMap.bind( keyboard, d, moveright );
moveMap.bind( keyboard, left, moveleft );
moveMap.bind( keyboard, right, moveright );

moveMap.bind( keyboard, w, moveforward );
moveMap.bind( keyboard, s, movebackward );
moveMap.bind( keyboard, up, moveforward );
moveMap.bind( keyboard, down, movebackward );

moveMap.bind( keyboard, e, moveup );
moveMap.bind( keyboard, c, movedown );

moveMap.bind( keyboard, space, jump );
moveMap.bind( mouse, xaxis, yaw );
moveMap.bind( mouse, yaxis, pitch );

//------------------------------------------------------------------------------
// Message HUD functions
//------------------------------------------------------------------------------

function pageMessageHudUp( %val )
{
   if ( %val )
      pageUpMessageHud();
}

function pageMessageHudDown( %val )
{
   if ( %val )
      pageDownMessageHud();
}

function resizeMessageHud( %val )
{
   if ( %val )
      cycleMessageHudSize();
}

moveMap.bind(keyboard, "u", toggleMessageHud );
moveMap.bind(keyboard, "t", teamMessageHud );
moveMap.bind(keyboard, "pageUp", pageMessageHudUp );
moveMap.bind(keyboard, "pageDown", pageMessageHudDown );
moveMap.bind(keyboard, "o", resizeMessageHud );

//------------------------------------------------------------------------------
// Mouse Trigger
//------------------------------------------------------------------------------

function mouse0(%val)
{
   $Lmouse = %val;
   if ($mvFreeLook != %val)
   {
     if ($mvFreeLook)
       freeLook(0);
     else if (!$mvFreeLook)
       $mvTriggerCount0++;
   }
}

function freeLook(%val)
{
   //if (%val)
     //canvas.hideCursor();
   //else
     //canvas.showCursor();

   $mvFreeLook = %val;
}

function mouse1(%val)
{
   $mvTriggerCount1++;
}

//moveMap.bind( mouse, button0, mouse0 );
//moveMap.bind( mouse, button1, mouse1 );


//------------------------------------------------------------------------------
// Zoom and FOV functions
//------------------------------------------------------------------------------

function zoomWheel(%val)
{
  commandToServer('setZoomLvl', %val > 0 ? -1 : 1);
}

moveMap.bind( mouse0, zaxis, "zoomWheel" );

//------------------------------------------------------------------------------
// Camera & View functions
//------------------------------------------------------------------------------

function toggleFirstPerson(%val)
{
   if (%val)
   {
      ServerConnection.setFirstPerson(!ServerConnection.isFirstPerson());
   }
}

function toggleCamera(%val)
{
   if (%val)
      commandToServer('ToggleCamera');
}

moveMap.bind(keyboard, tab, toggleFirstPerson );
moveMap.bind(keyboard, "alt c", toggleCamera);

//------------------------------------------------------------------------------
// Demo recording functions
//------------------------------------------------------------------------------

function startRecordingDemo( %val )
{
   if ( %val )
      startDemoRecord();
}

function stopRecordingDemo( %val )
{
   if ( %val )
      stopDemoRecord();
}

moveMap.bind( keyboard, F3, startRecordingDemo );
moveMap.bind( keyboard, F4, stopRecordingDemo );


//------------------------------------------------------------------------------
// Helper Functions
//------------------------------------------------------------------------------

function dropCameraAtPlayer(%val)
{
   if (%val)
      commandToServer('dropCameraAtPlayer');
}

function dropPlayerAtCamera(%val)
{
   if (%val)
      commandToServer('DropPlayerAtCamera');
}

moveMap.bind(keyboard, "F8", dropCameraAtPlayer);
moveMap.bind(keyboard, "F7", dropPlayerAtCamera);

function bringUpOptions(%val)
{
   if (%val)
      Canvas.pushDialog(OptionsDlg);
}

GlobalActionMap.bind(keyboard, "ctrl o", bringUpOptions);


//------------------------------------------------------------------------------
// Debugging Functions
//------------------------------------------------------------------------------

$MFDebugRenderMode = 0;
function cycleDebugRenderMode(%val)
{
   if (!%val)
      return;

   $MFDebugRenderMode++;

   if ($MFDebugRenderMode > 16)
      $MFDebugRenderMode = 0;
   if ($MFDebugRenderMode == 15)
      $MFDebugRenderMode = 16;

   setInteriorRenderMode($MFDebugRenderMode);

   if (isObject(ChatHud))
   {
      %message = "Setting Interior debug render mode to ";
      %debugMode = "Unknown";

      switch($MFDebugRenderMode)
      {
         case 0:
            %debugMode = "NormalRender";
         case 1:
            %debugMode = "NormalRenderLines";
         case 2:
            %debugMode = "ShowDetail";
         case 3:
            %debugMode = "ShowAmbiguous";
         case 4:
            %debugMode = "ShowOrphan";
         case 5:
            %debugMode = "ShowLightmaps";
         case 6:
            %debugMode = "ShowTexturesOnly";
         case 7:
            %debugMode = "ShowPortalZones";
         case 8:
            %debugMode = "ShowOutsideVisible";
         case 9:
            %debugMode = "ShowCollisionFans";
         case 10:
            %debugMode = "ShowStrips";
         case 11:
            %debugMode = "ShowNullSurfaces";
         case 12:
            %debugMode = "ShowLargeTextures";
         case 13:
            %debugMode = "ShowHullSurfaces";
         case 14:
            %debugMode = "ShowVehicleHullSurfaces";
         // Depreciated
         //case 15:
         //   %debugMode = "ShowVertexColors";
         case 16:
            %debugMode = "ShowDetailLevel";
      }

      ChatHud.addLine(%message @ %debugMode);
   }
}

GlobalActionMap.bind(keyboard, "F9", cycleDebugRenderMode);

//------------------------------------------------------------------------------
//
// Start profiler by pressing ctrl f3
// ctrl f3 - starts profile that will dump to console and file
//
function doProfile(%val)
{
   if (%val)
   {
      // key down -- start profile
      echo("Starting profile session...");
      profilerReset();
      profilerEnable(true);
   }
   else
   {
      // key up -- finish off profile
      echo("Ending profile session...");

      profilerDumpToFile("profilerDumpToFile" @ getSimTime() @ ".txt");
      profilerEnable(false);
   }
}

GlobalActionMap.bind(keyboard, "ctrl F3", doProfile);

//------------------------------------------------------------------------------
// Misc.
//------------------------------------------------------------------------------

GlobalActionMap.bind(keyboard, "tilde", toggleConsole);
GlobalActionMap.bindCmd(keyboard, "alt k", "cls();","");
GlobalActionMap.bindCmd(keyboard, "alt enter", "", "Canvas.attemptFullscreenToggle();");

//------------------------------------------------------------------------------
// Tauris specific commands
//------------------------------------------------------------------------------

function weaponSlot(%val, %i)
{
  if (%val)
  {
    %data = $slot[%i];
    if (%data !$= "")
      commandtoServer('use', %data);
  }
}

//------------------------------------------------------------------------------

function mediPack(%val)
{
  if (%val)
    commandtoServer('use', "mediPack");
}

//------------------------------------------------------------------------------

function programObject(%val)
{
  if (%val && $Client::missionGameType $= "Build")
    commandtoServer('programObject');
}

//------------------------------------------------------------------------------

function insObject(%val)
{
  if (%val)
    commandtoServer('inspectObject');
  // canvas.pushDialog(insConsoleDlg);
}

//------------------------------------------------------------------------------

function interactObject(%val)
{
  if (%val)
    commandtoServer('interactObject');
}

//------------------------------------------------------------------------------

function showPowerGrid(%val)
{
  if (%val && $Client::missionGameType $= "Build")
  {
    if (prgPowerDlg.awake)
      canvas.popDialog(prgPowerDlg);
    else
      canvas.pushDialog(prgPowerDlg);
  }
}

//------------------------------------------------------------------------------

function showSpawner(%val)
{
  if (%val && $Client::missionGameType $= "Build")
  {
    if (spawnerDlg.awake)
      canvas.popDialog(spawnerDlg);
    else
      canvas.pushDialog(spawnerDlg);
  }
}

//------------------------------------------------------------------------------

function showShipData(%val)
{
  if (%val)
  {
    if (shipDataDlg.awake)
      canvas.popDialog(shipDataDlg);
    else
      canvas.pushDialog(shipDataDlg);
  }
}

//------------------------------------------------------------------------------

function showinventory(%val)
{
  if (%val)
  {
    if (inventoryDlg.awake)
      canvas.popDialog(inventoryDlg);
    else
      canvas.pushDialog(inventoryDlg);
  }
}

//------------------------------------------------------------------------------

function selectFavorite1(%val)
{
  if (%val)
    commandtoServer("selectLoadout", 0);
}

function selectFavorite2(%val)
{
  if (%val)
    commandtoServer("selectLoadout", 1);
}

function selectFavorite3(%val)
{
  if (%val)
    commandtoServer("selectLoadout", 2);
}

function selectFavorite4(%val)
{
  if (%val)
    commandtoServer("selectLoadout", 3);
}

function selectFavorite5(%val)
{
  if (%val)
    commandtoServer("selectLoadout", 4);
}

function selectFavorite6(%val)
{
  if (%val)
    commandtoServer("selectLoadout", 5);
}

//------------------------------------------------------------------------------

function toggleFlashlight(%val)
{
  if (%val)
  {
    if (!$flashlight)
    {
      commandtoServer('toggleFlashlight');
      $flashlight = true;
    }
    else
    {
      commandtoServer('toggleFlashlight');
      $flashlight = false;
    }
  }
}

//------------------------------------------------------------------------------

//moveMap.bind(keyboard, "1", weaponSlot, 0);
//moveMap.bind(keyboard, "2", weaponSlot, 1);
//moveMap.bind(keyboard, "3", weaponSlot, 2);
//moveMap.bind(keyboard, "4", weaponSlot, 3);
//moveMap.bind(keyboard, "5", weaponSlot, 4);

moveMap.bind(keyboard, "q", mediPack);
//moveMap.bind(keyboard, "r", weaponSlot, "hp");
//moveMap.bind(keyboard, "g", weaponSlot, "nade");
moveMap.bind(keyboard, "l", toggleFlashlight);

//------------------------------------------------------------------------------

moveMap.bind(keyboard, "numpadenter", showinventory);
moveMap.bind(keyboard, "numpad1", selectFavorite1);
moveMap.bind(keyboard, "numpad2", selectFavorite2);
moveMap.bind(keyboard, "numpad3", selectFavorite3);
moveMap.bind(keyboard, "numpad4", selectFavorite4);
moveMap.bind(keyboard, "numpad5", selectFavorite5);
moveMap.bind(keyboard, "numpad6", selectFavorite6);

//------------------------------------------------------------------------------

//moveMap.bind(keyboard, "g", );
moveMap.bind(keyboard, "e", interactObject);
moveMap.bind(keyboard, "p", programObject);
moveMap.bind(keyboard, "i", insObject);
moveMap.bind(keyboard, "F2", showPowerGrid);
moveMap.bind(keyboard, "F1", showSpawner);
moveMap.bind(keyboard, "F5", showShipData);






