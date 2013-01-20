//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

//  if ( isObject( swapperMap ) )
//     swapperMap.delete();
//  new ActionMap(swapperMap);
//  swapperMap.bind( keyboard, up, textSwapperDlg_inc );
//  swapperMap.bind( keyboard, down, textSwapperDlg_dec );

//-----------------------------------------------------------------------------
// textSwapperDlg
//-----------------------------------------------------------------------------

function textSwapperDlg::onWake(%this)
{
  if (%this.selected $= "")
    %this.selected = 0;

   singleton Material(standIn)
   {
      mapTo = "matEd_mappedMat";
      diffuseMap[0] = "core/art/textures/unavailable";
   };

  %this.mat[max] = 0;
  for (%i = 0; %i < MaterialSet.getCount(); %i++)
  {
     %m = MaterialSet.getObject(%i);
     if (%m.textureGun)
     {
       %this.mat[%this.mat[max]] = %m;
       %this.mat[max]++;
     }
  }

  %this-->texturePreview.setModel("core/art/shapes/cubePreview.dts");
  %this-->texturePreview.setOrbitDistance(4);

  moveMap.bind( keyboard, "up", textSwapperDlg_inc );
  moveMap.bind( keyboard, "down", textSwapperDlg_dec );
}

function textSwapperDlg::onSleep(%this)
{
  %this-->texturePreview.deleteModel();

  moveMap.unbind( keyboard, "up");
  moveMap.unbind( keyboard, "down");
}

//-----------------------------------------------------------------------------

function textSwapperDlg::applyNewMaterial(%this)
{
  %mat = %this.mat[%this.selected];
  if (!isObject( %mat ))
    return;
  if (%mat.isMemberOfClass( "CustomMaterial" ))
  {
    %this.copyMaterials(missingTexture, standIn);
    error("Texture Swapper : Cannot paint with unsaved materials");
  }
  else
    %this.copyMaterials(%mat, standIn);

  standIn.flush();
  standIn.reload();

  commandtoServer('setToolMode', "textureGun", %mat.getName());
}

//-----------------------------------------------------------------------------

function textSwapperDlg::copyMaterials( %this, %copyFrom, %copyTo)
{
   // Make sure we copy and restore the map to.
   %mapTo = %copyTo.mapTo;
   %copyTo.assignFieldsFrom( %copyFrom );
   %copyTo.mapTo = %mapTo;
}

//-----------------------------------------------------------------------------

function textSwapperDlg_inc(%val)
{
  if (%val)
  {
    %this = textSwapperDlg;
    %this.selected++;
    if (%this.mat[max]-1 < %this.selected)
      %this.selected = 0;

    %this.applyNewMaterial();
  }
}

function textSwapperDlg_dec(%val)
{
  if (%val)
  {
    %this = textSwapperDlg;
    %this.selected--;
    if (%this.selected < 0)
      %this.selected = %this.mat[max]-1;

    %this.applyNewMaterial();
  }
}

//-----------------------------------------------------------------------------



