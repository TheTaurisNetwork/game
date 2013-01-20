//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//-----------------------------------------------------------------------------

singleton Material( BlankWhite )
{
   diffuseMap[0] = "~/art/textures/white";
   mapTo = "white";
   materialTag0 = "Miscellaneous";
};

singleton Material( Empty )
{
};

singleton Material( WarningMaterial )
{
   diffuseMap[0] = "~/art/textures/warnMat";
   emissive[0] = false;
   translucent = false;
};


singleton CubemapData( WarnMatCubeMap )
{
   cubeFace[0] = "~/art/textures/warnMat";
   cubeFace[1] = "~/art/textures/warnMat";
   cubeFace[2] = "~/art/textures/warnMat";
   cubeFace[3] = "~/art/textures/warnMat";
   cubeFace[4] = "~/art/textures/warnMat";
   cubeFace[5] = "~/art/textures/warnMat";
};


