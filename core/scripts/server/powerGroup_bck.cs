//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------



//----------------------------------------------------------------------------
//  Simgroup Function
//-----------------------------------------------------------------------------

function powerGroup::Add(%this, %obj)
{
  //warn("HI");
  parent::add(%this, %obj);
  %ind = %obj.getIndexInGroup();

  %data = %obj.getDatablock();
  if (%data.className $= "Generator")
  {
    %data.createBranch(%obj);

    %data.onEnergize(%obj);

    %pGroup = %this.getGroup().getPieceGroup();
    for (%i = 0; %i < %pGroup.getCount(); %i++)
    {
       %piece = %pGroup.getObject(%i);
       if (%piece.genInd >= %ind)
       {
         %piece.genInd++;
         %piece.generator = %this.getObject(%piece.genInd);
       }
    }
  }
  //return true;
}

//-----------------------------------------------------------------------------

function powerGroup::remove(%this, %obj)
{
  %ind = %obj.getIndexInGroup();
  %r = parent::remove(%this, %obj);

  %data = %obj.getDatablock();
  if (%data.className $= "Generator")
  {
    %pGroup = %this.getGroup().getPieceGroup();
    for (%i = 0; %i < %pGroup.getCount(); %i++)
    {
       %piece = %pGroup.getObject(%i);
       if (%piece.genInd == %ind)
         %piece.setGenerator(-1);
       else if (%piece.genInd > %ind)
       {
         %piece.genInd--;
         %piece.generator = %this.getObject(%piece.genInd);
       }
    }
  }
  return %r;
}

//-----------------------------------------------------------------------------

function powerGroup::getSigRadius(%this)
{
  return %this.sigRadius;
}

//-----------------------------------------------------------------------------
//  Generator Functions
//-----------------------------------------------------------------------------

function Generator::onDeploy(%data, %this, %player)
{
  %this.nameBase = "Gen_"@%this.getIndexInGroup();
  //parent::onDeploy(%this, %player);
}

//-----------------------------------------------------------------------------

function Generator::assetsPowered(%data, %this)
{
  %group = %this.getGroup().getGroup().getPieceGroup();
  %count = 0;
  for (%i = 0; %i < %group.getCount(); %i++)
  {
     %obj = %group.getObject(%i);
     if (%obj.needsPower && %obj.generator == %this)
       %count++;
  }
  return %count;
}

//-----------------------------------------------------------------------------

function Generator::createBranch(%data, %this)
{
  if (%this.branch[max] $= "")
    %this.branch[max] = 0;

  %this.branch[%this.branch[max]] = 1;   // branch enable state

  %this.branch[max]++;

  return true;
}

//-----------------------------------------------------------------------------

function Generator::createBranches(%data, %this, %num)
{
  for (%i = %this.branch[max]; %i < %num; %i++)
     %data.createBranch(%this);
}

//-----------------------------------------------------------------------------

function Generator::setBranchState(%data, %this, %branch, %bool)
{
  if (%branch == 0)
  {
    if (!%bool)
      return %data.onDeEnergize(%this);
    else if (%bool)
      return %data.onEnergize(%this);
  }
  if (%this.branch[%branch] $= "" || %this.branch[%branch] == %bool || !%this.enabled)
    return false;

  %this.branch[%branch] = %bool;

  %group = %this.getGroup().getGroup().getPieceGroup();
  for (%i = 0; %i < %group.getCount(); %i++)
  {
     %obj = %group.getObject(%i);
     if (%obj.needsPower && %obj.generator == %this && %obj.pwrBranch == %branch)
       %obj.setPowerState(%bool);
  }
  warn("Branch : "@ %branch @" is "@ %bool);
  return true;
}

//-----------------------------------------------------------------------------

function Generator::getBranchState(%data, %this, %branch)
{
  return %this.branch[%branch];
}

//-----------------------------------------------------------------------------

function Generator::onDisabled(%data, %this)
{
  %data.onDeEnergize(%this);
  parent::onDisabled(%this);
}

//-----------------------------------------------------------------------------

function Generator::onEnergize(%data, %this)
{
  if (%this.isDisabled())
    return false;
  %this.enabled = true;
  %this.power = %data.powerOut;

  %this.branch[0] = true;

  %this.getGroup().sigRadius += (%data.powerOut / 100);

  %group = %this.getGroup().getGroup().getPieceGroup();
  for (%i = 0; %i < %group.getCount(); %i++)
  {
     %obj = %group.getObject(%i);
     if (%obj.needsPower && %obj.generator == %this && %this.branch[%obj.pwrBranch] )
       %obj.setPowerState(%this, true);
  }

  // animation ;3
  return true;
}

//-----------------------------------------------------------------------------

function Generator::onDeEnergize(%data, %this)
{
  %this.enabled = false;
  %this.power = 0;

  %this.branch[0] = false;

  %this.getGroup().sigRadius -= (%data.powerOut / 100);

  %group = %this.getGroup().getGroup().getPieceGroup();
  for (%i = 0; %i < %group.getCount(); %i++)
  {
     %obj = %group.getObject(%i);
     if (%obj.needsPower && %obj.generator == %this)
       %obj.setPowerState(%this, false);
  }

  // animation ;3
  return true;
}

//-----------------------------------------------------------------------------
//  Asset Functions
//-----------------------------------------------------------------------------

function StaticShape::setGenerator(%this, %gen)
{
  if (!%this.needsPower)
    return false;
  if (isObject(%gen))
  {
    %this.genInd = %gen.getIndexInGroup();
    %this.generator = %gen;
  }
  else if (%gen > -1)
  {
    %this.genInd = %gen;
    %this.generator = %this.getGroup().getGroup().getGenerator(%gen);
  }
  else
  {
    %this.setPowerState(0);
    %this.genInd = %gen;
    %this.generator = "";
    return true;
  }
  return %this.setPwrBranch(0);
}

//-----------------------------------------------------------------------------

function StaticShape::setPwrBranch(%this, %branch)
{
  if (!%this.needsPower || %this.genInd == -1 || %this.generator.branch[%branch] $= "")
    return false;
  %this.pwrBranch = %branch;

  %pwr = %this.generator.branch[%branch];
  if (!%this.generator.enabled || !%this.generator.isEnabled())
    %pwr = false;

  %this.setPowerState( %pwr );
  return true;
}

//-----------------------------------------------------------------------------

function StaticShape::setPowerState(%this, %bool)
{
  if (%this.className() $= "Drive" && %this.powering)
  {
    if (!%bool)
    {
      if (!%this.enabled)
        %this.generator.getDatablock().onEnergize(%this.generator);
      if (!%this.generator.getBranchState(%this.pwrBranch))
        %this.generator.getDatablock().setBranchState(%this.generator, %this.pwrBranch, true);
    }
  }

  if (!%this.needsPower || %this.enabled == %bool)
    return false;
  if (%bool)
  {
    if (!isObject(%this.generator))
      %this.generator = %this.getObject(%this.genInd);
    if (!isObject(%this.generator))
    {
      %this.enabled = 0;
      %this.getDatablock().onDeEnergize(%this);
      return true;
    }

    %gen = %this.generator;

    if (%gen.power > %this.pwrRequired)
    {
      %gen.power = %gen.power - %this.pwrRequired;

      %this.enabled = %bool;

      %this.getDatablock().onEnergize(%this);
      return true;
    }
  }
  else if (!%bool)
  {
    %this.enabled = %bool;
    %this.getDatablock().onDeEnergize(%this);
    return true;
  }

  return false;
}

//-----------------------------------------------------------------------------

function Asset::onEnergize(%data, %this)
{
  warn(%data.getName()@"::onEnergize(%data, %this)");
  // class function. all datablocks will have different energize things...
}

//-----------------------------------------------------------------------------

function Asset::onDeEnergize(%data, %this)
{
  warn(%data.getName()@"::onDeEnergize(%data, %this)");
  // class function. all datablocks will have different deEnergize things...

}

//-----------------------------------------------------------------------------



