//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
//     Turret base
//-----------------------------------------------------------------------------

datablock TurretShapeData(mediumShipTurret)
{
   category = "Turrets";
   className = "shipTurret";
   shapeFile = "core/art/shapes/turretBase.dae";
   size = "med";                //turretBase

   pwrRequired = 90;
   needsPower = true;
   
   rechargeRate = 1;
   MaxEnergy = 100;
   maxDamage = 80;
   destroyedLevel = 75;
   disabledLevel = 60;
//   explosion = GrenadeExplosion;

   zRotOnly = false;

   weaponLinkType = "FireTogether";

   // Rotation settings
   minPitch = 0;
   maxPitch = 80;
   maxHeading = 180;
   headingRate = 30;
   pitchRate = 30;

   // Scan settings
   maxScanPitch = 10;
   maxScanHeading = 30;
   maxScanDistance = 20;
   trackLostTargetTime = 2;

   maxWeaponRange = 30;

   weaponLeadVelocity = 0;

   // Weapon mounting
   numWeaponMountPoints = 1;

   weapon = MPlasmaTurret;

   maxInv[MPlasmaTurret] = 1;
};


//-----------------------------------------------------------------------------

datablock ShapeBaseImageData(MPlasmaTurret)
{
   // Basic Item properties
   shapeFile = "core/art/shapes/plasmaTurret.DAE";
   emap = true;

   // Specify mount point
   mountPoint = 0;
   mountable = true;
   
   useEyeNode = 1;
   eyeOffset = "0 0 3";

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";
   category = "Weapon";
   
   nameTag = "Medium Plasma Turret";

   // Projectiles and Ammo.
   //item = AITurretHead;
   //ammo = AITurretAmmo;

   //projectile = TurretBulletProjectile;
   //projectileType = Projectile;
   //projectileSpread = "0.02";

   //casing = BulletShell;
   //shellExitDir        = "1.0 0.3 1.0";
   //shellExitOffset     = "0.15 -0.56 -0.1";
   //shellExitVariance   = 15.0;
   //shellVelocity       = 3.0;

   // Weapon lights up while firing
  // lightType = "WeaponFireLight";
  // lightColor = "0.992126 0.968504 0.708661 1";
  // lightRadius = "4";
  // lightDuration = "100";
  // lightBrightness = 2;

   // Shake camera while firing.
   shakeCamera = false;
   camShakeFreq = "0 0 0";
   camShakeAmp = "0 0 0";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   useRemainderDT = true;

   // Initial start up state
   stateName[0]                     = "Preactivate";
   stateIgnoreLoadedForReady[0]     = false;
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNotLoaded[0]    = "WaitingDeployment";  // If the turret weapon is not loaded then it has not yet been deployed
   stateTransitionOnNoAmmo[0]       = "NoAmmo";

   // Activating the gun.  Called when the weapon is first
   // mounted and there is ammo.
   stateName[1]                     = "Activate";
   stateTransitionGeneric0In[1]     = "Destroyed";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 0.5;
   stateSequence[1]                 = "Activate";

   // Ready to fire, just waiting for the trigger
   stateName[2]                     = "Ready";
   stateTransitionGeneric0In[2]     = "Destroyed";
   stateTransitionOnNoAmmo[2]       = "NoAmmo";
   stateTransitionOnTriggerDown[2]  = "Fire";
   stateSequence[2]                 = "scan";

   // Fire the weapon. Calls the fire script which does
   // the actual work.
   stateName[3]                     = "Fire";
   stateTransitionGeneric0In[3]     = "Destroyed";
   stateTransitionOnTimeout[3]      = "Reload";
   stateTimeoutValue[3]             = 0.2;
   stateFire[3]                     = true;
   stateRecoil[3]                   = "LightRecoil";
   stateAllowImageChange[3]         = false;
   stateSequence[3]                 = "fire";
   stateSequenceRandomFlash[3]      = true;        // use muzzle flash sequence
   stateScript[3]                   = "onFire";
   stateSound[3]                    = TurretFireSound;
   stateEmitter[3]                  = GunFireSmokeEmitter;
   stateEmitterTime[3]              = 0.025;
   stateEjectShell[3]               = true;

   // Play the reload animation, and transition into
   stateName[4]                     = "Reload";
   stateTransitionGeneric0In[4]     = "Destroyed";
   stateTransitionOnNoAmmo[4]       = "NoAmmo";
   stateTransitionOnTimeout[4]      = "Ready";
   stateWaitForTimeout[4]           = "0";
   stateTimeoutValue[4]             = 0.0;
   stateAllowImageChange[4]         = false;
   stateSequence[4]                 = "Reload";

   // No ammo in the weapon, just idle until something
   // shows up. Play the dry fire sound if the trigger is
   // pulled.
   stateName[5]                     = "NoAmmo";
   stateTransitionGeneric0In[5]     = "Destroyed";
   stateTransitionOnAmmo[5]         = "Reload";
   stateSequence[5]                 = "NoAmmo";
   stateTransitionOnTriggerDown[5]  = "DryFire";

   // No ammo dry fire
   stateName[6]                     = "DryFire";
   stateTransitionGeneric0In[6]     = "Destroyed";
   stateTimeoutValue[6]             = 1.0;
   stateTransitionOnTimeout[6]      = "NoAmmo";
   stateScript[6]                   = "onDryFire";

   // Waiting for the turret to be deployed
   stateName[7]                     = "WaitingDeployment";
   stateTransitionGeneric0In[7]     = "Destroyed";
   stateTransitionOnLoaded[7]       = "Deployed";
   stateSequence[7]                 = "wait_deploy";

   // Turret has been deployed
   stateName[8]                     = "Deployed";
   stateTransitionGeneric0In[8]     = "Destroyed";
   stateTransitionOnTimeout[8]      = "Ready";
   stateWaitForTimeout[8]           = true;
   stateTimeoutValue[8]             = 2.5;   // Same length as turret base's Deploy state
   stateSequence[8]                 = "deploy";

   // Turret has been destroyed
   stateName[9]                     = "Destroyed";
   stateSequence[9]                 = "destroyed";
};
