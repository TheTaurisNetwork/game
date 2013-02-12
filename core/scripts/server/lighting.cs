//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// Light class functions go here.

//-----------------------------------------------------------------------------

function Lights::onDeploy(%data, %this, %player)
{
  return Asset::onDeploy(%data, %this);
}

//-----------------------------------------------------------------------------

function Lights::setUp(%data, %this, %player)
{
   %data.spawnLight(%this);

   return %this.setPwrBranch( %this.pwrBranch );
}

//-----------------------------------------------------------------------------

function Lights::spawnLight(%data, %this)
{
   %light = new PointLight()
   {
      radius = "3";
      isEnabled = "1";
   };
   missionCleanup.add(%light);
   %light.setPosition(%this.getPosition());

   %this.light = %light;
   %light.pointBase = %this;
}


//-----------------------------------------------------------------------------

function Lights::onEnabled(%data, %this)
{
  if (!isObject(%this.light))
    %this.light = %data.spawnLight(%this);

  else
    %this.light.setLightEnabled( true );

  Asset::onEnabled(%data, %this);
}

//-----------------------------------------------------------------------------

function Lights::onDisabled(%data, %this)
{
  if (isObject(%this.light))
    %this.light.setLightEnabled( false );
  echo(%this.light);
  Asset::onDisabled(%data, %this);
}







