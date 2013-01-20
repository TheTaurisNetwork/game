//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// ShipGroup SimGroup Commands

//-----------------------------------------------------------------------------
//  Simgroup Function
//-----------------------------------------------------------------------------

function ShipGroup::createAssetGroup(%this)
{
  %group = new simGroup()
  {
      class = "assetGroup";
      internalName = "assetGroup";

      power = 0;
      netPower = 0;
      branch[max] = 0;
  };
  %this.add( %group );
  %group.createBranches(3);
}


function ShipGroup::getAssetGroup(%this)
{
  %i = %this-->assetGroup;
  if (isObject(%i))
    return %i;
  else
    return false;
}

//-----------------------------------------------------------------------------

function ShipGroup::CreatePieceGroup(%this)
{
  %group = new simGroup()
  {
      class = "pieceGroup";
      internalName = "pieceGroup";
  };
  %this.add( %group );
}

function ShipGroup::getPieceGroup(%this)
{
  %i = %this-->pieceGroup;
  if (isObject(%i))
    return %i;
  else
    return false;
}

//-----------------------------------------------------------------------------

function ShipGroup::CreateShipObject(%this)
{
  %group = new scriptObject()
  {
      class = "shipObject";
      internalName = "shipObject";
  };
  %this.add( %group );
}

function ShipGroup::getShipObject(%this)
{
  %i = %this-->shipObject;
  if (isObject(%i))
    return %i;
  else
    return false;
}

//-----------------------------------------------------------------------------

function ShipGroup::getGameObject(%this)
{
  %i = %this-->gameObject;
  if (isObject(%i))
    return %i;
  else
    return false;
}

//-----------------------------------------------------------------------------

function ShipGroup::refreshAssetGroup(%this)
{
  %group = %this.getassetGroup();
  for (%i = 0; %i < %group.getCount(); %i++)
  {
     %obj = %group.getObject(%i);
     if (%obj.enabled)
       %obj.onEnergize();
     else
       %obj.onDeEnergize();
  }
}

//-----------------------------------------------------------------------------

function ShipGroup::getGenerator(%this, %name)
{
  return %this.getAssetGroup.getGenerator(%name);
}

//-----------------------------------------------------------------------------

function ShipGroup::getHyperDrive(%this)
{
  %i = %this-->assetGroup-->hyperDrive;
  if (isObject(%i))
    return %i;
  else
    return false;
}

//-----------------------------------------------------------------------------
//  Various
//-----------------------------------------------------------------------------



//-----------------------------------------------------------------------------



//-----------------------------------------------------------------------------

