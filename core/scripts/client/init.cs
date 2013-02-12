//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Variables used by client scripts & code.  The ones marked with (c)
// are accessed from code.  Variables preceeded by Pref:: are client
// preferences and stored automatically in the ~/client/prefs.cs file
// in between sessions.
//
//    (c) Client::MissionFile             Mission file name
//    ( ) Client::Password                Password for server join

//    (?) Pref::Player::CurrentFOV
//    (?) Pref::Player::DefaultFov
//    ( ) Pref::Input::KeyboardTurnSpeed

//    (c) pref::Master[n]                 List of master servers
//    (c) pref::Net::RegionMask
//    (c) pref::Client::ServerFavoriteCount
//    (c) pref::Client::ServerFavorite[FavoriteCount]
//    .. Many more prefs... need to finish this off

// Moves, not finished with this either...
//    (c) firstPerson
//    $mv*Action...

//-----------------------------------------------------------------------------
// These are variables used to control the shell scripts and
// can be overriden by mods:
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
function initClient()
{
   echo("\n--------- Initializing Tauris: Client Scripts ---------");

   exec("./defaults.cs");
   if (isFile("~/prefs/"@$Pref::Player::Name@".prefs.cs"))
     exec("~/prefs/"@$Pref::Player::Name@".prefs.cs");

   // Make sure this variable reflects the correct state.
   $Server::Dedicated = false;

   // Game information used to query the master server
   $Client::GameTypeQuery = "Tauris";
   $Client::MissionTypeQuery = "Any";

   // Base client functionality
   exec("./messageCallbacks.cs" );
   exec("./message.cs" );
   exec("./mission.cs" );
   exec("./missionDownload.cs" );
   exec("./actionMap.cs" );
   exec("./renderManager.cs" );
   exec("./lighting.cs" );
   exec("./serverConnection.cs");

   initRenderManager();
   initLightingSystems();

   // Use our prefs to configure our Canvas/Window
   configureCanvas();

   // Load up the Game GUI
   exec("~/art/gui/remapDlg.gui");
   exec("~/art/gui/console.gui");
   exec("~/art/gui/consoleVarDlg.gui");
   exec("~/art/gui/netGraphGui.gui");
   exec("~/art/gui/chatHud.gui");
   exec("~/art/gui/playGui.gui");
   exec("~/art/gui/StartupGui.gui");
   exec("~/art/gui/MainMenuGui.gui");
   exec("~/art/gui/chooseLevelDlg.gui");
   exec("~/art/gui/optionsDlg.gui");
   exec("~/art/gui/loadingGui.gui");
   exec("~/art/gui/prgConsoleDlg.gui");
   exec("~/art/gui/insHelmConsoleDlg.gui");
   exec("~/art/gui/prgPowerDlg.gui");
   exec("~/art/gui/spawnerDlg.gui");
   exec("~/art/gui/insGeneratorDlg.gui");
   exec("~/art/gui/textSwapperDlg.gui");
   exec("~/art/gui/prgAssetPowerDlg.gui");
   exec("~/art/gui/inventoryDlg.gui");
   exec("~/art/gui/shipDataDlg.gui");

   // Gui scripts
   exec("~/scripts/gui/help.cs");

   execGui();

   // Default player key bindings
   exec("./default.bind.cs");

   if (isFile("~/prefs/"@$Pref::Player::Name@".config.cs"))
      exec("~/prefs/"@$Pref::Player::Name@".config.cs");

   // Really shouldn't be starting the networking unless we are
   // going to connect to a remote server, or host a multi-player
   // game.
   setNetPort(0);

   // Copy saved script prefs into C++ code.
   setDefaultFov( $pref::Player::defaultFov );
   setZoomSpeed( $pref::Player::zoomSpeed );

   if( isFile( "./audioData.cs" ) )
      exec( "./audioData.cs" );

   // Start up the main menu... this is separated out into a
   // method for easier mod override.

   if ($startWorldEditor || $startGUIEditor) {
      // Editor GUI's will start up in the primary main.cs once
      // engine is initialized.
      return;
   }

   // Connect to server if requested.
   if ($JoinGameAddress !$= "") {
      // If we are instantly connecting to an address, load the
      // loading GUI then attempt the connect.
      loadLoadingGui();
      connect($JoinGameAddress, "", $Pref::Player::Name);
   }
   else {
      // Otherwise go to the splash screen.
      Canvas.setCursor("DefaultCursor");
      loadStartup();
      //loadMainMenu();
   }   
}

//-----------------------------------------------------------------------------

function execGui()
{
   exec("~/scripts/gui/guiTreeViewCtrl.cs");
   exec("~/scripts/gui/messageBoxes/messageBox.ed.cs");
   exec("~/scripts/gui/chatHud.cs");
   exec("~/scripts/gui/playGui.cs");
   exec("~/scripts/gui/startupGui.cs");
   exec("~/scripts/gui/chooseLevelDlg.cs");
   exec("~/scripts/gui/optionsDlg.cs");
   exec("~/scripts/gui/loadingGui.cs");
   exec("~/scripts/gui/prgConsoleDlg.cs");
   exec("~/scripts/gui/insHelmConsoleDlg.cs");
   exec("~/scripts/gui/prgPowerDlg.cs");
   exec("~/scripts/gui/spawnerDlg.cs");
   exec("~/scripts/gui/insGeneratorDlg.cs");
   exec("~/scripts/gui/textSwapperDlg.cs");
   exec("~/scripts/gui/prgAssetPowerDlg.cs");
   exec("~/scripts/gui/inventoryDlg.cs");
   exec("~/scripts/gui/shipDataDlg.cs");
}

//-----------------------------------------------------------------------------

function loadMainMenu()
{
   // Startup the client with the Main menu...
//   if (isObject( MainMenuGui ))
   Canvas.setContent( MainMenuGui );
   Canvas.setCursor("DefaultCursor");
   // first check if we have a level file to load
   if ($levelToLoad !$= "")
   {
      %levelFile = "levels/";
      %ext = getSubStr($levelToLoad, strlen($levelToLoad) - 3, 3);
      if(%ext !$= "mis")
         %levelFile = %levelFile @ $levelToLoad @ ".mis";
      else
         %levelFile = %levelFile @ $levelToLoad;

      // Clear out the $levelToLoad so we don't attempt to load the level again
      // later on.
      $levelToLoad = "";
      
      // let's make sure the file exists
      %file = findFirstFile(%levelFile);

      if(%file !$= "")
         createAndConnectToLocalServer( "SinglePlayer", %file );
   }
}

function loadLoadingGui()
{
   Canvas.setContent("LoadingGui");
   LoadingProgress.setValue(1);

   LoadingProgressTxt.setValue("WAITING FOR SERVER");

   Canvas.repaint();
}
