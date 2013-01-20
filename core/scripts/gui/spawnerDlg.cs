//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// spawnerDlg
//-----------------------------------------------------------------------------

function spawnerDlg::onWake(%this)
{
  %this.awake = true;

  %this-->typeList.add("Asset", 0);
  %this-->typeList.add("Piece", 1);
  %this-->typeList.add("Exterior", 2);

  %this.depType = %this.depType $= "" ? 0 : %this.depType;
  
  %this-->typeList.setSelected(%this.depType);
}


function spawnerDlg::onSleep(%this)
{
  %this.awake = "";
}

//-----------------------------------------------------------------------------

function spawnerDlg::onSelect(%this)
{
  %this.depType = %this-->typeList.getSelected();

  if (%this.depType == 0)
    %this-->collision.setActive(0);
  else if (%this.depType == 2)
  {
    %this-->collision.setValue(1);
    %this-->collision.setActive(0);
  }
  else
  {
    %this-->collision.setValue(1);
    %this-->collision.setActive(1);
  }

  %this-->spawnList.clear();

  for (%i = 0; %i < $deployable[max]; %i++)
  {
    %db = $deployable[%i];
    %class = $deployable[%i, c];
    if (%this.depType $= %class)
      %this-->spawnList.addRow(%i, $depName[%db] TAB $depClass[%db] TAB %db);
  }
}

//-----------------------------------------------------------------------------

function spawnerDlg::spawn(%this)
{
  %text = %this-->spawnList.getRowTextById( %this-->spawnList.getSelectedId() );

  if (%this.depType > 0)
  {
    %col = %this-->collision.getValue();
    %arg = %this.depType == "2" ? "2" : %col;
  }

  %db = getField(%text, 2);
  commandtoServer('receiveUploadDataStream', %this, "spawnerDlg", %db, %arg);

  canvas.popDialog(%this);
}

//-----------------------------------------------------------------------------






