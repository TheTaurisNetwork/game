//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------

// Variables used by server scripts & code.  The ones marked with (c)
// are accessed from code.  Variables preceeded by Pref:: are server
// preferences and stored automatically in the ServerPrefs.cs file
// in between server sessions.
//
//    (c) Server::ServerType              {SinglePlayer, MultiPlayer}
//    (c) Server::GameType                Unique game name
//    (c) Server::Dedicated               Bool
//    ( ) Server::MissionFile             Mission .mis file name
//    (c) Server::MissionName             DisplayName from .mis file
//    (c) Server::MissionType             Not used
//    (c) Server::PlayerCount             Current player count
//    (c) Server::GuidList                Player GUID (record list?)
//    (c) Server::Status                  Current server status
//
//    (c) Pref::Server::Name              Server Name
//    (c) Pref::Server::Password          Password for client connections
//    ( ) Pref::Server::AdminPassword     Password for client admins
//    (c) Pref::Server::Info              Server description
//    (c) Pref::Server::MaxPlayers        Max allowed players
//    (c) Pref::Server::RegionMask        Registers this mask with master server
//    ( ) Pref::Server::BanTime           Duration of a player ban
//    ( ) Pref::Server::KickBanTime       Duration of a player kick & ban
//    ( ) Pref::Server::MaxChatLen        Max chat message len
//    ( ) Pref::Server::FloodProtectionEnabled Bool

//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------

function initServer()
{
   echo("\n--------- Initializing Taruis: Server Scripts ---------");

   exec("./defaults.cs");
   if (isFile("~/prefs/serverprefs.cs"))
     exec("~/prefs/serverprefs.cs");
   
   $Server::MissionType = "";

   // Server::Status is returned in the Game Info Query and represents the
   // current status of the server. This string sould be very short.
   $Server::Status = "Unknown";

   // Turn on testing/debug script functions
   $Server::TestCheats = false;

   // Specify where the mission files are.
   $Server::MissionFileSpec = "core/levels/*.mis";

   //execServer();
}

function execServer()
{
   // The common module provides the basic server functionality
   // Base server functionality
   exec("./functionLib.cs");

   exec("./audio.cs");
   exec("./message.cs");
   exec("./commands.cs");
   exec("./levelInfo.cs");
   exec("./missionLoad.cs");
   exec("./missionDownload.cs");

   exec("./clientConnection.cs");
   exec("./player.cs");
   exec("./staticShape.cs");
   exec("./item.cs");
   exec("./weapon.cs");
   exec("./kickban.cs");
   exec("./spawn.cs");
   exec("./camera.cs");
   exec("./centerPrint.cs");
   exec("./commands.cs");
   exec("./inventory.cs");
   exec("./TSNodes.cs");
   
   exec("./defaultGame.cs");
   exec("./buildGame.cs");
   exec("./battleGame.cs");
   
   exec("./saveLoad.cs");

   exec("./deployables.cs");
   exec("./console.cs");
   exec("./hyperDrive.cs");
   exec("./station.cs");
   exec("./lighting.cs");
   exec("./elevator.cs");
   exec("./door.cs");
   exec("./turret.cs");

   exec("./shipGroup.cs");
   exec("./shipObject.cs");
   exec("./powerGroup.cs");
   exec("./pieceGroup.cs");
}


//-----------------------------------------------------------------------------

function initDedicated()
{
   enableWinConsole(true);
   echo("\n--------- Starting Dedicated Server ---------");

   // Make sure this variable reflects the correct state.
   $Server::Dedicated = true;

   // The server isn't started unless a mission has been specified.
   if ($missionArg !$= "") {
      createServer("MultiPlayer", $missionArg);
   }
   else
      echo("No mission specified (use -mission filename)");
}

