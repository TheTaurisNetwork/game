//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the LurkerWeaponImage is used.
//-----------------------------------------------------------------------------

datablock ItemData(textureGun)
{
   category = "Weapon";

   className = "Weapon";

   // Basic Item properties
   shapeFile = "~/art/shapes/rifle/TP_Lurker.DAE";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   // Dynamic properties defined by the scripts
   PreviewImage = 'textGun.png';
   pickUpName = "Texture Gun";
   description = "Texture Swapper";
   image = textureGunImage;
};


datablock ShapeBaseImageData(textureGunImage)
{
   // Basic Item properties
   shapeFile = "~/art/shapes/rifle/TP_Lurker.DAE";
   shapeFileFP = "~/art/shapes/rifle/FP_Lurker.DAE";
   emap = true;

   imageAnimPrefix = "Rifle";
   imageAnimPrefixFP = "Rifle";

   mountPoint = 0;
   firstPerson = true;
   useEyeNode = true;
   animateOnServer = true;

   correctMuzzleVector = true;

   class = "ToolImage";
   className = "ToolImage";

   useRemainderDT = true;

   minEnergy = 0.0;
   usesEnergy = 1;
   
   // Initial start up state
   stateName[0]                     = "Preactivate";
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNoAmmo[0]       = "NoAmmo";

   // Activating the gun.  Called when the weapon is first
   // mounted and there is ammo.
   stateName[1]                     = "Activate";
   //stateTransitionGeneric0In[1]     = "SprintEnter";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 0.5;
   stateSequence[1]                 = "switch_in";
   //stateSound[1]                    = LurkerSwitchinSound;

   // Ready to fire, just waiting for the trigger
   stateName[2]                     = "Ready";
   //stateTransitionGeneric0In[2]     = "SprintEnter";
   //stateTransitionOnMotion[2]       = "ReadyMotion";
   //tateTransitionOnTimeout[2]      = "ReadyFidget";
   //stateTimeoutValue[2]             = 10;
   //stateWaitForTimeout[2]           = false;
   stateScaleAnimation[2]           = false;
   stateScaleAnimationFP[2]         = false;
   //stateTransitionOnNoAmmo[2]       = "NoAmmo";
   stateTransitionOnTriggerDown[2]  = "Fire";
   stateSequence[2]                 = "idle";

   // Fire the weapon. Calls the fire script which does
   // the actual work.
   stateName[3]                     = "Fire";
   //stateTransitionGeneric0In[5]     = "SprintEnter";
   //stateTransitionOnTimeout[5]      = "NewRound";
   //stateTimeoutValue[5]             = 0.15;
   stateFire[3]                     = true;
   stateRecoil[3]                   = "";
   stateAllowImageChange[3]         = false;
   stateSequence[3]                 = "fire";
   stateScaleAnimation[3]           = false;
   stateSequenceNeverTransition[3]  = true;
   //stateSequenceRandomFlash[3]      = true;        // use muzzle flash sequence
   stateScript[3]                   = "onFire";
   //stateSound[5]                    = LurkerFireSoundList;
   //stateEmitter[5]                  = GunFireSmokeEmitter;
   //stateEmitterTime[5]              = 0.025;


};

