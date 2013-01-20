//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// Weapon class functions go here.

//-----------------------------------------------------------------------------

$WeaponSlot = 0;

//-----------------------------------------------------------------------------
// Weapon Class
//-----------------------------------------------------------------------------

function Weapon::onUse(%data, %obj, %item)
{
   %slot = $WeaponSlot;
   if (%obj.getMountedImage(%slot) !$= "")
     %obj.unmountImage(%slot);

   serverPlay3D(WeaponUseSound, %obj.getTransform());

   %obj.mountImage(%data.image, %slot);
   return true;
}

function Weapon::onPickup(%this, %obj, %shape, %amount)
{
   if (Parent::onPickup(%this, %obj, %shape, %amount))
   {
      serverPlay3D(WeaponPickupSound, %shape.getTransform());
      if (%shape.getClassName() $= "Player" && %shape.getMountedImage($WeaponSlot) == 0)
         %shape.use(%realItem);
   }
}

function Weapon::onInventory(%this, %obj, %amount)
{
   if (!%amount && (%slot = %obj.getMountSlot(%this.image)) != -1)
      %obj.unmountImage(%slot);
}

//-----------------------------------------------------------------------------
// Weapon Image Class
//-----------------------------------------------------------------------------

function WeaponImage::onMount(%this, %obj, %slot)
{
   // Images assume a false ammo state on load.  We need to
   // set the state according to the current inventory.
   %ammo = %this.ammo;
   if(%ammo !$= "")
   {
      if (%obj.getInventory(%ammo))
      {
         %obj.setImageAmmo(%slot, true);
         %currentAmmo = %obj.getInventory(%ammo);
      }
      else
         %currentAmmo = 0;
   }
   %obj.client.equip(%slot);
}

function WeaponImage::onUnmount(%this, %obj, %slot)
{
  %obj.client.unEquip(%slot);
}

// ----------------------------------------------------------------------------
// A "generic" weaponimage onFire handler for most weapons.  Can be overridden
// with an appropriate namespace method for any weapon that requires a custom
// firing solution.

// projectileSpread is a dynamic property declared in the weaponImage datablock
// for those weapons in which bullet skew is desired.  Must be greater than 0,
// otherwise the projectile goes straight ahead as normal.  lower values give
// greater accuracy, higher values increase the spread pattern.
// ----------------------------------------------------------------------------

function WeaponImage::onFire(%this, %obj, %slot)
{
   warn("Attack");
   return;
   if ( !%this.infiniteAmmo )
      %obj.decInventory(%this.ammo, 1);

   if (%this.projectileSpread)
   {
      // We'll need to "skew" this projectile a little bit.  We start by
      // getting the straight ahead aiming point of the gun
      %vec = %obj.getMuzzleVector(%slot);

      // Then we'll create a spread matrix by randomly generating x, y, and z
      // points in a circle
      for(%i = 0; %i < 3; %i++)
         %matrix = %matrix @ (getRandom() - 0.5) * 2 * 3.1415926 * %this.projectileSpread @ " ";
      %mat = MatrixCreateFromEuler(%matrix);

      // Which we'll use to alter the projectile's initial vector with
      %muzzleVector = MatrixMulVector(%mat, %vec);
   }
   else
   {
      // Weapon projectile doesn't have a spread factor so we fire it using
      // the straight ahead aiming point of the gun
      %muzzleVector = %obj.getMuzzleVector(%slot);
   }

   // Get the player's velocity, we'll then add it to that of the projectile
   %objectVelocity = %obj.getVelocity();
   %muzzleVelocity = VectorAdd(
      VectorScale(%muzzleVector, %this.projectile.muzzleVelocity),
      VectorScale(%objectVelocity, %this.projectile.velInheritFactor));

   // Create the projectile object
   %p = new (%this.projectileType)()
   {
      dataBlock = %this.projectile;
      initialVelocity = %muzzleVelocity;
      initialPosition = %obj.getMuzzlePoint(%slot);
      sourceObject = %obj;
      sourceSlot = %slot;
      client = %obj.client;
   };
   MissionCleanup.add(%p);
   return %p;
}

// ----------------------------------------------------------------------------
// A "generic" weaponimage onAltFire handler for most weapons.  Can be
// overridden with an appropriate namespace method for any weapon that requires
// a custom firing solution.
// ----------------------------------------------------------------------------

function WeaponImage::onAltFire(%this, %obj, %slot)
{
   warn("alt Attack");
   return;
   %obj.decInventory(%this.ammo, 1);

   if (%this.altProjectileSpread)
   {
      // We'll need to "skew" this projectile a little bit.  We start by
      // getting the straight ahead aiming point of the gun
      %vec = %obj.getMuzzleVector(%slot);

      // Then we'll create a spread matrix by randomly generating x, y, and z
      // points in a circle
      for(%i = 0; %i < 3; %i++)
         %matrix = %matrix @ (getRandom() - 0.5) * 2 * 3.1415926 * %this.altProjectileSpread @ " ";
      %mat = MatrixCreateFromEuler(%matrix);

      // Which we'll use to alter the projectile's initial vector with
      %muzzleVector = MatrixMulVector(%mat, %vec);
   }
   else
   {
      // Weapon projectile doesn't have a spread factor so we fire it using
      // the straight ahead aiming point of the gun.
      %muzzleVector = %obj.getMuzzleVector(%slot);
   }

   // Get the player's velocity, we'll then add it to that of the projectile
   %objectVelocity = %obj.getVelocity();
   %muzzleVelocity = VectorAdd(
      VectorScale(%muzzleVector, %this.altProjectile.muzzleVelocity),
      VectorScale(%objectVelocity, %this.altProjectile.velInheritFactor));

   // Create the projectile object
   %p = new (%this.projectileType)()
   {
      dataBlock = %this.altProjectile;
      initialVelocity = %muzzleVelocity;
      initialPosition = %obj.getMuzzlePoint(%slot);
      sourceObject = %obj;
      sourceSlot = %slot;
      client = %obj.client;
   };
   MissionCleanup.add(%p);
   return %p;
}

//-----------------------------------------------------------------------------
// Ammmo Class
//-----------------------------------------------------------------------------

function Ammo::onPickup(%this, %obj, %shape, %amount)
{
   // The parent Item method performs the actual pickup.
   if (Parent::onPickup(%this, %obj, %shape, %amount))
      serverPlay3D(AmmoPickupSound, %shape.getTransform());
}

function Ammo::onInventory(%this, %obj, %amount)
{
   // The ammo inventory state has changed, we need to update any
   // mounted images using this ammo to reflect the new state.
   for (%i = 0; %i < 8; %i++)
   {
      if ((%image = %obj.getMountedImage(%i)) > 0)
      {
         if (%image.ammo.className $= "Ammo")
         {
            %obj.setImageAmmo(%i, %amount != 0);
            %currentAmmo = %obj.getInventory(%realItem);
            %obj.client.setAmmoAmountHud(%currentAmmo);
         }
      }
   }
}

//-----------------------------------------------------------------------------
// Tools
//-----------------------------------------------------------------------------

function ToolImage::onMount(%this, %obj, %slot)
{
  WeaponImage::onUnMount(%this, %obj, %slot);

  switch$(%this.getName())
  {
       case "textureGunImage":
          commandtoClient(%obj.client, 'pushGui', "textSwapperDlg");

          if (%this.client.tool["textureGun"] $= "")
            %this.client.tool["textureGun"] = material1;

  }
}

function ToolImage::onUnMount(%this, %obj, %slot)
{
  WeaponImage::onUnMount(%this, %obj, %slot);
}

//-----------------------------------------------------------------------------

function ToolImage::onFire(%this, %user, %slot)
{
  %obj = %user.SightObject(20, $TypeMasks::StaticShapeObjectType);
  if (%obj && %obj.className() $= "Piece")
  {
    %newDB = strReplace(%this.client.tool["textureGun"], "material", "block");

    %obj.setDatablock(%newDB);
  }
}







