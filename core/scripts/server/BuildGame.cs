//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function BuildGame::initGameType(%game)
{
  parent::initGameType(%game);

  %this.clientLoadingDone = true;

  $armourList[$armourList] = LightBuilder;
  $armourList++;
}

//-----------------------------------------------------------------------------

function BuildGame::onStart(%game)
{
  parent::onStart(%game);
  if (!isObject(buildGroup))
  {
    new simGroup(buildGroup) {};
  }
  MissionGroup.add(buildGroup);
}                //http://www.youtube.com/watch?v=o22eIJDtKho

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function BuildGame::onClientEnterGame(%game, %this)
{
   %cameraSpawnPoint = pickPlayerSpawnPoint(%this.client.buildGroup);
   %game.spawnCamera(%this, %cameraSpawnPoint);

   %playerSpawnPoint = pickPlayerSpawnPoint(%this.client.buildGroup);
   %game.spawnPlayer(%this, %playerSpawnPoint);

   if (%this.player)
   {
     %this.selFav = 0;
     %game.defaultLoadout(%this.player);
     %this.player.setName("user");
   }
}

//function BuildGame::onClientLeaveGame(%this, %client)
//{
//  Parent::onClientLeaveGame(%this, %client);
//}

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function BuildGame::spawnPlayer(%game, %this, %spawnPoint)
{
   if (isObject(%this.player))
     return error("Attempting to create a player for a client that already has one");

   %spawnDataBlock = "LightBuilder";

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

function BuildGame::onClientDeath(%game, %this, %sourceObject, %sourceClient, %damageType, %damLoc)
{
   Parent::onClientDeath(%game, %this, %sourceObject, %sourceClient, %damageType, %damLoc);
   
}

//-----------------------------------------------------------------------------
// Advanced Shit
//-----------------------------------------------------------------------------

function BuildGame::defaultLoadout(%game, %player)
{
  %player.setInventory(rifle, 1);
  %player.setInventory(pistol, 1);
  %player.setInventory(pistolammo, 20);
  %player.setInventory(nade, 2);
  %player.setInventory(ammopack, 1);
}

//-----------------------------------------------------------------------------

function BuildGame::deployObject(%game, %obj, %player)
{
  if (!isObject(%obj) || !isObject(%player))
    return false;

  error(%obj);
  error(%obj.getClassName());
  %group = %player.client.getBuildingGroup();
  //error(%group.class);
  if (%obj.getClassName() !$= "TSStatic")
    %group.getAssetGroup().add(%obj);
  else
    %group.getPieceGroup().add(%obj);

  %obj.getDatablock().setUp(%obj, %player);
}

//-----------------------------------------------------------------------------

function BuildGame::makeBuildGroup(%game, %client)
{
  %group = new simGroup()
  {
     class = "shipGroup";
     builder = %client;
  };
  buildGroup.add(%group);

  %group.createShipObject();
  %group.createAssetGroup();
  %group.createPieceGroup();

  %client.buildGroup = %group;

  %game.initShipGameObject(%group, %client);
  return %group;
}

//-----------------------------------------------------------------------------

function BuildGame::prepareLoad(%game, %client, %file)
{
  if (%game.clientLoading || isObject(%client.buildGroup))
    return false;

  //error(isFile("~/ships/"@%file@".cs"));
  if (isFile("~/ships/"@%file@".cs"))
  {
    if (%client.isHelper())
      %client.isHelper().removeHelper(%client);

    %game.clientLoading = %client;
    %gane.clientLoadingDone = false;

    exec("~/ships/"@%file@".cs");
    return true;
  }
  else
    return error("No Such File : ~/ships/"@%file@".cs");
}

//-----------------------------------------------------------------------------

function BuildGame::loadShip(%game, %ship)
{
  buildGroup.add(%ship);

  %game.initShipGameObject(%ship, %game.clientLoading);

  %ship.refreshAssetGroup();

  %game.clientLoading.buildGroup = %ship;

  %game.clientLoading = "";
  %game.clientLoadingDone = true;
}

//-----------------------------------------------------------------------------

function BuildGame::initShipGameObject(%game, %ship, %client)
{
  %o = new scriptObject()
  {
    class = ShipGameObject;
    internalName = "gameObject";
    canSave = false;
  };
  %ship.add(%o);

  %o.init(%ship, %client);
}

//-----------------------------------------------------------------------------

function BuildGame::saveClientShip(%game, %client, %name)
{
  if (isObject(tempGroup))
    new scriptObject(tempGroup) { };

  for (%i = 0; %i < BuildGroup.getCount(); %i++)
    if (BuildGroup.getObject(%i).getGameObject().builder == %client)
      %group = BuildGroup.getObject(%i);
  if (!isObject(%group))
    return false;
  return saveShip(%group, %name);
}

//-----------------------------------------------------------------------------

package BuildGame {

function ShipGameObject::init(%this, %ship, %client)
{
  %this.builder = %client;
  %this.addBookmark("0 0 100", "Center");
}

//-----------------------------------------------------------------------------

function ShipGameObject::getBuilder(%this)
{
  return %this.builder;
}

//-----------------------------------------------------------------------------

function ShipGameObject::addHelper(%this, %client)
{
  %h = %client.isHelper();
  if (%h)
    %h.removeHelper(%client);

  %this.helpers = %this.helpers SPC %client;
}

//-----------------------------------------------------------------------------

function ShipGameObject::removeHelper(%this, %client)
{
  for (%i = 0; %i < getWordCount(%this.helpers); %i++)
    if (getWord(%this.helpers, %i) $= %client)
      %this.helpers = removeWord(%this.helpers, %i);
}

//-----------------------------------------------------------------------------

function ShipGameObject::isHelper(%this, %client)
{
  if (strstr(%this.helpers, %client) > -1)
    return true;
  return false;
}

//-----------------------------------------------------------------------------
};
