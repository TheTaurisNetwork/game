//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// prgConsoleDlg
//-----------------------------------------------------------------------------

function prgPowerDlg::onWake(%this)
{
  %this.awake = true;
  commandtoServer('requestDataforGui', %this, %this.getName());
}

function prgPowerDlg::onSleep(%this)
{
 %this.awake = "";
}

//-----------------------------------------------------------------------------

function prgPowerDlg::getDatastream(%this, %power, %name, %usedPwr, %branches, %state)
{
  prgPowerList.clear();
  %this-->branchList.clear();

  for (%i = 0; %i < getWordCount(%name); %i++)
  {
    %this.genHash[%i, p] = getWord(%power, %i);
    %this.genHash[%i, n] = getWord(%name, %i);

    %totPwr += %this.genHash[%i, p];

    prgPowerList.addRow(%i, %this.genHash[%i, n] TAB %this.genHash[%i, p]);
  }

  for (%i = 0; %i < %branches; %i++)
    %this-->branchList.addRow(%i, getWord(%state, %i) == 1 ? "\c5"@%i : "\c4"@%i);

  %this-->usedPwr.text = %usedPwr;
  %this-->totPwr.text = "/" SPC %totPwr;

  prgPowerList.setSelectedById(0);
}

//-----------------------------------------------------------------------------

function prgPowerList::onSelect(%this, %id, %name)
{
  prgPowerDlg-->handleEdit.setText(prgPowerDlg.genHash[%id, n]);
}

//-----------------------------------------------------------------------------

function prgPowerDlg::addBranch(%this)
{
  commandtoServer('receiveUploadDataStream', %this, "prgPowerDlg", 1);
}

//-----------------------------------------------------------------------------

function prgPowerDlg::toggleBranch(%this)
{
  commandtoServer('receiveUploadDataStream', %this, "prgPowerDlg", 2, %this-->branchList.getSelectedId());
}

//-----------------------------------------------------------------------------

function prgPowerDlg::apply(%this)
{
  %i = prgPowerList.getSelectedId();
  if (%i !$= "")
    commandtoServer('receiveUploadDataStream', %this, "prgPowerDlg", 0, %this.genHash[%i, n], %this-->handleEdit.getText());
  else
    prgPowerDlg-->handleEdit.setText("");
}

//-----------------------------------------------------------------------------



