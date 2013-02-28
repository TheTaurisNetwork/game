// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// ShipObject ScriptObject Commands

//-----------------------------------------------------------------------------



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
  %ship.nameBase = %name;
  %ship.loading = true;
  
  new FileObject("File");
  File.openForWrite("core/ships/"@%name@".cs");
  File.writeLine("%ship = ");
  File.writeObject(%ship);
  File.writeLine("%ship.onLoaded();");
  File.close();
  File.delete();

  %ship.loading = "";

  return true;
}

//-----------------------------------------------------------------------------




