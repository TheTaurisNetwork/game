//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// InventoryDlg
//-----------------------------------------------------------------------------

function InventoryDlg::onWake(%this)
{
  %this.awake = true;
  %this.selectedFav = "";
  
  %this-->armormenu.clear();
  for (%i = 0; %i < $armourList; %i++)
    %this-->armormenu.add($armourList[%i], %i);
  %this-->primmenu.clear();
  for (%i = 0; %i < $primaryList; %i++)
    %this-->primmenu.add($primaryList[%i], %i);
  %this-->secondmenu.clear();
  for (%i = 0; %i < $secondaryList; %i++)
    %this-->secondmenu.add($secondaryList[%i], %i);
  %this-->packmenu.clear();
  for (%i = 0; %i < $packList; %i++)
    %this-->packmenu.add($packList[%i], %i);
  %this-->miscmenu.clear();
  for (%i = 0; %i < $nadeList; %i++)
    %this-->miscmenu.add($nadeList[%i], %i);

  %this-->but0.text = strupr(getField($pref::loadout[0], 0));
  %this-->but1.text = strupr(getField($pref::loadout[1], 0));
  %this-->but2.text = strupr(getField($pref::loadout[2], 0));
  %this-->but3.text = strupr(getField($pref::loadout[3], 0));
  %this-->but4.text = strupr(getField($pref::loadout[4], 0));
  %this-->but5.text = strupr(getField($pref::loadout[5], 0));

  %this.loadFav( %this.selectedFav );
}

function InventoryDlg::onSleep(%this)
{
  %this.awake = "";
}

//-----------------------------------------------------------------------------

function InventoryDlg::getDatastream(%this, %index, %list)
{
  error("getDatastream" SPC %index SPC %list);
  $pref::loadout[%index] = %list;


  if (%this.saveclose)
  {
    %this.saveclose = "";
    Canvas.popDialog(%this);
  }
}

//-----------------------------------------------------------------------------

function InventoryDlg::loadFav(%this, %f)
{
  if (%f $= "")
    %f = 0;
  else
    %this.save(%f);

  %this.selectedFav = %f;

  %fav = $pref::loadout[%f];

  %this-->favName.setText(getField(%fav, 0));

  %i = %this-->armormenu.findText(getField(%fav, 1));
  %this-->armormenu.setSelected(%i);

  %i = %this-->primmenu.findText(getField(%fav, 2));
  %this-->primmenu.setSelected(%i);

  %i = %this-->secondmenu.findText(getField(%fav, 3));
  %this-->secondmenu.setSelected(%i);

  %i = %this-->packmenu.findText(getField(%fav, 4));
  %this-->packmenu.setSelected(%i);

  %i = %this-->miscmenu.findText(getField(%fav, 5));
  %this-->miscmenu.setSelected(%i);
}

//-----------------------------------------------------------------------------

function InventoryDlg::save(%this)
{
  %f = %this.selectedFav;

  %name = %this-->favName.getValue();
  %armour = %this-->armormenu.getTextById(%this-->armormenu.getSelected());
  %prim = %this-->primmenu.getTextById(%this-->primmenu.getSelected());
  %sec = %this-->secondmenu.getTextById(%this-->secondmenu.getSelected());
  %pack = %this-->packmenu.getTextById(%this-->packmenu.getSelected());
  %nade = %this-->miscmenu.getTextById(%this-->miscmenu.getSelected());

  $pref::loadout[%f] = %name TAB %armour TAB %prim TAB %sec TAB %pack TAB %nade;

  CommandtoServer('receiveInvFav', %f, $pref::loadout[%f]);
}

//-----------------------------------------------------------------------------

function InventoryDlg::saveclose(%this)
{
  %this.save();

  %this.saveclose = true;
}


//-----------------------------------------------------------------------------


