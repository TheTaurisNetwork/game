//-----------------------------------------------------------------------------
// Copyright (c) 2013 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// prgAssetPowerDlg
//-----------------------------------------------------------------------------

function prgAssetPowerDlg::onWake(%this)
{
  commandtoServer('requestDataforGui', %this, %this.getName());
  %this.awake = true;
}

function prgAssetPowerDlg::onSleep(%this)
{
  %this.awake = "";

}

//-----------------------------------------------------------------------------

function prgAssetPowerDlg::getDatastream(%this, %arg1, %arg2, %arg3)
{
  %this-->pwrBranch.clear();
//  %this-->pwrBranch.addScheme(0, "0 100 0", "0 100 0", "0 100 0"); // green
//  %this-->pwrBranch.addScheme(1, "255 0 0", "0 0 0", "255 0 0"); // red

    for (%i = 0; %i < getWordCount(%arg1); %i++)
    {
      %colour = getWord(%arg1, %i) ? "\c5"@%i : "\c4"@%i;
      //%scheme = getWord(%arg1, %i) ? 0 : 1;

      %this-->pwrBranch.add(%colour, %i);//, %scheme);
    }
  %this-->pwrBranch.setSelected(%arg3);
  %this-->pwrNeed.setText("Power Need : " @ %arg2);
}

//-----------------------------------------------------------------------------

function prgAssetPowerDlg::apply(%this)
{
  %pwrbrnch = %this-->pwrBranch.getSelected();
  %pwrbrnch = strReplace(%pwrbrnch, "\c4", "");
  %pwrbrnch = strReplace(%pwrbrnch, "\c5", "");
  commandtoServer('receiveUploadDataStream', %this, "prgAssetPowerDlg", %pwrbrnch);
}

//-----------------------------------------------------------------------------



