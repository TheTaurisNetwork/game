//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// insGeneratorDlg
//-----------------------------------------------------------------------------

function insGeneratorDlg::onWake(%this)
{
  commandtoServer('requestDataforGui', %this, %this.getName());
  %this.awake = true;
}

function insGeneratorDlg::onSleep(%this)
{
  %this.awake = "";

}

//-----------------------------------------------------------------------------

function insGeneratorDlg::getDatastream(%this, %arg1, %arg2, %arg3)
{
  %this-->GenName.setText("Handle : " @ %arg1);
  %this-->GenPower.setText("Power Out : " @ %arg2);
  %this-->GenState.setText("State : " @ %arg3);
}

//-----------------------------------------------------------------------------

function insGeneratorDlg::toggle(%this)
{
  commandtoServer('receiveUploadDataStream', %this, %this.getName());
}

//-----------------------------------------------------------------------------



