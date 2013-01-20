// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// ShipObject ScriptObject Commands

//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//       Loading
//-----------------------------------------------------------------------------

function loadingGroup::onAdd(%this)
{
 error("LOADING");

}

//-----------------------------------------------------------------------------

function loadingGroup::load(%this)
{
 %ship = %this.getObject(0);
 if (%ship.loading $= "1")
 {
   game.loadShip(%ship);
 
   %ship.loading = "";
 }
 %this.delete();
}

//-----------------------------------------------------------------------------
//       Loading
//-----------------------------------------------------------------------------
function loadAv() { loadClientShip(user.client, "avenger"); }
function loadClientShip(%client, %name)
{
  if (game.prepareLoad(%client, %name))
    warn("Ship "@%name@" loaded.");
}

//-----------------------------------------------------------------------------
//       Saving
//-----------------------------------------------------------------------------

function saveClientShip(%client, %name)
{
  if (game.class $= "BuildGame")
  {
    if (game.saveClientShip(%client, %name))
      warn("Ship "@%name@" saved.");
    else
      error("ERROR DURING SAVE. SHIP NOT SAVED");
  }
  else
    return error("WRONG GAMETYPE. SHIP NOT SAVED");
}

//-----------------------------------------------------------------------------

function saveShip(%ship, %name)
{
  new SimGroup(tempGroup)
  {
    class = "loadingGroup";
  };
  tempGroup.add( %ship );
  
  %GO = %ship.getGameObject();
  missionCleanup.add(%GO);

  %ship.nameBase = %name;
  %ship.loading = true;
  
  new FileObject("File");
  File.openForWrite("core/ships/"@%name@".cs");
  File.writeObject(tempGroup);
  File.writeLine("tempGroup.load();");
  File.close();
  File.delete();

  %ship.loading = "";
  %ship.add(%GO);
  buildGroup.add(%ship);

  tempGroup.delete();

  return true;
}

//-----------------------------------------------------------------------------




