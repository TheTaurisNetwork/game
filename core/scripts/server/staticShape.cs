//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
//     StaticShapeData Class
//-----------------------------------------------------------------------------
// pre-add to the asset group
function ShapeBase::onSelect(%data)
{
 warn(%data);
}

//-----------------------------------------------------------------------------
// post-add to the asset grou
function ShapeBase::onDeselect(%data)
{
 warn("onDeSelect" SPC %data);
}

//-----------------------------------------------------------------------------
// Assets added to the game in general
function ShapeBase::onUse(%data, %this)
{

}

//-----------------------------------------------------------------------------

function StaticShapeData::onInteract(%data, %this, %player)
{

}

//-----------------------------------------------------------------------------


function StaticShapeData::onEnabled(%data, %this)
{
}

//-----------------------------------------------------------------------------

function StaticShapeData::onDisabled(%data, %this)
{
}


//-----------------------------------------------------------------------------
//     StaticShape Class
//-----------------------------------------------------------------------------

function StaticShape::getGenByInd(%this, %ind)
{
 return %this.getGroup().getGenerator(%ind);
}

//-----------------------------------------------------------------------------

function StaticShape::setTexture(%this, %skin)
{
 if (%skin $= "")
   return false;
// %this.setSkinName(%skin);
 return %this.setDataBlock(%skin);
}

//-----------------------------------------------------------------------------

function StaticShape::isGenerator(%this)
{
 if (%this.ClassName() $= "Generator")
   return true;
}

//-----------------------------------------------------------------------------

function StaticShape::pwrRequired(%this)
{
  if (%this.getDatablock().needsPower)
    return false;
  return %this.getDatablock().pwrRequired;
}

//-----------------------------------------------------------------------------
//     Helper Class
//-----------------------------------------------------------------------------

function spawnStaticShape(%data)
{
  %r = new staticShape()
  {
      dataBlock = %data;
  };
  return %r;
}

//-----------------------------------------------------------------------------

function spawnTSStaticShape(%data, %bool, %isHull)
{
  %col = %bool ? "Visible Mesh" : "None";

  %r = new TSStatic()
  {
      shapeName = "core/art/shapes/"@%data;

      collisionType = %col;
      decalType = %col;
  };

  if (%isHull)
    %r.setInternalName("exterior");

  return %r;
}

//-----------------------------------------------------------------------------

function spawnTurretBase(%data)
{
  %r = new TurretShape()
  {
      dataBlock = %data;
  };
  return %r;
}

//-----------------------------------------------------------------------------


