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

  if (%obj.className() $= "Console" || %obj.className() $= "Generator" || %obj.className() $= "Drive" || %obj.className() $= "Station")
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

function Armor::onAdd(%data, %player)
{
  %player.setGravity(0);
  %player.checkIfInside(1);
}

//-----------------------------------------------------------------------------

function Armor::onHyperJump(%data, %player)
{

}

//-----------------------------------------------------------------------------

function ShapeBase::applyGravity(%this, %g)
{
  if (%g)
    %this.setGravity(-20);
  else
    %this.setGravity(0);
}

//-----------------------------------------------------------------------------

function ShapeBase::checkIfInside(%this, %last)
{
  if (%this.getDamageState() $= "Destroyed")
    return;

  %pos = %this.getPosition();
  %targetpos = vectoradd(%pos, vectorscale("0 0 -1", 20));

  %down = getWord(containerraycast(%pos, %targetpos, $DefaultLOSMask, %this), 0);

  %state = true;
  if (%down)
  {
    if (%down.getGroup().getGroup().class $= "shipGroup")
      if (%down.getGroup().getGroup().getShipObject().gravityMode)
        %state = false;
  }

  if (%down.internalName == "Exterior")
    %state = false;

  if (%last != %state)
    %this.applyGravity(%state);

  %this.InteriorLoop = %this.schedule(500, checkIfInside, %state);
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

function ShapeBase::flashLight(%this, %mode)
{
  if (%this.isPlayer())
  {
    if (%mode)
    {
      if (!isObject(%this.flObj))
      {
        %this.flObj = new SpotLight()
        {
          range = "40";
          innerAngle = "40";
          outerAngle = "45";
          isEnabled = "1";
          castShadows = "1";
        };
      }
      %this.mountObject(%this.flObj, 0);
      %this.flObj.mountPos = "0 0.4 0";
      %this.flObj.mountRot = "1 0 0 12";
    }
    else
      %this.flObj.delete();
  }
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



