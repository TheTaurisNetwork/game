//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// Item class functions go here.

//-----------------------------------------------------------------------------

$Item::RespawnTime = 30 * 1000;

$Item::PopTime = 30 * 1000;

//-----------------------------------------------------------------------------
// ItemData base class methods used by all items
//-----------------------------------------------------------------------------

function Item::onUse(%this, %user, %data)
{
  if (%this.getInventory(%data) > 0)
    return %data.onUse(%this, %user);
}

//-----------------------------------------------------------------------------

function Item::respawn(%this)
{
   %this.startFade(0, 0, true);
   %this.setHidden(true);

   %this.schedule($Item::RespawnTime, "setHidden", false);
   %this.schedule($Item::RespawnTime + 100, "startFade", 1000, 0, false);
}

function Item::schedulePop(%this)
{
   %this.schedule($Item::PopTime - 1000, "startFade", 1000, 0, true);
   %this.schedule($Item::PopTime, "delete");
}

//-----------------------------------------------------------------------------
// Callbacks to hook items into the inventory system

function ItemData::onThrow(%this, %user, %amount)
{
   if (%amount $= "")
      %amount = 1;

   %user.decInventory(%this,%amount);

   %obj = new Item()
   {
      datablock = %this;
      rotation = "0 0 1 0";
      count = %amount;
   };
   MissionGroup.add(%obj);

   %obj.schedulePop();

   return %obj;
}

function ItemData::onPickup(%this, %obj, %user, %realItem, %amount)
{
    %count = %obj.count;

    if (%count $= "")
      %count = 1;
    %user.incInventory(%this, %count);

    if (%user.client)
       messageClient(%user.client, 'MsgItemPickup', '\c0You picked up %1', %obj.pickup);

    if (%obj.isStatic())
       %obj.respawn();
    else
       %obj.delete();
    return true;
}

function ItemData::createItem(%data)
{
   %obj = new Item()
   {
      dataBlock = %data;
      static = false;
      rotate = false;
   };
   return %obj;
}

//-----------------------------------------------------------------------------
// Wdwd
//-----------------------------------------------------------------------------

function mediPack::onUse(%data, %obj, %user)
{
//  %this.
}

//-----------------------------------------------------------------------------




