//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

$HDerr[0] = "No Hyper-Drive detected";
$HDerr[1] = "Ignition aborted - insufficient energy for ignition";
$HDerr[2] = "Spinup aborted - insufficient energy for jump";
$HDerr[3] = "Hyper-Drive unresponsive";
$HDerr[4] = "Bad jump coordinates";
$HDerr[10] = "CATASTROPHIC FAILURE - DRIVE CRITICAL";

$HDmsg[0] = "Hyper-Drive status : Standing by";
$HDmsg[1] = "Hyper-Drive status : Spinning up drive";
$HDmsg[2] = "Hyper-Drive status : Entering hyper space";
$HDmsg[3] = "Hyper-Drive status : Re-entering normal space";


//-----------------------------------------------------------------------------
//  Console Functions
//-----------------------------------------------------------------------------

function StaticShape::setControl(%this, %gen, %branch, %bool, %latch)
{
  if (!%this.isEnabled())
    return false;

  if (%this.actuated)
    %this.getDatablock().actuate(%this);

  %this.controllingB = %branch;
  %this.controllingG = %branch == -1 ? %gen : -1;

  %this.toggle = %bool;
  %this.latching = %latch;

  %this.actuated = false;
}

//-----------------------------------------------------------------------------

function Console::onDeploy(%data, %this, %player)
{
  if (%data.getName() $= "Console")
  {
    %this.controllingG = -1;
    %this.controllingB = 0;

    %this.toggle = false;
    %this.latching = false;
  }

  %this.actuated = false;

  return Asset::onDeploy(%data, %this);
}

//-----------------------------------------------------------------------------

function Console::setUp(%data, %this, %player)
{
  return %this.setPwrBranch( %this.pwrBranch );
}

//-----------------------------------------------------------------------------

function Console::onEnabled(%data, %this)
{
  if (%this.actuated && %this.latching)
  {
    %gen = %this.getGroup().getGenerator(%this.controllingG);
    if (isObject(%gen))
      %gen.setPowering(%this.toggle);
    else if (%this.controllingB != -1)
      %this.getGroup().setBranchState(%this.controllingB, %this.toggle);
  }

  Asset::onEnabled(%data, %this);
}

//-----------------------------------------------------------------------------

function Console::onDisabled(%data, %this)
{
  if (%this.actuated && %data.getName() $= "Console")
  {
    %gen = %this.getGroup().getGenerator(%this.controllingG);
    if (isObject(%gen))
      %gen.setPowering(%this.toggle ? 0 : 1);
    else if (%this.controllingB != -1)
      %this.getGroup().setBranchState(%this.controllingB, %this.toggle ? 0 : 1);

    if (!%this.latching)
      %this.actuated = false;
  }
//  else if (%data.getName() $= "HelmConsole" && %data.getName() $= "DoorSwitch")
//    %this.actuated = false;
//  else if ()
//    %this.actuated = false;
//  else if (%data.getName() $= "TurretConsole")


  Asset::onDisabled(%data, %this);
}

//-----------------------------------------------------------------------------

function Console::onInteract(%data, %this, %player)
{
  if (!%this.isEnabled())
    return false;

  %data.actuate(%this, %player);
}

//-----------------------------------------------------------------------------

function Console::actuate(%data, %this, %player)
{
  if (!%this.isEnabled())
    return false;

  if (%data.getName() $= "Console")
  {
    %pG = %this.getGroup();
    %gen = %pG.getGenerator(%this.controllingG);

    if (!%this.actuated)
    {
      if (isObject(%gen))
        %gen.setPowering(%this.toggle);
      else
        %pG.setBranchState(%this.controllingB, %this.toggle);

      %this.actuated = true;
    }
    else
    {
      if (isObject(%gen))
        %gen.setPowering(%this.toggle ? 0 : 1);
      else
        %pG.setBranchState(%this.controllingB, %this.toggle ? 0 : 1 );

      %this.actuated = false;
    }
  }
  else if (%data.getName() $= "HelmConsole")
  {
    %g = %this.getGroup();
    if (%g.getGroup().getGameObject().jumpSeq == 0)
      %g-->HyperDrive.getDatablock().onInteract(%g-->HyperDrive);
  }
  else if (%data.getName() $= "DoorSwitch")
  {
    %door = %this.getGroup().getDoorByName(%this.door);
    if (isObject(%door))
      %door.getDatablock().onInteract(%door);
  }
  else if (%data.getName() $= "TurretConsole")
  {
    %data.connectPlayer(%this, %player);
  }

  return true;
}

//-----------------------------------------------------------------------------

function TurretConsole::connectPlayer(%data, %this, %player)
{
  %turret = %this.getGroup().getTurretByName(%this.turret);

  if (!isObject(%turret) || !%turret.isEnabled())
    echo("Data-link cannot be established");
  else
  {
    echo("Data-link established");

    %turret.mountObject(%player, 0);
    %player.mVehicle = %turret;
    %player.fireCtrlr = %this;
    //%this.mountObject(%player, 0);
    //%player.mountPos = %data.offset;

    %cl = %player.client;
    %cl.setControlObject(%turret);

    CommandtoClient(%cl, 'pushGui', "turretCtrlGui");
  }
}

//-----------------------------------------------------------------------------

function TurretConsole::disconnectPlayer(%data, %this, %player)
{
  %this.UnMountObject(0);
  %player.mVehicle = "";
  %player.fireCtrlr = "";

  echo("Data-link timed out");

  CommandtoClient(%cl, 'pushGui', "playGui");
}

//-----------------------------------------------------------------------------

function TurretConsole::turretDisabled(%data, %this)
{
  // playthread for screen off and red blinking and stuff
}

//-----------------------------------------------------------------------------

function TurretConsole::turretEnabled(%this)
{
  // playthread for screen on and lights and stuff
}


//-----------------------------------------------------------------------------






