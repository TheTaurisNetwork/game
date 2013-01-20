//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------

function DefaultGame::activatePackages(%game)
{
   // activate the default package for the game type
   activatePackage(DefaultGame);
   if(isPackage(%game.class) && %game.class !$= DefaultGame)
      activatePackage(%game.class);
}

function DefaultGame::deactivatePackages(%game)
{
   deactivatePackage(DefaultGame);
   if(isPackage(%game.class) && %game.class !$= DefaultGame)
      deactivatePackage(%game.class);
}

//package DefaultGame {
//};
//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

$Camera::MovementSpeed = 30;

function DefaultGame::initGameType(%game)
{
  $Game::DefaultPlayerSpawnGroups  = "PlayerSpawnPoints";
  $Game::DefaultCameraSpawnGroups  = "CameraSpawnPoints PlayerSpawnPoints";

  $Game::DefaultPlayerDatablock  = "LightMale";
}

//-----------------------------------------------------------------------------

function DefaultGame::onStart(%game)
{
    if ($Game::Running)
    {
        error("startGame(): End the game first!");
        return;
    }

    $Game::Running = true;
}

//-----------------------------------------------------------------------------

function DefaultGame::onEnd(%game)
{
   if (!$Game::Running)
   {
      error("endGame(): No game running!");
      return;
   }

   // Inform the client the game is over
   for( %clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++ )
   {
      %cl = ClientGroup.getObject( %clientIndex );
      commandToClient(%cl, 'GameEnd');
   }

   // Delete all the temporary mission objects
   resetMission();
   $Game::Running = false;
}

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function DefaultGame::onClientEnterGame(%game, %this)
{
   // Find a spawn point for the camera
   %cameraSpawnPoint = pickCameraSpawnPoint($Game::DefaultCameraSpawnGroups);
   // Spawn a camera for this client using the found %spawnPoint
   %game.spawnCamera(%this, %cameraSpawnPoint);

   // Find a spawn point for the player
   %playerSpawnPoint = pickPlayerSpawnPoint($Game::DefaultPlayerSpawnGroups);
   // Spawn a camera for this client using the found %spawnPoint
   %game.spawnPlayer(%this, %playerSpawnPoint);

}

function DefaultGame::onClientLeaveGame(%this, %client)
{
   // Cleanup the camera
   if (isObject(%client.camera))
      %client.camera.delete();
   // Cleanup the player
   if (isObject(%client.player))
      %client.player.delete();
}

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function DefaultGame::spawnCamera(%game, %this, %spawnPoint)
{
   // Set the control object to the default camera
   if (!isObject(%this.camera))
   {
     %this.camera = SpawnObject("Camera", "Observer");
   }
   // If we have a camera then set up some properties
   if (isObject(%this.camera))
   {
      MissionCleanup.add( %this.camera );
      %this.camera.scopeToClient(%this);

      %this.setControlObject(%this.camera);
   }
}

function DefaultGame::spawnPlayer(%game, %this, %spawnPoint)
{
   if (isObject(%this.player))
     return error("Attempting to create a player for a client that already has one");

   %spawnDataBlock = "LightMaleBuilder";

   if (isObject(%this.ship))
   {
     %ss = %this.ship.getSpawnPoint();
     %x = getRandom(%ss.radius*-1, %ss.radius);
     %y = getRandom(%ss.radius*-1, %ss.radius);

     %spawnPoint = vectorAdd(%ss.getPosition(), %x SPC %y SPC 0);
   }

   // Create a default player
   %player = spawnObject("Player", %spawnDataBlock);


   // Treat %spawnPoint as a transform
   %player.setPosition(%spawnPoint);

   // If we didn't actually create a player object then bail
   if (!isObject(%player))
   {
      // Make sure we at least have a camera
      %this.spawnCamera(%spawnPoint);

      return;
   }

   // Update the default camera to start with the player
   if (isObject(%this.camera))
   {
      if (%player.getClassname() $= "Player")
         %this.camera.setTransform(%player.getEyeTransform());
      else
         %this.camera.setTransform(%player.getTransform());
   }

   MissionCleanup.add(%player);

   %player.client = %this;

   if (%player.isMethod("setShapeName"))
      %player.setShapeName(%this.playerName);

   if (%player.isMethod("setEnergyLevel"))
      %player.setEnergyLevel(%player.getDataBlock().maxEnergy);

   %this.player = %player;

   if( $startWorldEditor )
   {
      %control = %this.camera;
      %control.mode = toggleCameraFly;
      EditorGui.syncCameraGui();
   }
   else
      %control = %player;

   if(isDefined("%control"))
      %this.setControlObject(%control);

   %this.player.interactLoop();
}

//-----------------------------------------------------------------------------
// Handle a player's death
//-----------------------------------------------------------------------------

function DefaultGame::onClientDeath(%game, %this, %sourceObject, %sourceClient, %damageType, %damLoc)
{
   messageAll('PlayerDeathMsg', '%1, was killed by %2.', %this.player.nameBase, %sourceclient.player.nameBase);

   // Clear out the name on the corpse
   if (isObject(%this.player))
   {
      if (%this.player.isMethod("setShapeName"))
         %this.player.setShapeName("");
   }

    // Switch the client over to the death cam
    if (isObject(%this.camera) && isObject(%this.player))
    {
        %this.camera.setMode("Corpse", %this.player);
        %this.setControlObject(%this.camera);
    }

    // Unhook the player object
    %this.player = 0;
}




//-----------------------------------------------------------------------------
// Ship - GameObject
//-----------------------------------------------------------------------------

function ShipGameObject::init(%this, %ship, %client)
{

}

//-----------------------------------------------------------------------------

function ShipGameObject::setJumpCoords(%this, %coords)
{
  %hd = %this.getGroup().getHyperDrive();
  if (%hd.isEnabled())
  {
    %sObj = %this.getGroup().getShipObject();
    if (vectorDist(%sObj.getCenterPoint(), %coords) > %sObj.shipHDfield * 2)
      return %this.jumpCoords = %coords;
  }
  return false;
}

//-----------------------------------------------------------------------------

function ShipGameObject::getJumpCoords(%this)
{
  if (!%this.getGroup().getHyperDrive().isEnabled() || %this.jumpCoords $= "")
    return false;
  return %this.jumpCoords;
}

//-----------------------------------------------------------------------------

function ShipGameObject::setJumpSphereExtent(%this, %d)
{
  %hd = %this.getGroup().getHyperDrive();
  if (!%hd.isEnabled() || %hd.powering)
    return false;
  if (%d < 1 || %d > %this.getGroup().getShipObject().calcHDfieldExtent())
    return false;
  return %this.jumpSphere = %d;
}

//-----------------------------------------------------------------------------

function ShipGameObject::getJumpSphereExtent(%this)
{
  %hd = %this.getGroup().getHyperDrive();
  if (!%hd.isEnabled() || %this.jumpSphere $= "")
    return false;
  return %this.jumpSphere;
}

//-----------------------------------------------------------------------------

function ShipGameObject::driveCountDown(%this, %num)
{
  %this.num = (%this.num $= "") ? %num : %this.num--;
  messageShip(%this.getGroup(), 'shipMsg', "++Hyper-Drive > Spin-Up " @ %this.num @ "++");

  if (%this.num == 1)
    %this.num = "";
}

//-----------------------------------------------------------------------------

function ShipGameObject::setErrCode(%gameObject, %this, %code)
{
  error("Jump code" SPC %code);
  switch(%code)
  {
     case 0:                                 // clear error codes
         %gameObject.errCode = 0;
         %this.powering = 0;

     case 1:                                 // abort during ignition
         %gameObject.errCode = 1;
         %gameObject.jumpSeq = 0;

         messageShip(%this.getGroup().getGroup(), 'shipMsg', "++Hyper-Drive > Ignition failure++");

     case 2:                                 // abort during spinup
         %gameObject.errCode = 2;
         %this.powering = false;
         %gameObject.jumpSeq = 0;

          %this.startEnergyRecharge();

         if (isEventPending(%this.jumpSeq))
           cancel(%this.jumpSeq);
         %this.jumpSeq = "";

         messageShip(%this.getGroup().getGroup(), 'shipMsg', "++Hyper jump aborted : Insufficient energy++");

     case 3:                                 // non-responsive
         %gameObject.errCode = 3;
         %this.powering = false;
         %gameObject.jumpSeq = 0;

         if (isEventPending(%this.jumpSeq))
           cancel(%this.jumpSeq);
         %this.jumpSeq = "";

         messageShip(%this.getGroup().getGroup(), 'shipMsg', "++Hyper-Drive > Unresponsive++");

     case 4:                                 // bad jump coords
         %gameObject.errCode = 4;
         messageShip(%this.getGroup().getGroup(), 'shipMsg', "++Bad jump coordinates++");

     case 5:                                 // bad jump coords
         %gameObject.errCode = 5;
         messageShip(%this.getGroup().getGroup(), 'shipMsg', "++Hyper-Drive > Spindown in process++");

     default:                                // catastrophic failure
         %gameObject.errCode = 10;
         %this.powering = false;
         %gameObject.jumpSeq = 0;

         messageShip(%this.getGroup().getGroup(), 'shipMsg', "++Hyper-Drive > Critical++");

         if (isEventPending(%this.jumpSeq))
           cancel(%this.jumpSeq);
         %this.jumpSeq = "";
  }

}

//-----------------------------------------------------------------------------

function ShipGameObject::errCodeToText(%this)
{
  switch(%this.errCode)
  {
     case 0:                                 // clear error codes
         return "";

     case 1:                                 // abort during ignition
         return "++Hyper-Drive > Ignition failure++";

     case 2:                                 // abort during ignition
         return "++Hyper jump aborted : Insufficient energy++";

     case 3:                                 // abort during spinup
         return "++Hyper-Drive > Unresponsive++";

     case 4:                                 // non-responsive
         return "++Bad jump coordinates++";

     case 5:                                 // bad jump coords
         return "++Hyper-Drive > Critical++";

  }
}

//-----------------------------------------------------------------------------

function ShipGameObject::addBookmark(%this, %coord, %name)
{
  %this.bookmarkNames = trim(%this.bookmarkName TAB %name);
  %this.bookmarks = trim(%this.bookmark TAB %coord);
}

//-----------------------------------------------------------------------------

function ShipGameObject::cfefefefef(%this, %coord, %name)
{
  %this.bookmark[%name] = %coord;
}

//-----------------------------------------------------------------------------



