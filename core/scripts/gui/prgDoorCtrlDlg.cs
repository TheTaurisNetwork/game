//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// prgDoorCtrlDlg
//-----------------------------------------------------------------------------

function prgDoorCtrlDlg::onWake(%this)
{
  commandtoServer('requestDataforGui', %this, %this.getName());
  %this.awake = true;
}

function prgDoorCtrlDlg::onSleep(%this)
{
  %this.awake = "";
}

//-----------------------------------------------------------------------------

function prgDoorCtrlDlg::getDatastream(%this, %arg1, %arg2, %arg3, %arg4)
{
  %this-->ctrlDoor.clear();
  %this-->pwrBranch.clear();

  %this-->pwrBranch.addScheme(0, "0 100 0", "0 100 0", "0 100 0"); // green
  %this-->pwrBranch.addScheme(1, "255 0 0", "0 0 0", "255 0 0"); // red

  for (%i = 0; %i < %arg1; %i++)
  {
     %colour = getWord(%arg1, %i) ? "\c5"@%i : "\c4"@%i;
     %scheme = getWord(%arg1, %i) ? 0 : 1;

     %this-->pwrBranch.add(%colour, %i, %scheme);
  }

  for (%i = 0; %i < getWordCount(%arg2); %i++)
  {
    %this.doorHash[%i, n] = getWord(%arg2, %i);

    %this-->ctrlDoor.add(%this.doorHash[%i, n], %i);

    if (%arg4 == %this.doorHash[%i, n])
      %door = %i;
  }

  if (%door)
    %this-->ctrlDoor.setSelected(%door);
  %this-->pwrBranch.setSelected(%arg3);
}

//-----------------------------------------------------------------------------

function prgDoorCtrlDlg::apply(%this)
{
  %set_Pwrbrnch = %this-->pwrBranch.getSelected();
  %set_door = %this-->ctrlDoor.getTextById(%this-->ctrlDoor.getSelected());

  commandtoServer('receiveUploadDataStream', %this, "prgDoorCtrlDlg", %set_Pwrbrnch, %set_door);
}

//-----------------------------------------------------------------------------



