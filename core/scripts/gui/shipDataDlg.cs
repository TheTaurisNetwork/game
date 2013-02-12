//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// shipDataDlg
//-----------------------------------------------------------------------------

function shipDataDlg::onWake(%this)
{
  %this.awake = true;

  commandtoServer('requestDataforGui', %this, %this.getName());
}

function shipDataDlg::onSleep(%this)
{
  %this.awake = "";
}

//-----------------------------------------------------------------------------

function shipDataDlg::getDatastream(%this, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7)
{
  %this-->lightpwrBranch.clear();
  %this-->gravitypwrBranch.clear();
  %this-->gepwrBranch.clear();

  for (%i = 0; %i < getWordCount(%arg1); %i++)
  {
    %state = getWord(%arg1, %i);

    %this-->lightpwrBranch.add(getWord(%state, %i) == 1 ? "\c5"@%i : "\c4"@%i, %i);
    %this-->gravitypwrBranch.add(getWord(%state, %i) == 1 ? "\c5"@%i : "\c4"@%i, %i);
    %this-->gepwrBranch.add(getWord(%state, %i) == 1 ? "\c5"@%i : "\c4"@%i, %i);

    %this-->lightpwrBranch.setSelected(%arg4);
    %this-->gravitypwrBranch.setSelected(%arg5);
    %this-->gepwrBranch.setSelected(%arg6);
  }

  %this-->lightpwrNeed.setText("Pwr need :" @ %arg2);
  %this-->gravitypwrNeed.setText("Pwr need :" @ %arg3);
  %this-->gepwrNeed.setText("Pwr need :" @ %arg7);

}

//-----------------------------------------------------------------------------

function shipDataDlg::toggle(%this, %mode)
{

  commandtoServer('receiveUploadDataStream', %this, "shipDataDlg", 0, %mode);
}

//-----------------------------------------------------------------------------

function shipDataDlg::apply(%this)
{
  %l = %this-->lightpwrBranch.getSelected();
  %g = %this-->gravitypwrBranch.getSelected();
  %ge = %this-->gepwrBranch.getSelected();
  
  commandtoServer('receiveUploadDataStream', %this, "shipDataDlg", 1, %l, %g, %ge);
}

//-----------------------------------------------------------------------------



