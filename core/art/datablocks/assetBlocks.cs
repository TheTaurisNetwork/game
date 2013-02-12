//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
//     Control types
//-----------------------------------------------------------------------------

datablock staticShapeData(Console)
{
   category = "Assets";
   className = Console;
   shapeFile = "core/art/shapes/cube.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;

   needsPower = true;
   pwrRequired = 30;

   maxDamage = 1.00;
   destroyedLevel = 1.00;
   disabledLevel = 0.50;

   rechargeRate = 0.01;
   maxEnergy = 3;
};

//-----------------------------------------------------------------------------

datablock staticShapeData(HelmConsole : Console)
{
};

//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
//     Hyper Drive types
//-----------------------------------------------------------------------------

datablock staticShapeData(HyperDriveMkI)
{
   category = "Assets";
   className = Drive;
   shapeFile = "core/art/shapes/cube.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;

   needsPower = true;
   pwrRequired = 400;

   maxDamage = 1.00;
   destroyedLevel = 1.00;
   disabledLevel = 0.30;

   rechargeRate = 0.1;
   maxEnergy = 200;
   //jumpSphere = 200;
   driveModifier = 513;
};

//-----------------------------------------------------------------------------

datablock staticShapeData(HyperDriveMkII : HyperDriveMkI)
{
   pwrRequired = 600;

   rechargeRate = 0.6;
   maxEnergy = 200;
  // jumpSphere = 600;
   driveModifier = 583;
};

//-----------------------------------------------------------------------------

datablock staticShapeData(HyperDriveMkIII : HyperDriveMkI)
{
   pwrRequired = 800;

   rechargeRate = 0.8;
   maxEnergy = 200;
  // jumpSphere = 1000;
   driveModifier = 713;
};

//-----------------------------------------------------------------------------
//     Lights
//-----------------------------------------------------------------------------

datablock staticShapeData(Lights)
{
   category = "Assets";
   className = Lights;
   shapeFile = "core/art/shapes/cube.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;

   needsPower = true;
   pwrRequired = 2;
};

//-----------------------------------------------------------------------------
//     Asset
//-----------------------------------------------------------------------------

datablock staticShapeData(InventoryStation)
{
   category = "Assets";
   className = Station;
   shapeFile = "core/art/shapes/cube.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;

   needsPower = true;
   pwrRequired = 50;
};

//-----------------------------------------------------------------------------

datablock staticShapeData(upElevator)
{
   category = "Assets";
   className = Elevator;
   shapeFile = "core/art/shapes/cube.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;

   needsPower = true;
   pwrRequired = 3;
   mode = 1;

};

//-----------------------------------------------------------------------------

datablock staticShapeData(downElevator)
{
   category = "Assets";
   className = Elevator;
   shapeFile = "core/art/shapes/cube.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;

   needsPower = true;
   pwrRequired = 3;
   mode = 0;
   
};

//-----------------------------------------------------------------------------



