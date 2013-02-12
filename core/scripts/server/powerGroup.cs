//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------



//----------------------------------------------------------------------------
//  Simgroup Function
//-----------------------------------------------------------------------------

function assetGroup::Add(%this, %obj)
{
  parent::add(%this, %obj);
  %obj.preloadPower = true;
  %obj.setDamageState("Disabled");

  %data = %obj.getDatablock();
  if (%data.className $= "Generator")
  {
    %obj.setPowering(1);

    %i = %this.getObjectIndex(%obj);
    %name = "Gen_"@%i;

    while (!%obj.setHandle( %name ))
    {
      %name = "Gen_"@%i++;
    }
  }
}

//-----------------------------------------------------------------------------

function assetGroup::remove(%this, %obj)
{
  %data = %obj.getDatablock();
  if (%data.className $= "Generator")
    %obj.setPowering(0);
    
  parent::remove(%this, %obj);
}


//-----------------------------------------------------------------------------

function assetGroup::getGenerator(%this, %name)
{
  if (%name == -1)
    return false;
  for (%i = 0; %i < %this.getCount(); %i++)
  {
    %obj = %this.getObject( %i );
    if (%obj.nameBase $= %name && %obj.className() $= "Generator")
      return %obj;
  }
  return false;
}

//-----------------------------------------------------------------------------

function assetGroup::incPower(%this, %pwr)
{
  %r = %this.setPower(%this.getPower() + %pwr);

//  %this.removeLoad(%pwr);
  return %r;
}

//-----------------------------------------------------------------------------

function assetGroup::decPower(%this, %pwr)
{
  %r = %this.setPower(%this.getPower() - %pwr);

  %this.checkPwr();
  return %r;
}

//-----------------------------------------------------------------------------

function assetGroup::checkPwr(%this)
{
  if (%this.getPowerLoad() > %this.getPower())
  {
    %this.turnOffBiggestLoad();
    %this.checkPwr();
  }
}

//-----------------------------------------------------------------------------

function assetGroup::enoughPower(%this, %load)
{
  if (%this.getPowerLoad() + %load <= %this.getPower())
    return true;
  return false;
}

//-----------------------------------------------------------------------------

function assetGroup::turnOffBiggestLoad(%this)
{
  for (%i = 0; %i < %this.getCount(); %i++)
  {
    %obj = %this.getObject(%i);
    if (%obj.needsPower && %obj.isEnabled())
    {
      if (%h.pwrRequired < %obj.pwrRequired)
         %h = %obj;
    }
  }
  %h.setPwrState(0);
}

//-----------------------------------------------------------------------------

function assetGroup::setPower(%this, %pwr)
{
  %pwr = %pwr < 0 ? 0 : %pwr;
  if (%this.power != %pwr)
  {
    %this.power = %pwr;
    %this.sigRadius = (%pwr / 100);

    return true;
  }
  return false;
}

//-----------------------------------------------------------------------------

function assetGroup::getPower(%this)
{
  return %this.power;
}

//-----------------------------------------------------------------------------

function assetGroup::getSigRadius(%this)
{
  return %this.sigRadius;
}

//-----------------------------------------------------------------------------

function assetGroup::removeLoad(%this, %pwr)
{
  return %this.powerLoad = %this.powerLoad - %pwr;
}

//-----------------------------------------------------------------------------

function assetGroup::addLoad(%this, %pwr)
{
  return %this.powerLoad = %this.powerLoad + %pwr;
}

//-----------------------------------------------------------------------------

function assetGroup::getPowerLoad(%this)
{
  return %this.powerLoad;
}

//-----------------------------------------------------------------------------

function assetGroup::createBranch(%this)
{
  if (%this.branch[max] $= "")
    %this.branch[max] = 0;

  %this.branch[%this.branch[max]] = 1;   // branch enable state

  %this.branch[max]++;

  return true;
}

//-----------------------------------------------------------------------------

function assetGroup::createBranches(%this, %num)
{
  for (%i = %this.branch[max]; %i < %num; %i++)
     %this.createBranch();
}

//-----------------------------------------------------------------------------

function assetGroup::setBranchState(%this, %branch, %bool)
{
  if (%this.branch[%branch] $= "" || %this.branch[%branch] == %bool)
    return false;

  %this.branch[%branch] = %bool;

  %group = %this.getGroup();
  for (%i = 0; %i < %group.getCount(); %i++)
  {
     %obj = %group.getObject(%i);
     if (%obj.needsPower && %obj.pwrBranch == %branch)
       %obj.setPowerState(%bool);
  }
  warn("Branch : "@ %branch @" is "@ %bool);
  return true;
}

//-----------------------------------------------------------------------------

function assetGroup::getBranchState(%this, %branch)
{
  return %this.branch[%branch];
}

//=============================================================================

function assetGroup::enableLights(%this)
{
  for (%i = 0; %i < %this.getCount(); %i++)
  {
    %obj = %this.getObject(%i);
    if (%obj.className() $= "Lights")
      %obj.setPwrBranch( %obj.pwrBranch );
  }
}

//-----------------------------------------------------------------------------

function assetGroup::disableLights(%this)
{
  for (%i = 0; %i < %this.getCount(); %i++)
  {
    %obj = %this.getObject(%i);
    if (%obj.className() $= "Lights")
      %obj.setPowerState( 0 );
  }
}

//-----------------------------------------------------------------------------

function assetGroup::setLightingBranch(%this, %b)
{
  for (%i = 0; %i < %this.getCount(); %i++)
  {
    %obj = %this.getObject(%i);
    if (%obj.className() $= "Lights")
      %obj.setPwrBranch( %b );
  }
}

//=============================================================================

function assetGroup::setGravityBranch(%this, %b)
{
  %this.gravityBranch = %b;
  %this.getGroup().getShipObject().gravityMode(1);
}

//=============================================================================

function assetGroup::setElevatorBranch(%this, %b)
{
  for (%i = 0; %i < %this.getCount(); %i++)
  {
    %obj = %this.getObject(%i);
    if (%obj.className() $= "Elevator")
      %obj.setPwrBranch(%b);
  }
}

//-----------------------------------------------------------------------------

function assetGroup::enableElevators(%this)
{
  for (%i = 0; %i < %this.getCount(); %i++)
  {
    %obj = %this.getObject(%i);
    if (%obj.className() $= "Elevator")
      %obj.setPwrBranch( %obj.pwrBranch );
  }
}

//-----------------------------------------------------------------------------

function assetGroup::disableElevators(%this)
{
  for (%i = 0; %i < %this.getCount(); %i++)
  {
    %obj = %this.getObject(%i);
    if (%obj.className() $= "Elevator")
      %obj.setPowerState( 0 );
  }
}

//-----------------------------------------------------------------------------
//  Generator Functions
//-----------------------------------------------------------------------------

function Generator::onDeploy(%data, %this, %player)
{
  return %this.setPowering(0);
}

//-----------------------------------------------------------------------------

function StaticShape::setHandle(%this, %name)
{
  %group = %this.getGroup();
  for (%i = 0; %i < %group.getCount(); %i++)
     if (%group.getObject( %i ).nameBase $= %name)
       return false;
  %this.nameBase = %name;
  return true;
}

//-----------------------------------------------------------------------------

function Generator::onEnabled(%data, %this)
{
  if (%this.isDestroyed() || %this.getGroup().class !$= "assetGroup")
    return false;

  warn(%data.getName()@"::onEnabled(%data, %this)");

  %this.getGroup().incPower( %data.powerOut );

  parent::onEnabled(%this);
}

//-----------------------------------------------------------------------------

function Generator::onDisabled(%data, %this)
{
  if (%this.getGroup().class $= "assetGroup")
    %this.getGroup().decPower( %data.powerOut );

  warn(%data.getName()@"::onDisabled(%data, %this)");

  parent::onDisabled(%this);
}


//-----------------------------------------------------------------------------

function StaticShape::setPowering(%this, %bool)
{
 if (!%this.isDestroyed())// && %this.className() $= "Generator")
 {
    if (%bool)
      return %this.setDamageState("Enabled");
    else
      return %this.setDamageState("Disabled");
 }
 return false;
}

//-----------------------------------------------------------------------------
//  Asset Functions
//-----------------------------------------------------------------------------

function StaticShape::setPwrBranch(%this, %branch)
{
  %pGroup = %this.getGroup();
  if (!%this.needsPower || %pGroup.branch[%branch] $= "")
    return false;
  %this.pwrBranch = %branch;

  return %this.setPowerState( %pGroup.getBranchState(%branch) );
}

//-----------------------------------------------------------------------------

function StaticShape::setPowerState(%this, %bool)
{
//  if (%this.className() $= "Drive")
//  {
//    if (!%bool)
//    {

//    }
//  }

  %pGroup = %this.getGroup();

  if (!%this.needsPower || %this.isDestroyed())
    return false;

  if (%bool && %this.isEnabled())
    return false;

  if (%bool)
    if (!%pGroup.enoughPower(%this.pwrRequired))
      %bool = false;

  if (!%bool && %this.isDisabled())
    return false;

  return %this.setPowering(%bool);
}

//-----------------------------------------------------------------------------

function Asset::onEnabled(%data, %this)
{
  %this.getGroup().addLoad( %data.pwrRequired );
  warn(%data.getName()@"::onEnabled(%data, %this)");
}

//-----------------------------------------------------------------------------

function Asset::onDisabled(%data, %this)
{
  if (!%this.preloadPower)
    %this.getGroup().removeLoad( %data.pwrRequired );
  %this.preloadPower = "";
  warn(%data.getName()@"::onDisabled(%data, %this)");
}

