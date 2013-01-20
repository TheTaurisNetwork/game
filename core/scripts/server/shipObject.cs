//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// ShipObject ScriptObject Commands

//-----------------------------------------------------------------------------

function ShipObject::getSpawnPoint(%this)
{
  %p = -1;
  for (%i = 0; %i < %this.getCount(); %i++)
  {
     %obj = %this.getObject(%i);
     if (%obj.getDatablockName() $= "SpawnSphereMarker")
       %ind[%p++] = %obj;
  }

  %ss = %ind[getRandom(0, %p)];
  if (%ss)
    return %ss;

  return false;
}

//-----------------------------------------------------------------------------

function ShipObject::setCenterPoint(%this, %cp)
{
  if (getwordCount(%cp) == 3)
    return %this.getGroup().getPieceGroup()-->exterior.setPosition(%cp);
  return false;
}


//-----------------------------------------------------------------------------

function ShipObject::getCenterPoint(%this)
{
  if (isObject(%this.getGroup().getPieceGroup()-->exterior))
    %pos = %this.getGroup().getPieceGroup()-->exterior.getPosition();
  if (%pos $= "")
  {
    if (%this.getGroup().getGroup()-->assetGroup.getCount() > 0)
      %pos = %this.getGroup().getGroup()-->assetGroup.getObject(0).getPosition();
    else if (%this.getGroup().getGroup()-->pieceGroup.getCount() > 0)
      %pos = %this.getGroup().getGroup()-->pieceGroup.getObject(0).getPosition();
  }
  return %pos;
}

//-----------------------------------------------------------------------------

function ShipObject::getShipMass(%this)
{
  return 10000;
}

//-----------------------------------------------------------------------------

function ShipObject::calcHDfieldExtent(%this)
{
  %ext = %this.getgroup()-->pieceGroup-->exterior;
  if (isObject(%ext))
  {
    %raw = %ext.getRealSize();
    for (%i = 0; %i < 2; %i++)
    {
       %vec = getword(%raw, %i);
       if (%vec > %g)
         %g = %vec;
    }
  }
  if (%g !$= "")
    return %this.shipHDfield = %g;
  return %this.shipHDfield = 10;
}

//-----------------------------------------------------------------------------

function ShipObject::getSpawnPoint(%this)
{
  %pGroup = %this.getPieceGroup();
  %rand = getRandom(0, %pGroup.getCount());
  return %pGroup.getObject(%rand);
}

//-----------------------------------------------------------------------------


