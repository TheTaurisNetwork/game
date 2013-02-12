//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------

function Station::onDeploy(%data, %this, %player)
{
  return Asset::onDeploy(%data, %this);
}

//-----------------------------------------------------------------------------

function Station::setUp(%data, %this, %player)
{
  return %this.setPwrBranch( %this.pwrBranch );
}

//-----------------------------------------------------------------------------

function Station::onInteract(%data, %this, %player)
{
  if (!%this.isEnabled())
    return false;

  if (%data.getName() $= "InventoryStation")
    %player.buyLoadOut();
}

//-----------------------------------------------------------------------------

function Station::onEnabled(%data, %this)
{
  Asset::onEnabled(%data, %this);
}

//-----------------------------------------------------------------------------

function Station::onDisabled(%data, %this)
{
  Asset::onDisabled(%data, %this);
}

