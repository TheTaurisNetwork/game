//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------



//-----------------------------------------------------------------------------
//  Console Functions
//-----------------------------------------------------------------------------

function Console::setControl(%this, %gen, %branch)
{
  if (!%this.enabled && !%this.isEnabled())
    return false;

  %this.controllingG = %gen;
  %this.controllingB = %branch;
}

//-----------------------------------------------------------------------------

function Console::onInteract(%this, %player)
{
  %this.toggleState();
}

//-----------------------------------------------------------------------------

function Console::onInteract(%this, %player)
{
  if (!%this.enabled && !%this.isEnabled())
    return false;
  %this.toggleState();
}

//-----------------------------------------------------------------------------

function Console::toggleState(%this, %player)
{
  if (!%this.enabled && !%this.isEnabled())
    return false;

}

//-----------------------------------------------------------------------------



//-----------------------------------------------------------------------------
//  Helpers
//-----------------------------------------------------------------------------




