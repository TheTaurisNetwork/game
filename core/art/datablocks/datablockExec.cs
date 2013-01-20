//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//-----------------------------------------------------------------------------

// Load up all datablocks.  This function is called when
// a server is constructed.

// Set up the Camera's
exec("./camera.cs");

// Common Marker's
exec("./markers.cs");

exec("./defaultparticle.cs");

// LightFlareData and LightAnimData(s)
exec("./lights.cs");

exec("./tools.cs");
exec("./player.cs");
exec("./buildingBlocks.cs");
exec("./powerBlocks.cs");
exec("./assetBlocks.cs");
