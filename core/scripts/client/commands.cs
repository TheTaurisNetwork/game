//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

function clientCmdGameEnd(%seq)
{
   // Stop local activity... the game will be destroyed on the server
   sfxStopAll();
}

// Sync the Camera and the EditorGui
function clientCmdSyncEditorGui()
{
   if (isObject(EditorGui))
      EditorGui.syncCameraGui();
}

//-----------------------------------------------------------------------------
//       Tauris Functions
//-----------------------------------------------------------------------------

function clientCmdplayerCanInteract(%bool)
{
  playGui-->interActBitmap.setVisible(%bool);
}

//-----------------------------------------------------------------------------

function clientCmdreceiveItemList(%armour, %prim, %sec, %packs, %nade)
{
   for (%i = 0; %i < getFieldCount(%armour); %i++)
   {
     $armourList[%i] = getField(%armour, %i);
   }
   $armourList = %i;
   for (%i = 0; %i < getFieldCount(%prim); %i++)
   {
     $primaryList[%i] = getField(%prim, %i);
   }
   $primaryList = %i;
   for (%i = 0; %i < getFieldCount(%sec); %i++)
   {
     $secondaryList[%i] = getField(%sec, %i);
   }
   $secondaryList = %i;
   for (%i = 0; %i < getFieldCount(%packs); %i++)
   {
     $packList[%i] = getField(%packs, %i);
   }
   $packList = %i;
   for (%i = 0; %i < getFieldCount(%nade); %i++)
   {
     $nadeList[%i] = getField(%nade, %i);
   }
   $nadeList = %i;

   for (%i = 0; %i < 6; %i++)
     CommandtoServer('receiveInvFav', %i, $pref::loadout[%i]);
}

//-----------------------------------------------------------------------------

function clientCmdpushGui(%gui)
{
  if (isobject(%gui))
    canvas.pushDialog(%gui);
}

//-----------------------------------------------------------------------------

function clientCmdguiDatastream(%gui, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8, %arg9)
{
 if (%gui.awake)
    %gui.getDatastream(%arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8, %arg9, %arg10);
  else
    error("NO GUI : "@%gui@" STREAM : "@ %arg1 SPC %arg2 SPC %arg3 SPC %arg4 SPC %arg5 SPC %arg6 SPC %arg7 SPC %arg8 SPC %arg9 SPC %arg10);
}

//-----------------------------------------------------------------------------







