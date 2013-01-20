//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

datablock staticShapeData(Block)
{
   category = "Constructors";
   className = "Piece";
   shapeFile = "core/art/shapes/cube.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;
};

//-----------------------------------------------------------------------------

datablock staticShapeData(Block1)
{
   category = "Constructors";
   className = "Piece";
   shapeFile = "core/art/shapes/cube1.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;
};

datablock staticShapeData(Block2)
{
   category = "Constructors";
   className = "Piece";
   shapeFile = "core/art/shapes/cube2.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;
};

datablock staticShapeData(Block3)
{
   category = "Constructors";
   className = "Piece";
   shapeFile = "core/art/shapes/cube3.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;
};


datablock staticShapeData(Block4)
{
   category = "Constructors";
   className = "Piece";
   shapeFile = "core/art/shapes/cube4.dae";

   typeMask = $TypeMasks::StaticShapeObjectType;
};

//-----------------------------------------------------------------------------

datablock staticShapeData(DeployedSpin : Block)
{
shapeFile = "core/art/shapes/dmiscf.dts";
};

datablock staticShapeData(DeployedSpin2 : Block)
{
};

datablock staticShapeData(DeployedSpine : DeployedSpin)
{
shapeFile = "core/art/shapes/dmiscf.dts";
};

datablock staticShapeData(DeployedSpine2 : DeployedSpin2)
{
shapeFile = "core/art/shapes/dmiscf.dts";
};
