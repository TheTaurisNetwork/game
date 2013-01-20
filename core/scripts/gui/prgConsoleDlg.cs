//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// prgConsoleDlg
//-----------------------------------------------------------------------------

function prgConsoleDlg::onWake(%this)
{
  commandtoServer('requestDataforGui', %this, %this.getName());
  %this.awake = true;
}

function prgConsoleDlg::onSleep(%this)
{
  %this.awake = "";
}

//-----------------------------------------------------------------------------

function prgConsoleDlg::onBranchSelect(%this)
{
  %this-->ctrlBranch.selected = true;

  %this-->ctrlGen.setNoneSelected();
  %this-->ctrlGen.selected = false;
}

function prgConsoleDlg::onGenSelect(%this)
{
  %this-->ctrlGen.selected = true;

  %this-->ctrlBranch.setNoneSelected();
  %this-->ctrlBranch.selected = false;
}

//-----------------------------------------------------------------------------

function prgConsoleDlg::getDatastream(%this, %func, %arg1, %arg2, %arg3, %arg4, %arg5)
{
  if (%func == 0)
  {
    %this-->ctrlGen.clear();
    %this-->ctrlBranch.clear();
    %this-->pwrBranch.clear();
    %this-->ctrlState.clear();

    %this-->pwrBranch.addScheme(0, "0 100 0", "0 100 0", "0 100 0"); // green
    %this-->pwrBranch.addScheme(1, "255 0 0", "0 0 0", "255 0 0"); // red

    %this-->ctrlBranch.addScheme(0, "0 100 0", "0 100 0", "0 100 0"); // green
    %this-->ctrlBranch.addScheme(1, "255 0 0", "0 0 0", "255 0 0"); // red

    for (%i = 0; %i < getWordCount(%arg1); %i++)
    {
      %this.genHash[%i, n] = getWord(%arg1, %i);

      %this-->ctrlGen.add(%this.genHash[%i, n], %i);
    }

    for (%i = 0; %i < %arg3; %i++)
    {
      %colour = getWord(%arg2, %i) ? "\c5"@%i : "\c4"@%i;
      %scheme = getWord(%arg2, %i) ? 0 : 1;

      %this-->pwrBranch.add(%colour, %i, %scheme);
      %this-->ctrlBranch.add(%colour, %i, %scheme);
    }
    
    %this-->ctrlState.add("Off", 0);
    %this-->ctrlState.add("On", 1);

    commandtoServer('requestDataforGui', %this, %this.getName(), 1);
  }
  else if (%func == 1)
  {
    if (%arg1 > -1 && %arg1 !$= "")
      %this-->pwrBranch.setSelected( %arg1);

    if (%arg2 !$= "")
    {
      for (%i = 0; %this.genHash[%i, n] !$= ""; %i++)
        if (%this.genHash[%i, n] $= %arg2)
          %this-->ctrlGen.setSelected( %i, true );
    }
    else if (%arg3 > -1 && %arg3 !$= "")
      %this-->ctrlBranch.setSelected( %arg3, true );

    %this-->ctrlState.setSelected( %arg4 !$= "" ? %arg4 : 0 );

    %this-->latch.setValue( %arg5 );
  }
}

//-----------------------------------------------------------------------------

function prgConsoleDlg::apply(%this)
{
  %set_Pwrbrnch = %this-->pwrBranch.getSelected();
  %set_gen = %this-->ctrlGen.getTextById(%this-->ctrlGen.getSelected());
  %set_brnch = %this-->ctrlBranch.getSelected();
  %set_toggle = %this-->ctrlState.getSelected();
  %set_latch = %this-->latch.getValue();

  if (!%this-->ctrlGen.selected)
    %set_gen = -1;
  if (!%this-->ctrlBranch.selected)
  {
    %set_brnch = -1;
    %set_gen = strReplace(%set_gen, "\c4", "");
    %set_gen = strReplace(%set_gen, "\c5", "");
  }

  commandtoServer('receiveUploadDataStream', %this, "prgConsoleDlg", %set_Pwrbrnch, %set_gen, %set_brnch, %set_toggle, %set_latch);
  //canvas.popDialog(%this);
}

//-----------------------------------------------------------------------------



