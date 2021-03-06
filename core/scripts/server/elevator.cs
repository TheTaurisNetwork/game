//-----------------------------------------------------------------------------
// Copyright (c) 2013 The Tauris Network
//-----------------------------------------------------------------------------



//-----------------------------------------------------------------------------
//  Elevator Functions
//-----------------------------------------------------------------------------

function Elevator::onDeploy(%data, %this, %player)
{
  return Asset::onDeploy(%data, %this);
}

//-----------------------------------------------------------------------------

function Elevator::setUp(%data, %this, %player)
{
 return %this.setPwrBranch( %this.pwrBranch );
}

//-----------------------------------------------------------------------------

function Elevator::onEnabled(%data, %this)
{
  if (!isObject(%this.trigger))
  {
    %this.trigger = startTrigger(%data.mode);
    %this.trigger.setScale("1 2 7");

    %pos = vectorAdd(%this.getPosition(), "0 0 0");
    %this.trigger.setPosition(%pos);
  }

  if (!isObject(%this.emitter))
  {
    %datablock = %data.mode ? "ElevatorUpEmitter" : "ElevatorDownEmitter";
    %offset = %data.mode ? "0 0 1" : "0 0 5";

    %this.emitter = new ParticleEmitterNode()
    {
      emitter = %datablock;
      velocity = "1";
      dataBlock = "ElevatorNodeData";
    };

    %this.mountObject(%this.emitter, 0);
    %this.emitter.mountPos = %offset;
  }
  else
    %this.emitter.setActive(1);

  Asset::onEnabled(%data, %this);
}

//-----------------------------------------------------------------------------

function Elevator::onDisabled(%data, %this)
{
  if (isObject(%this.trigger))
    %this.trigger.delete();
  if (isObject(%this.emitter))
    %this.emitter.setActive(0);

  Asset::onDisabled(%data, %this);
}

//-----------------------------------------------------------------------------

function Elevator::onHyperjump(%data, %this)
{
  if (isObject(%this.trigger))
  {
    %pos = vectorAdd(%this.getPosition(), "0 0 -2");
    %this.trigger.setPosition(%pos);
  }
}

//-----------------------------------------------------------------------------
//  Elevator Functions
//-----------------------------------------------------------------------------

function startTrigger(%up)
{
  %obj = new trigger()
  {
    dataBlock = "elevatorTrigger";
    polyhedron = "-0.5000000 0.5000000 0.0000000 1.0000000 0.0000000 0.0000000 0.0000000 -1.0000000 0.0000000 0.0000000 0.0000000 1.0000000";
    up = %up;
    down = %up ? 0 : 1;
  };
  return %obj;
}

//-----------------------------------------------------------------------------

function elevatorTrigger::onEnterTrigger(%this,%trigger,%obj)
{
  if (%trigger.up)
    %obj.setGravity( 20 );

  else
    %obj.setGravity( -10 );

  cancel(%obj.InteriorLoop);
}

//-----------------------------------------------------------------------------

function elevatorTrigger::onLeaveTrigger(%this,%trigger,%obj)
{
  %obj.setGravity( 0 );

  %obj.checkIfInside();
}

//-----------------------------------------------------------------------------








