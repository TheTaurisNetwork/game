//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// Player class functions go here.

//-----------------------------------------------------------------------------
//       Interacting
//-----------------------------------------------------------------------------

function ShapeBase::interactLoop(%this)
{
  if (!isObject(%this))
    return;
  %this.schedule(250, interactLoop);
  %obj = %this.sightObject(10, $DefaultInteractMask);

  if (!isObject(%obj) || %obj.getClassName() $= "GroundPlane" || %obj.deployer $= "")
  {
    if (%this.interactObj !$= "")
      commandtoClient(%this.client, 'playerCanInteract', 0);
    %this.interactObj = "";
    return;
  }

  if (%obj.className() $= "Console" || %obj.className() $= "Generator" || %obj.className() $= "Drive")
  {
      if (%this.interactObj != %obj)
        commandtoClient(%this.client, 'playerCanInteract', 1);
      %this.interactObj = %obj;
  }
  else
  {
    if (%this.interactObj !$= "")
      commandtoClient(%this.client, 'playerCanInteract', 0);
    %this.interactObj = "";
    %this.programmingObject = "";
  }
}

//-----------------------------------------------------------------------------

function Armor::onTrigger(%data, %player, %trig, %val)
{
  if (%trig == 0)
  {
    if (%val)
      if (isObject(%player.testingDeploy))
        %player.deployObject(%player.testingDeploy);
  }
}

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function ShapeBase::interact(%this, %obj)
{
  if (%this.isPlayer())
    %obj.getDatablock().onInteract(%obj, %this);
}

//-----------------------------------------------------------------------------

function ShapeBase::canInteract(%this, %obj)
{
  if (!%this.isPlayer())
    return false;
  return true;

  if (!%this.isPlayer())
    return false;
  if (%obj.hacked)
    return true;
  else if (%this.team != %obj.team)
    return false;
}

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function SimObject::isPlayer(%this)
{
  if (%this.getClassName() $= "Player")
    return true;
  return false;
}

//-----------------------------------------------------------------------------



