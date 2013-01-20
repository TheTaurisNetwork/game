//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function BattleGame::initGameType(%game)
{
  Game.initGameType(%game);

  $Game::DefaultPlayerDatablock  = "LightMaleBuilder";
}

//-----------------------------------------------------------------------------

function BattleGame::onStart(%game)
{


    parent::onStart(%game);
}

//-----------------------------------------------------------------------------

function BattleGame::onEnd(%game)
{


   parent::onEnd(%game);
}

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function BattleGame::onClientEnterGame(%game, %this)
{
  Parent::onClientEnterGame(%game, %this);
}

function BattleGame::onClientLeaveGame(%this, %client)
{
  Parent::onClientLeaveGame(%this, %client);
}

//-----------------------------------------------------------------------------
// Handle a player's death
//-----------------------------------------------------------------------------

function BattleGame::onClientDeath(%game, %this, %sourceObject, %sourceClient, %damageType, %damLoc)
{
   Parent::onClientDeath(%game, %this, %sourceObject, %sourceClient, %damageType, %damLoc);
}

