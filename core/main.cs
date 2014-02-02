//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//-----------------------------------------------------------------------------

// Constants for referencing video resolution preferences
$WORD::RES_X = 0;
$WORD::RES_Y = 1;
$WORD::FULLSCREEN = 2;
$WORD::BITDEPTH = 3;
$WORD::REFRESH = 4;
$WORD::AA = 5;

//---------------------------------------------------------------------------------------------
// onStart
// Called when the engine is starting up. Initializes this mod.
//---------------------------------------------------------------------------------------------
function onStart()
{
   warn(" % - Initializing Core");

   $isFirstPersonVar = 1;

   if ($platform $= "macos")
      $pref::Video::displayDevice = "OpenGL";
   else
      $pref::Video::displayDevice = "D3D9";
   
   // Initialise stuff.
   // Not Reentrant
   if( $coreInitialized == true )
      return;

   // Core keybindings.
   GlobalActionMap.bind(keyboard, tilde, toggleConsole);
   GlobalActionMap.bind(keyboard, "ctrl p", doScreenShot);
   GlobalActionMap.bindcmd(keyboard, "alt enter", "Canvas.attemptFullscreenToggle();","");
   GlobalActionMap.bindcmd(keyboard, "alt k", "cls();",  "");

   exec("~/art/gui/profiles.cs");

   // Seed the random number generator.
   setRandomSeed();

   // Set up networking.
   setNetPort(0);

   // Initialize the canvas.
   exec("./scripts/client/audio.cs");
   exec("./scripts/client/canvas.cs");
   exec("./scripts/client/cursor.cs");
   exec("./scripts/client/persistenceManagerTest.cs");

   initializeCanvas();

   // Start processing file change events.
   startFileChangeNotifications();

   // Materials and Shaders for rendering various object types
   exec("./art/materials/materialExec.cs");

   // Very basic functions used by everyone.
   exec("./scripts/client/audioEnvironments.cs" );
   exec("./scripts/client/audioDescriptions.cs" );
   exec("./scripts/client/audioStates.cs" );
   exec("./scripts/client/audioAmbiences.cs" );
   exec("./scripts/client/screenshot.cs");
   exec("./scripts/client/scriptDoc.cs");
   //exec("~/scripts/client/keybindings.cs");
   exec("./scripts/client/helperfuncs.cs");
   exec("./scripts/client/commands.cs");
   exec("./scripts/client/metrics.cs");
   exec("./scripts/client/recordings.cs");
   exec("./scripts/client/centerPrint.cs");
   exec("./scripts/client/commonMaterialData.cs");
   exec("./scripts/client/shaders.cs");
   exec("./scripts/client/terrainBlock.cs");
   exec("./scripts/client/imposter.cs");
   exec("./scripts/client/scatterSky.cs");
//   exec("./scripts/client/clouds.cs");
   exec("./scripts/client/messageHud.cs");

   // Initialize all core post effects.
   exec("./scripts/client/postFx.cs");
   initPostEffects();

   // Initialize the post effect manager.
   exec("./scripts/client/postFx/postFXManager.gui");
   exec("./scripts/client/postFx/postFXManager.gui.cs");
   exec("./scripts/client/postFx/postFXManager.gui.settings.cs");
   exec("./scripts/client/postFx/postFXManager.persistance.cs");

   PostFXManager.settingsApplyDefaultPreset();  // Get the default preset settings

   // Set a default cursor.
   Canvas.setCursor(DefaultCursor);

   loadKeybindings();

   $coreInitialized = true;
   
   exec("./scripts/client/client.cs");
   exec("./scripts/server/server.cs");
   exec("./scripts/client/init.cs");
   exec("./scripts/server/init.cs");
   physicsInit();

   sfxStartup();

   initServer();

   if ($Server::Dedicated)
      initDedicated();
   else
      initClient();

   warn("Core Initialized - %");
}

//---------------------------------------------------------------------------------------------
// onExit
// Called when the engine is shutting down. Shutdowns this mod.
//---------------------------------------------------------------------------------------------
function onExit()
{   
   // Stop file change events.
   stopFileChangeNotifications();

   sfxShutdown();

   // Ensure that we are disconnected and/or the server is destroyed.
   // This prevents crashes due to the SceneGraph being deleted before
   // the objects it contains.
   if ($Server::Dedicated)
      destroyServer();
   else
      disconnect();

   // Destroy the physics plugin.
   physicsDestroy();

   echo("Exporting client prefs");
   export("$pref::*", "~/prefs/"@$Pref::Player::Name@".prefs.cs", False);

   echo("Exporting server prefs");
   export("$ServerPref::*", "~/prefs/serverprefs.cs", False);
   BanList::Export("./prefs/banlist.cs");
}

function loadKeybindings()
{
   $keybindCount = 0;
   // Load up the active projects keybinds.
   if(isFunction("setupKeybinds"))
      setupKeybinds();
}

//---------------------------------------------------------------------------------------------
// displayHelp
// Prints the command line options available for this mod.
//---------------------------------------------------------------------------------------------
function displayHelp() {
   // Let the parent do its stuff.
   Parent::displayHelp();

   error("Core options:\n" @
         "  -fullscreen            Starts game in full screen mode\n" @
         "  -windowed              Starts game in windowed mode\n" @
         "  -autoVideo             Auto detect video, but prefers OpenGL\n" @
         "  -openGL                Force OpenGL acceleration\n" @
         "  -directX               Force DirectX acceleration\n" @
         "  -voodoo2               Force Voodoo2 acceleration\n" @
         "  -prefs <configFile>    Exec the config file\n");
}

//---------------------------------------------------------------------------------------------
// parseArgs
// Parses the command line arguments and processes those valid for this mod.
//---------------------------------------------------------------------------------------------
function parseArgs()
{
   // Loop through the arguments.
   for (%i = 1; %i < $Game::argc; %i++)
   {
      %arg = $Game::argv[%i];
      %nextArg = $Game::argv[%i+1];
      %hasNextArg = $Game::argc - %i > 1;
   
      switch$ (%arg)
      {
         case "-fullscreen":
            setFullScreen(true);
            $argUsed[%i]++;

         case "-windowed":
            setFullScreen(false);
            $argUsed[%i]++;

         case "-openGL":
            $pref::Video::displayDevice = "OpenGL";
            $argUsed[%i]++;

         case "-directX":
            $pref::Video::displayDevice = "D3D";
            $argUsed[%i]++;

         case "-voodoo2":
            $pref::Video::displayDevice = "Voodoo2";
            $argUsed[%i]++;

         case "-autoVideo":
            $pref::Video::displayDevice = "";
            $argUsed[%i]++;

         case "-prefs":
            $argUsed[%i]++;
            if (%hasNextArg) {
               exec(%nextArg, true, true);
               $argUsed[%i+1]++;
               %i++;
            }
            else
               error("Error: Missing Command Line argument. Usage: -prefs <path/script.cs>");
      }
   }
}

//---------------------------------------------------------------------------------------------
// dumpKeybindings
// Saves of all keybindings.
//---------------------------------------------------------------------------------------------
function dumpKeybindings()
{
   // Loop through all the binds.
   for (%i = 0; %i < $keybindCount; %i++)
   {
      // If we haven't dealt with this map yet...
      if (isObject($keybindMap[%i]))
      {
         // Save and delete.
         $keybindMap[%i].save(getPrefsPath("bind.cs"), %i == 0 ? false : true);
         $keybindMap[%i].delete();
      }
   }
}

//-----------------------------------------------------------------------------
// reloadMaterials
//-----------------------------------------------------------------------------

function reloadCoreMaterials()
{
   reloadTextures();
   exec("~/art/materials/materialExec.cs");
   reInitMaterials();
}

//};

//activatePackage(CorePackage);

