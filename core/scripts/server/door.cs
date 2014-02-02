//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// Door class functions go here.

//-----------------------------------------------------------------------------

function Door::onDeploy(%data, %this, %player)
{
  return Asset::onDeploy(%data, %this);
}

//-----------------------------------------------------------------------------

function Door::setUp(%data, %this, %player)
{
  %this.actuated = false;

  %this.doorName = "Door0";
  %data.getValidDoorName(%this);
//  error(%this.doorname);
  %this.colMesh = spawnTSStaticShape("doorCol.dae", 1);
  %this.colMesh.setTransform(%this.getTransform());
  %this.getGroup().getGroup().getPieceGroup().add(%this.colMesh);

  return %this.setPwrBranch( %this.pwrBranch );
}

//-----------------------------------------------------------------------------

function Door::getValidDoorName(%data, %this)
{
  %aGroup = %this.getGroup();
  for (%i = 0; %i < %aGroup.getCount(); %i++)
  {
    if (%this.doorName == %aGroup.getObject(%i).doorName)
      %this.doorName = "Door"@%n++;

    else
      return %this.doorName;
  }
}

//-----------------------------------------------------------------------------

function AssetGroup::getDoorByName(%group, %name)
{
  for (%i = 0; %i < %group.getCount(); %i++)
  {
    %obj = %group.getObject(%i);
    if (strCmp(%name, %obj.doorName) == 0)
      return %obj;
  }
 return false;
}

//-----------------------------------------------------------------------------

function Door::onEnabled(%data, %this)
{
  if (%this.inMotion)
  {
    %obj.playThread( 0 );
    %this.inMotion = false;
   // if (%this.actuated)
   //   %data.openDoor(%this);
   // else
   //   %data.closeDoor(%this);
  }
}

//-----------------------------------------------------------------------------

function Door::onDisabled(%data, %this)
{
  %this.pauseThread( 0 );

  if (isEventPending(%this.stateChange))
  {
    %this.inMotion = true;
    cancel(%this.stateChange);
  }
  //Asset::onDisabled(%data, %this);
}

//-----------------------------------------------------------------------------

function Door::onInteract(%data, %this, %player)
{
  %new = %this.actuated ? 0 : 1;
  
  if (%new)
    %data.openDoor(%this);
  else
    %data.closeDoor(%this);
}

//-----------------------------------------------------------------------------

function Door::openDoor(%data, %this)
{
  if (isEventPending(%this.stateChange))
    cancel(%this.stateChange);

  %this.actuated = true;

  %this.playThread( 0, "open" );
  %col = "None";

  %this.stateChange = %data.schedule(1000, stateChange, %this, %col);
}

function Door::closeDoor(%data, %this)
{
  if (isEventPending(%this.stateChange))
    cancel(%this.stateChange);

  %this.actuated = false;

  %this.playThread( 0, "close" );
  %col = "Visible Mesh";

  %this.stateChange = %data.schedule(1000, stateChange, %this, %col);
}

//-----------------------------------------------------------------------------

function Door::stateChange(%data, %this, %col)
{
  cancel(%this.stateChange);
  %this.colMesh.collisionType = %col;
  %this.colMesh.decalType = %col;
}

//-----------------------------------------------------------------------------





