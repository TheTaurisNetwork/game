//-----------------------------------------------------------------------------
// Copyright (c) 2013 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// insHelmConsoleDlg
//-----------------------------------------------------------------------------

function insHelmConsoleDlg::onWake(%this)
{
  commandtoServer('requestDataforGui', %this, %this.getName(), 1);
  %this.awake = true;

  %this.liveUpdate();
}

function insHelmConsoleDlg::onSleep(%this)
{
  %this.awake = "";
}

//-----------------------------------------------------------------------------

function insHelmConsoleDlg::liveUpdate(%this)
{
  if (%this.awake $= "")
    return;
  commandtoServer('requestDataforGui', %this, %this.getName());
  %this.schedule(1000, liveUpdate);
}

//-----------------------------------------------------------------------------

function insHelmConsoleDlg::onBookmarkSelect(%this)
{
  %text = %this.bkmkList[%this-->bkmkMenu.getSelected()] < %this-->maxDist.getValue() ? "\c5Success probable" : "\c4Insufficient Energy";
  %this-->bkmkOk.setText(%text);
}

//-----------------------------------------------------------------------------

function insHelmConsoleDlg::BJEdit(%this, %vec)
{
  %x = 0;
  %y = 0;
  %z = 0;

  %x = %this-->BJXEdit.getValue();
  %y = %this-->BJYEdit.getValue();
  %z = %this-->BJZEdit.getValue();

  %offset = %x SPC %y SPC %z;
  if (vectorLen(%offset) > 0)
  {
    %text = vectorDist("0 0 0", %offset) < %this-->maxDist.getValue() ? "\c5Sufficient" : "\c4Insufficient";
    %this-->BJOk.setText(%text);
  }
}

//-----------------------------------------------------------------------------

function insHelmConsoleDlg::getDatastream(%this, %func, %arg1, %arg2, %arg3, %arg4, %arg5)
{
  %this-->energyBar.setValue(%arg1);
  %this-->maxDist.setText(%arg2);

  if (%arg1 == 0)
  {
    %this-->bjButton.setActive(0);
    %this-->bkmkButton.setActive(0);
  }
  else
  {
    %this-->bjButton.setActive(1);
    %this-->bkmkButton.setActive(1);
  }

  %this-->BJOk.setText("");

  %x = %this-->BJXEdit.getValue();
  %y = %this-->BJYEdit.getValue();
  %z = %this-->BJZEdit.getValue();

  %offset = %x SPC %y SPC %z;
  if (vectorLen(%offset) > 0)
  {
    %text = vectorDist("0 0 0", %offset) < %this-->maxDist.getValue() ? "\c5Sufficient" : "\c4Insufficient";
    %this-->BJOk.setText(%text);
  }

  if (%func)
  {
    %this-->bkmkOk.setText("");
    %this-->bkmkMenu.clear();

    for (%i = 0; %i < getWordCount(%arg3); %i++)
    {
      %this.bkmkList[%i, n] = getField(%arg3, %i);
      %this.bkmkList[%i] = getWord(%arg4, %i);
      
      %this-->bkmkMenu.add(%this.bkmkList[%i, n], %i);
    }
    
    %this-->driveErrCode.setText( %arg5 $= "" ? "" : "Drive Error : \c4" @ %arg5 );
  }
}

//-----------------------------------------------------------------------------

function insHelmConsoleDlg::jump(%this, %var)
{
  if (%var)
  {
    %x = %this-->BJXEdit.getValue();
    %y = %this-->BJYEdit.getValue();
    %z = %this-->BJZEdit.getValue();

    %arg1 = %x SPC %y SPC %z;
//    if (vectorDist("0 0 0", %offset) > %this-->maxDist.getText())
  }
  else if (!%var)
    %arg1 = %this-->bkmkMenu.getSelected();

  commandtoServer('receiveUploadDataStream', %this, "insHelmConsoleDlg", %var, %arg1);
}

//-----------------------------------------------------------------------------

function insHelmConsoleDlg::openBkMk(%this)
{
  %this-->bkmkWindow.setVisible(1);
  %this-->bkmkName.setText("New Bookmark");
}

//-----------------------------------------------------------------------------

function insHelmConsoleDlg::closebkmk(%this)
{
  %this-->bkmkWindow.setVisible(0);
}

//-----------------------------------------------------------------------------


function insHelmConsoleDlg::addBookmark(%this)
{
  %name = %this-->bkmkName.getValue();
  if (%name $= "")
    return;
  commandtoServer('receiveUploadDataStream', %this, "insHelmConsoleDlg", 2, %name);
  %this.closebkmk();
}

//-----------------------------------------------------------------------------


