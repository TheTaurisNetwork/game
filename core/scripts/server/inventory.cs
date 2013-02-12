//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// Inventory class functions go here.

//-----------------------------------------------------------------------------

$armourList[0] = Light;
$armourList[1] = Medium;
$armourList[2] = Heavy;
$armourList = 3;

$primaryList[0] = rifle;
$primaryList[1] = shotgun;
$primaryList = 2;

$secondaryList[0] = pistol;
$secondaryList[1] = nangun;
$secondaryList[2] = nanrep;
$secondaryList[3] = detonator;
$secondaryList = 4;

$packList[0] = ammopack;
$packList[1] = medicpack;
$packList[2] = repairpack;
$packList[3] = c4;
$packList = 4;

$nadeList[0] = flash;
$nadeList[1] = nade;
$nadeList[2] = emp;
$nadeList = 3;

//-----------------------------------------------------------------------------

$DbToName[Light] = "Light";
$DbToName[Medium] = "Marine";
$DbToName[Heavy] = "Armoured";
$DbToName[LightBuilder] = "Engineer";

$DbToName[rifle] = "Pulse Rifle";
$DbToName[shotgun] = "Shotgun";

$DbToName[pistol] = "Handgun";
$DbToName[nangun] = "Medi Nanite";
$DbToName[nanrep] = "Nanite Applicator";
$DbToName[detonator] = "C4 Detonator";

$DbToName[c4] = "C4 Charges";
$DbToName[ammopack] = "Ammo Pack";
$DbToName[medicpack] = "Medical Pack";
$DbToName[repairpack] = "Nanite Distro";

$DbToName[flash] = "Flashbang";
$DbToName[nade] = "M9 Frag Grenade";
$DbToName[emp] = "M2 EMP Grenade";

//-----------------------------------------------------------------------------
// Inventory server commands
//-----------------------------------------------------------------------------

function serverCmdUse(%client, %data)
{
   %client.getControlObject().use(%data);
}

//-----------------------------------------------------------------------------
// Loadouts
//-----------------------------------------------------------------------------

function ShapeBase::buyLoadOut(%this)
{
  %client = %this.client;

  validateLoadout(%client, %client.selFav, %client.favLoadout);

  %list = %client.favLoadout[%client.selFav];

  warn(%client.favLoadout[%client.selFav]);

  %armour = nametoDb(getField(%list, 1));
  %this.setDatablock(%armour);
  
  %primary = nametoDb(getField(%list, 2));
  if (isobject(%primary.ammo))
  {
    %max = %this.maxInventory(%primary.ammo);
    %this.setInventory(%primary.ammo, %max);
  }
  %max = %this.maxInventory(%primary);
  %this.setInventory(%primary, %max);

  %secondary = nametoDb(getField(%list, 3));
  if (isobject(%secondary.ammo))
  {
    %max = %this.maxInventory(%secondary.ammo);
    %this.setInventory(%secondary.ammo, %max);
  }
  %max = %this.maxInventory(%secondary);
  %this.setInventory(%secondary, %max);

  %pack = nametoDb(getField(%list, 4));
  %max = %this.maxInventory(%pack);
  %this.setInventory(%pack, %max);

  %misc = nametoDb(getField(%list, 5));
  %max = %this.maxInventory(%misc);
  %this.setInventory(%misc, %max);
}

//-----------------------------------------------------------------------------

function validateLoadout(%client, %index, %list)
{
  %pack = getField(%list, 4);
  %sec = getField(%list, 3);

  %req = nametoDb(%pack).required;
  if (%req !$= "")
    if (%req != nametoDb(%sec))
      %list = strReplace(%list, %sec, $DbToName[%req]);

  %client.favLoadout[%index] = %list;
}

//-----------------------------------------------------------------------------

function nametoDb(%name)
{
 for (%i = 0; %i < $armourList; %i++)
 {
    %db = $armourList[%i];
    if ($DbToName[%db] $= %name)
      return %db;
 }

 for (%i = 0; %i < $primaryList; %i++)
 {
    %db = $primaryList[%i];
    if ($DbToName[%db] $= %name)
      return %db;
 }

 for (%i = 0; %i < $secondaryList; %i++)
 {
    %db = $secondaryList[%i];
    if ($DbToName[%db] $= %name)
      return %db;
 }

 for (%i = 0; %i < $packList; %i++)
 {
    %db = $packList[%i];
    if ($DbToName[%db] $= %name)
      return %db;
 }

 for (%i = 0; %i < $nadeList; %i++)
 {
    %db = $nadeList[%i];
    if ($DbToName[%db] $= %name)
      return %db;
 }

}

//-----------------------------------------------------------------------------
// ShapeBase inventory support
//-----------------------------------------------------------------------------

function ShapeBase::use(%this, %data)
{
   %conn = %this.getControllingClient();
   if (%conn)
   {
      %defaultFov = %conn.getControlCameraDefaultFov();
      %fov = %conn.getControlCameraFov();
      if (%fov != %defaultFov)
         return false;
   }

   if (%this.getInventory(%data) > 0)
      return %data.onUse(%this, %data);

   return false;
}

function ShapeBase::throw(%this, %data, %amount)
{
   if (%this.getInventory(%data) > 0)
   {
      %obj = %data.onThrow(%this, %data, %amount);
      if (%obj)
      {
         %this.throwObject(%obj);
         serverPlay3D(ThrowSnd, %this.getTransform());
         return true;
      }
   }
   return false;
}

function ShapeBase::pickup(%this, %obj, %amount)
{
   %data = %obj.getDatablock();

   if (%amount $= "")
    %amount = %this.maxInventory(%data) - %this.getInventory(%data);

   if (%amount < 0)
      %amount = 0;
   if (%amount)
      return %data.onPickup(%obj, %this, %amount);
   return false;
}

//-----------------------------------------------------------------------------

function ShapeBase::hasInventory(%this, %data)
{
   return (%this.inv[%data] > 0);
}

function ShapeBase::hasAmmo(%this, %weapon)
{
   if (%weapon.image.ammo $= "")
      return(true);
   else
      return(%this.getInventory(%weapon.image.ammo) > 0);
}

function ShapeBase::maxInventory(%this, %data)
{
  return %this.getDatablock().maxInv[%data];
}

function ShapeBase::incInventory(%this, %data, %amount)
{
   %total = %this.inv[%data];

   %this.setInventory(%data, %total + %amount);

   return %amount;
}

function ShapeBase::decInventory(%this, %data, %amount)
{
   %total = %this.inv[%data];
   if (%total > 0)
   {
      if (%total < %amount)
         %amount = %total;
      %this.setInventory(%data, %total - %amount);
   }
}

//-----------------------------------------------------------------------------

function ShapeBase::getInventory(%this, %data)
{
   return %this.inv[%data];
}

function ShapeBase::setInventory(%this, %data, %value, %silent)
{
   if (%value < 0)
      %value = 0;

   if (%this.inv[%data] != %value)
   {
      %this.inv[%data] = %value;

      if (!%silent)
        %this.onInventory(%data, %value);
      %data.onInventory(%this, %value);
   }

   return %value;
}

//-----------------------------------------------------------------------------

function ShapeBase::clearInventory(%this)
{
  %this.inv[rifle] = "";

  %this.client.sendInventory();
}

//-----------------------------------------------------------------------------

function ShapeBase::throwObject(%this, %obj)
{
   %throwForce = %this.throwForce;
   if (!%throwForce)
   %throwForce = 20;

   // Start with the shape's eye vector...
   %eye = %this.getEyeVector();
   %vec = vectorScale(%eye, %throwForce);

   // Add a vertical component to give the object a better arc
   %verticalForce = %throwForce / 2;
   %dot = vectorDot("0 0 1", %eye);
   if (%dot < 0)
      %dot = -%dot;
   %vec = vectorAdd(%vec, vectorScale("0 0 "@%verticalForce, 1 - %dot));

   // Add the shape's velocity
   %vec = vectorAdd(%vec, %this.getVelocity());

   // Set the object's position and initial velocity
   %pos = getBoxCenter(%this.getWorldBox());
   %obj.setTransform(%pos);
   %obj.applyImpulse(%pos, %vec);

   // Since the object is thrown from the center of the shape,
   // the object needs to avoid colliding with it's thrower.
   %obj.setCollisionTimeout(%this);
}

//-----------------------------------------------------------------------------
// Callback hooks invoked by the inventory system
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// ShapeBase object callbacks invoked by the inventory system

function ShapeBase::onInventory(%this, %data, %value)
{
  %this.client.sendItem(%data, %value);
}

//-----------------------------------------------------------------------------
// ShapeBase datablock callback invoked by the inventory system.

function ShapeBaseData::onUse(%this, %user)
{
   // Invoked when the object uses this datablock, should return
   // true if the item was used.

   return false;
}

function ShapeBaseData::onThrow(%this, %user, %amount)
{
   // Invoked when the object is thrown.  This method should
   // construct and return the actual mission object to be
   // physically thrown.  This method is also responsible for
   // decrementing the user's inventory.

   return 0;
}

function ShapeBaseData::onPickup(%this, %obj, %user, %amount)
{
   // Invoked when the user attempts to pickup this datablock object.
   // The %amount argument is the space in the user's inventory for
   // this type of datablock.  This method is responsible for
   // incrementing the user's inventory is something is addded.
   // Should return true if something was added to the inventory.

   return false;
}

function ShapeBaseData::onInventory(%this, %user, %value)
{
   // Invoked whenever an user's inventory total changes for
   // this datablock.
}
