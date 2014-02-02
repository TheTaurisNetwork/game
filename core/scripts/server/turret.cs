//-----------------------------------------------------------------------------
// Copyright (c) 2013 The Tauris Network
//-----------------------------------------------------------------------------

// Turret class functions go here.

//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// TurretBase Class
//-----------------------------------------------------------------------------

function ShipTurret::onDeploy(%data, %this, %player)
{
  %this.static = true;
  return Asset::onDeploy(%data, %this, %player);
}

//-----------------------------------------------------------------------------

function ShipTurret::setUp(%data, %this, %player)
{
  %this.turretName = %this.getValidName(%data.size@"0");

  return %this.setPwrBranch( %this.pwrBranch );
}

//-----------------------------------------------------------------------------

function TurretShapeData::onAdd(%data, %obj)
{
   %obj.setRechargeRate(%data.rechargeRate);
   %obj.setEnergyLevel(%data.MaxEnergy);
   %obj.setRepairRate(0);

   if (isObject(%data.weapon))
   {
     %barrel = %data.weapon;

     if (%barrel.mountable = %data)
     {
       if (%barrel.nameTag !$= "")
         %obj.setShapeName(%barrel.nameTag);

       %obj.incInventory(%this.weapon, 1);

       // Mount the image
       error(%obj.mountImage(%data.weapon, 0, 1));
       %obj.setImageGenericTrigger(0, 0, false); // Used to indicate the turret is destroyed
     }
   }

//   %data.setUp(%obj);
}

//-----------------------------------------------------------------------------

function TurretShapeData::onRemove(%this, %obj)
{

}

//-----------------------------------------------------------------------------

function TurretShape::getValidName(%this, %name)
{
  %data = %this.getDatablock();
  %aGroup = %this.getGroup();
  for (%i = 0; %i < %aGroup.getCount(); %i++)
  {
     if (%aGroup.getObject(%i).isTurret())
     {
       %cmp = strCmp(%name, %aGroup.getObject(%i).turretName);

       if (%cmp == 0)
         return %data.size@%n++;
       else if (%cmp == -1)
         %name = %data.size@%n++;
     }
  }
  return %name;
}

//-----------------------------------------------------------------------------

function AssetGroup::getTurretByName(%this, %name)
{
  for (%i = 0; %i < %this.getCount(); %i++)
  {
     %obj = %this.getObject(%i);
     if (%obj.isTurret())
       if (strCmp(%name, %obj.turretName) == 0)
         return %obj;
  }
  return false;
}

//-----------------------------------------------------------------------------

function TurretShapeData::onEnabled(%data, %this)
{
  %console = %this.getGroup().getCtrlrByTurret(%this.turretName);
  echo(%console);
  %console.turretEnabled();
}

//-----------------------------------------------------------------------------

function AssetGroup::getCtrlrByTurret(%group, %name)
{
  for (%i = 0; %i < %group.getCount(); %i++)
  {
    %obj = %group.getObject(%i);
    if (strCmp(%name, %obj.turret) == 0)
      return %obj;
  }
 return false;
}

//-----------------------------------------------------------------------------

function TurretShapeData::onDisabled(%data, %this)
{
  %data.onLooseControl(%this.getControllingObject());

  %console = %this.getGroup().getCtrlrByTurret(%this.turretName).turretDisabled();
}

//-----------------------------------------------------------------------------

function TurretShapeData::onDestroyed(%data, %this)
{
  %data.onLooseControl(%this.getControllingObject());

  %console = %this.getGroup().getCtrlrByTurret(%this.turretName).turretDisabled();
}

//-----------------------------------------------------------------------------

function TurretShapeData::onLooseControl(%data, %player)
{
  if (isObject(%player))
  {
    %player.fireCtrlr.disconnectPlayer();
  }
}

//-----------------------------------------------------------------------------

function TurretShapeData::onMountObject(%data, %obj, %player, %slot)
{
  error("onMountObject");
}

function TurretShapeData::onUnmountObject(%data, %obj, %player)
{
  error("onMountObject");

}

//-----------------------------------------------------------------------------

function TurretShapeData::onMount(%this, %turret, %player, %node)
{
  error("onMount");
//   %player.client.RefreshVehicleHud(%turret, %this.reticle, %this.zoomReticle);
}

function TurretShapeData::onUnmount(%this, %turret, %player, %node)
{
  error("onUnmount");
//   %player.client.RefreshVehicleHud(0, "", "");
}


//-----------------------------------------------------------------------------

function SimObject::isTurret(%this)
{
  if (%this.getClassName() $= "TurretShape" || %this.getClassName() $= "TurretShapeData")
    return true;
  return false;
}



