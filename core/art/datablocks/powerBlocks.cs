//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

datablock staticShapeData(GeneratorStandard)
{
   category = "Assets";
   className = Generator;
   shapeFile = "core/art/shapes/cube.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;

   powerOut = 400;

   maxDamage = 1.00;
   destroyedLevel = 1.00;
   disabledLevel = 0.55;

   maxEnergy = 6.7;
};

//-----------------------------------------------------------------------------

datablock staticShapeData(GeneratorLarge : GeneratorStandard)
{
   powerOut = 800;

   rechargeRate = 0.008;
   maxEnergy = 8;
};

//-----------------------------------------------------------------------------



