//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//-----------------------------------------------------------------------------

datablock ParticleEmitterNodeData(DefaultEmitterNodeData)
{
   timeMultiple = 1;
};

// Elevator Up

datablock ParticleData(ElevatorUp)
{
   textureName          = "core/art/textures/flare";
   dragCoefficient      = 7.0;
   gravityCoefficient   = -0.4;   // rises slowly
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 2000;
   lifetimeVarianceMS   = 250;
   useInvAlpha          = true;
   spinRandomMin        = -30.0;
   spinRandomMax        = 30.0;

   colors[0] = "0.0 0.0 0.0 0.0";
   colors[1] = "0.0 1.0 0.0 1.0";
   colors[2] = "0.0 1.0 0.0 1.0";
   colors[3] = "0.0 1.0 0.0 1.0";
   
   sizes[0] = "0.0";
   sizes[1] = "0.1";
   sizes[2] = "0.1";
   sizes[3] = "0.1";
};

datablock ParticleEmitterData(ElevatorUpEmitter)
{
   ejectionPeriodMS = 1000;
   periodVarianceMS = 1;

   ejectionVelocity = 3.0;
   velocityVariance = 5.0;

   thetaMin         = 0.0;
   thetaMax         = 30.0;

   particles        = ElevatorUp;
};

//-----------------------------------------------------------------------------

datablock ParticleData(ElevatorDown)
{
   textureName          = "core/art/textures/flare";
   dragCoefficient      = 7.0;
   gravityCoefficient   = 0.3;   // rises slowly
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 2500;
   lifetimeVarianceMS   = 250;
   useInvAlpha          = true;
   spinRandomMin        = -30.0;
   spinRandomMax        = 30.0;

   colors[0] = "0.0 0.0 0.0 0.0";
   colors[1] = "1.0 0.0 0.0 1.0";
   colors[2] = "1.0 0.0 0.0 1.0";
   colors[3] = "1.0 0.0 0.0 1.0";

   sizes[0] = "0.0";
   sizes[1] = "0.1";
   sizes[2] = "0.1";
   sizes[3] = "0.1";

};

datablock ParticleEmitterData(ElevatorDownEmitter)
{
   ejectionPeriodMS = 1000;
   periodVarianceMS = 1;

   ejectionVelocity = 3.0;
   velocityVariance = 5.0;

   thetaMin         = 0.0;
   thetaMax         = 30.0;


   particles        = ElevatorDown;
};

//-----------------------------------------------------------------------------

datablock ParticleEmitterNodeData(ElevatorNodeData)
{
   timeMultiple = 20;
};

//-----------------------------------------------------------------------------


