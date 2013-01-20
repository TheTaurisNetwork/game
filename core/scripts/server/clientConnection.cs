//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//-----------------------------------------------------------------------------

// ClientConnection class functions go in here


//-----------------------------------------------------------------------------
// This script function is called before a client connection
// is accepted.  Returning "" will accept the connection,
// anything else will be sent back as an error to the client.
// All the connect args are passed also to onConnectRequest
//
function GameConnection::onConnectRequest( %client, %netAddress, %name )
{
   echo("Connect request from: " @ %netAddress);
   if($Server::PlayerCount >= $ServerPref::MaxPlayers)
      return "CR_SERVERFULL";
   return "";
}

//-----------------------------------------------------------------------------
// This script function is the first called on a client accept
//
function GameConnection::onConnect( %client, %name )
{
   // Send down the connection error info, the client is
   // responsible for displaying this message if a connection
   // error occures.
   messageClient(%client,'MsgConnectionError',"",$ServerPref::ConnectionError);

   // Send mission information to the client
   sendLoadInfoToClient( %client );

   // Simulated client lag for testing...
   // %client.setSimulatedNetParams(0.1, 30);

   // Get the client's unique id:
   // %authInfo = %client.getAuthInfo();
   // %client.guid = getField( %authInfo, 3 );
   %client.guid = 0;
   addToServerGuidList( %client.guid );
   
   // Set admin status
   if (%client.getAddress() $= "local") {
      %client.isAdmin = true;
      %client.isSuperAdmin = true;
   }
   else {
      %client.isAdmin = false;
      %client.isSuperAdmin = false;
   }

   // Save client preferences on the connection object for later use.
   %client.gender = "Male";
   %client.armor = "Light";
   %client.race = "Human";
   %client.skin = addTaggedString( "base" );
   %client.setPlayerName(%name);
   %client.team = "";
   %client.score = 0;

   // 
   echo("CADD: " @ %client @ " " @ %client.getAddress());

   // Inform the client of all the other clients
   %count = ClientGroup.getCount();
   for (%cl = 0; %cl < %count; %cl++) {
      %other = ClientGroup.getObject(%cl);
      if ((%other != %client)) {
         // These should be "silent" versions of these messages...
         messageClient(%client, 'MsgClientJoin', "", 
               %other.playerName,
               %other,
               %other.sendGuid,
               %other.team,
               %other.score, 
               %other.isAIControlled(),
               %other.isAdmin, 
               %other.isSuperAdmin);
      }
   }

   // Inform the client we've joined up
   messageClient(%client,
      'MsgClientJoin', 'Welcome to Tauris, %1.',
      %client.playerName, 
      %client,
      %client.sendGuid,
      %client.team,
      %client.score,
      %client.isAiControlled(), 
      %client.isAdmin, 
      %client.isSuperAdmin);

   // Inform all the other clients of the new guy
   messageAllExcept(%client, -1, 'MsgClientJoin', '\c1%1 joined the game.', 
      %client.playerName, 
      %client,
      %client.sendGuid,
      %client.team,
      %client.score,
      %client.isAiControlled(), 
      %client.isAdmin, 
      %client.isSuperAdmin);

   // If the mission is running, go ahead download it to the client
   if ($missionRunning)
   {
      %client.loadMission();
   }
   else if ($Server::LoadFailMsg !$= "")
   {
      messageClient(%client, 'MsgLoadFailed', $Server::LoadFailMsg);
   }
   $Server::PlayerCount++;
}

//-----------------------------------------------------------------------------
function GameConnection::loadMission(%this)
{
   // Send over the information that will display the server info
   // when we learn it got there, we'll send the data blocks
   %this.currentPhase = 0;
   if (%this.isAIControlled())
   {
      // Cut to the chase...
      %this.onClientEnterGame();
   }
   else
   {
      commandToClient(%this, 'MissionStartPhase1', $missionSequence,
         $Server::MissionFile, MissionGroup.musicTrack);
      echo("*** Sending mission load to client: " @ $Server::MissionFile);
   }
}

//-----------------------------------------------------------------------------
// A player's name could be obtained from the auth server, but for
// now we use the one passed from the client.
// %realName = getField( %authInfo, 0 );
//
function GameConnection::setPlayerName(%client,%name)
{
   %client.sendGuid = 0;

   // Minimum length requirements
   %name = trim( strToPlayerName( %name ) );
   if ( strlen( %name ) < 3 )
      %name = "Poser";

   // Make sure the alias is unique, we'll hit something eventually
   if (!isNameUnique(%name))
   {
      %isUnique = false;
      for (%suffix = 1; !%isUnique; %suffix++)  {
         %nameTry = %name @ "." @ %suffix;
         %isUnique = isNameUnique(%nameTry);
      }
      %name = %nameTry;
   }

   // Tag the name with the "smurf" color:
   %client.nameBase = %name;
   %client.playerName = addTaggedString("\cp\c8" @ %name @ "\co");
}

function isNameUnique(%name)
{
   %count = ClientGroup.getCount();
   for ( %i = 0; %i < %count; %i++ )
   {
      %test = ClientGroup.getObject( %i );
      %rawName = stripChars( detag( getTaggedString( %test.playerName ) ), "\cp\co\c6\c7\c8\c9" );
      if ( strcmp( %name, %rawName ) == 0 )
         return false;
   }
   return true;
}

//-----------------------------------------------------------------------------
// This function is called when a client drops for any reason
//
function GameConnection::onDrop(%client, %reason)
{
   game.onClientLeaveGame(%client);
   
   removeFromServerGuidList( %client.guid );
   messageAllExcept(%client, -1, 'MsgClientDrop', '\c1%1 has left the game.', %client.playerName, %client);

   removeTaggedString(%client.playerName);
   echo("CDROP: " @ %client @ " " @ %client.getAddress());
   $Server::PlayerCount--;
   
   // Reset the server if everyone has left the game
   if( $Server::PlayerCount == 0 && $Server::Dedicated)
      schedule(0, 0, "resetServerDefaults");
}

//-----------------------------------------------------------------------------

function GameConnection::onDeath(%this, %sourceObject, %sourceClient, %damageType, %damLoc)
{
  game.onClientDeath(%this, %sourceObject, %sourceClient, %damageType, %damLoc);
}

//-----------------------------------------------------------------------------

function GameConnection::startMission(%this)
{
   // Inform the client the mission starting
   commandToClient(%this, 'MissionStart', $missionSequence, $Server::MissionType);
}


function GameConnection::endMission(%this)
{
   // Inform the client the mission is done.  Note that if this is
   // called as part of the server destruction routine, the client will
   // actually never see this comment since the client connection will
   // be destroyed before another round of command processing occurs.
   // In this case, the client will only see the disconnect from the server
   // and should manually trigger a mission cleanup.
   commandToClient(%this, 'MissionEnd', $missionSequence);
}

//--------------------------------------------------------------------------
//
//--------------------------------------------------------------------------

function GameConnection::getBuildingGroup(%this)
{
  %group = %this.buildGroup;
  //warn("1" SPC %group.class);
  if (!isObject(%group))
  {
    %group = %this.isHelper();
    //warn("2" SPC %group.class);
    if (isObject(%group))
    {
      %group = %this.getGroup();
      //warn("3" SPC %group.class);
    }
    else if (!isObject(%group))
    {
      %group = game.makeBuildGroup(%this);
      //warn("4" SPC %group.class);
    }
  }
  return %group;
}

//--------------------------------------------------------------------------

function GameConnection::isHelper(%this)
{
  for (%i = 0; %i < buildGroup.getCount(); %i++)
    if (buildGroup.getObject(%i).getGameObject().isHelper(%client))
      return buildGroup.getObject(%i).getGameObject();
  return false;
}

//--------------------------------------------------------------------------

function GameConnection::sendItem(%this, %data, %slot)
{
  messageClient(%this, "itemMsg", "", %data, %slot);
}

//--------------------------------------------------------------------------

function GameConnection::equip(%this, %data, %slot)
{
  messageClient(%this, "equipMsg", "", %data, %slot);
}

//--------------------------------------------------------------------------

function GameConnection::unEquip(%this, %slot)
{
  messageClient(%this, "equipMsg", "", "", %slot);
}

//--------------------------------------------------------------------------

function GameConnection::sendGuiDatastream(%this, %gui, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8)
{
  commandToClient(%this, 'guiDatastream', %gui, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8);
}

//--------------------------------------------------------------------------
// Sync the clock on the client.

function GameConnection::syncClock(%client, %time)
{
   commandToClient(%client, 'syncClock', %time);
}


//--------------------------------------------------------------------------
// Update all the clients with the new score

function GameConnection::incScore(%this,%delta)
{
   %this.score += %delta;
   messageAll('MsgClientScoreChanged', "", %this.score, %this);
}
