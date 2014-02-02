
datablock FlyingVehicleData(hvehicle)
{

   shapeFile = "core/art/shapes/toonsGlider/toonGlider.dts";
   computeCRC = true;

   drag    = 0.01;
   density = 1.0;

   cameraMaxDist = 15;
   cameraOffset = 2.5;
   cameraLag = 0.9;

   minDrag = 30;           // Linear Drag (eventually slows you down when not thrusting...constant drag)
   rotationalDrag = 900;        // Anguler Drag (dampens the drift after you stop moving the mouse...also tumble drag)

   maxAutoSpeed = 15;       // Autostabilizer kicks in when less than this speed. (meters/second)
   autoAngularForce = 400;       // Angular stabilizer force (this force levels you out when autostabilizer kicks in)
   autoLinearForce = 300;        // Linear stabilzer force (this slows you down when autostabilizer kicks in)
   autoInputDamping = 0.95;      // Dampen control input so you don't` whack out at very slow speeds

   // Maneuvering
   maxSteeringAngle = 5;    // Max radiens you can rotate the wheel. Smaller number is more maneuverable.
   horizontalSurfaceForce = 6;   // Horizontal center "wing" (provides "bite" into the wind for climbing/diving and turning)
   verticalSurfaceForce = 4;     // Vertical center "wing" (controls side slip. lower numbers make MORE slide.)
   maneuveringForce = 3000;      // Horizontal jets (W,S,D,A key thrust)
   steeringForce = 1200;         // Steering jets (force applied when you move the mouse)
   steeringRollForce = 400;      // Steering jets (how much you heel over when you turn)
   rollForce = 4;                // Auto-roll (self-correction to right you after you roll/invert)
   hoverHeight = 5;        // Height off the ground at rest
   createHoverHeight = 3;  // Height off the ground when created
   maxForwardSpeed = 100;  // speed in which forward thrust force is no longer applied (meters/second)

   // Turbo Jet
   jetForce = 2000;      // Afterburner thrust (this is in addition to normal thrust)
   minJetEnergy = 28;     // Afterburner can't be used if below this threshhold.
   jetEnergyDrain = 2.8;       // Energy use of the afterburners (low number is less drain...can be fractional)                                                                                                                                                                                                                                                                                          // Auto stabilize speed
   vertThrustMultiple = 3.0;

   // Rigid body
   mass = 150;        // Mass of the vehicle
   bodyFriction = 0;     // Don't mess with this.
   bodyRestitution = 0.5;   // When you hit the ground, how much you rebound. (between 0 and 1)
   minRollSpeed = 0;     // Don't mess with this.
   softImpactSpeed = 14;       // Sound hooks. This is the soft hit.
   hardImpactSpeed = 25;    // Sound hooks. This is the hard hit.

   // Ground Impact Damage (uses DamageType::Ground)
   minImpactSpeed = 10;      // If hit ground at speed above this then it's an impact. Meters/second
   speedDamageScale = 0.06;

   // Object Impact Damage (uses DamageType::Impact)
   collDamageThresholdVel = 23.0;
   collDamageMultiplier   = 0.02;

   //
   minTrailSpeed = 15;      // The speed your contrail shows up at.

   //
   softSplashSoundVelocity = 10.0;
   mediumSplashSoundVelocity = 15.0;
   hardSplashSoundVelocity = 20.0;
   exitSplashSoundVelocity = 10.0;
   isSelectable = true;
};

function FlyingVehicleData::onAdd(%this,%obj,%data)
{
    MissionGroup.add(%obj);
    %obj.mountable = true;
    %obj.isSelectable = true;
}



