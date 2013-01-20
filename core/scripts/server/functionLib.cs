//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------

// Helper and Misc functions go in here
//-----------------------------------------------------------------------------

function writeShip()
{
  new FileObject("File");
  File.openForWrite("core/temp.cs");

  %group = BuildingGroup;
  for (%i = 0; %i < %group.getCount(); %i++)
  {
    %obj = %group.getObject(%i);
    if (strstr(%obj.getDatablockName(), "DeployedSpin") > -1)
    {

      %obj.setDatablock("Block");
    }
    else
      %obj.delete();
  }
  File.writeObject(%group);

  File.close();
  File.delete();
}

function writeShip2()
{
  //%group = buildGroup.getObject(0).getPieceGroup();
  %group = missionCleanup;

  for (%i = 0; %i < %group.getCount(); %i++)
  {
    %obj = %group.getObject(%i);
    if (strstr(%obj.getDatablockName(), "DeployedSpine") > -1)
    {
      %scale = %obj.getRealSize();
      %obj.setDatablock( "block" );
      %obj.setRealSize( %scale );

    }
  }
}

//-----------------------------------------------------------------------------

function convert(%file)
{
  new FileObject("File"); //create file object
  File.openForWrite("core/new/"@%file@".cs"); //open it up, and create it if it isn't there
  new FileObject("File2"); //create file object
  File2.openForRead("core/"@%file@".cs"); //open it up, and create it if it isn't there

  while (!file2.isEOF())
  {
    %line = File.readLine();
    if (strstr(%line, "//"))
      continue;

//    %line = strReplace(%line, "StaticShape", "TSStatic");
//    %line = strReplace(%line, "StaticShape", "TSStatic");
    
	File2.writeLine(%text);
  }
  File.close(); //close the file
	File.delete(); //delete the object (not the file)
	File2.close(); //close the file
	File2.delete(); //delete the object (not the file)


}

//-----------------------------------------------------------------------------

function spawnGen()
{
  user.deployObject(user.testDeploy("GeneratorStandard"), user);
}

function spawntest(%gen)
{
  user.deployObject(user.testDeploy("Console"), user);
}

//-----------------------------------------------------------------------------

function SimObject::className(%this)
{
  if (%this.shapeName !$= "")
    return "TSStatic";
  if (%this.getClassName() $= "StaticShape")
    return %this.getDatablock().className;
  return false;
}

//-----------------------------------------------------------------------------

function SimObject::getIndexInGroup(%this)
{
  return %this.getGroup().getObjectIndex(%this);
}

//-----------------------------------------------------------------------------

function SimObject::setPosition(%this, %pos)
{
  %this.setTransform(%pos SPC %this.getRotation());
}

function SimObject::setRotation(%this, %rot)
{
  %this.setTransform(%this.getPosition() SPC %rot);
}

//-----------------------------------------------------------------------------

function SimObject::getPosition(%this)
{
  return getwords(%this.getTransform(), 0, 2);
}

function SimObject::getRotation(%this)
{
  return getwords(%this.getTransform(), 3, 3);
}

//-----------------------------------------------------------------------------

function StaticShape::setRechargeRate(%this, %e)
{
  %this.eDelta = %e;
  if (!isEventPending(%this.eRech))
    %this.rechCycle();
}

//-----------------------------------------------------------------------------

function StaticShape::stopEnergyRecharge(%this)
{
  if (isEventPending(%this.eRech))
    cancel(%this.eRech);
}

function StaticShape::startEnergyRecharge(%this)
{
  if (!isEventPending(%this.eRech))
    %this.rechCycle();
}

//-----------------------------------------------------------------------------

function StaticShape::rechCycle(%this)
{
  %this.setEnergyLevel(%this.energy + %this.eDelta);
  %this.eRech = %this.schedule(100, rechCycle);
}

//-----------------------------------------------------------------------------

function StaticShape::getRechargeRate(%this)
{
  return %this.eDelta;
}

function StaticShape::getEnergyLevel(%this)
{
  return %this.energy;
}

function StaticShape::setEnergyLevel(%this, %e)
{
  %data = %this.getDatablock();
  %e = %e > %data.maxEnergy ? %data.maxEnergy : %e;

  %this.energy = %e;
  if (%this.energy < 0)
    %this.energy = 0;

  return %this.energy;
}

//-----------------------------------------------------------------------------

function SimObject::decEnergyLevel(%this, %d)
{
  if (%this.getEnergyLevel() > %d)
  {
    %this.setEnergyLevel( %this.getEnergyLevel() - %d );
    return true;
  }
  return false;
}

//-----------------------------------------------------------------------------

$DefaultObjectMask =
  $TypeMasks::PlayerObjectType |
  $TypeMasks::ProjectileObjectType |
  $TypeMasks::VehicleObjectType |
  $TypeMasks::ItemObjectType;

$DefaultLOSMask =
  $TypeMasks::StaticTSObjectType |
  $TypeMasks::InteriorObjectType |
  $TypeMasks::StaticShapeObjectType |
  $typemasks::StaticRenderedObjectType;

$DefaultAllMask = $DefaultLOSMask | $DefaultObjectMask;

$DefaultInteractMask =
  $TypeMasks::StaticShapeObjectType |
  $TypeMasks::ItemObjectType |
  $TypeMasks::VehicleObjectType;

//-----------------------------------------------------------------------------

function ShapeBase::Sight(%player, %range, %mask, %ignore)
{
  if (%range $= "")
    %range = 100;

  if (%mask $= "")
    %mask = $DefaultAllMask;

  if (%ignore $= "")
    %ignore = %player.GetId();

  %pos = %player.getEyePoint();
  %vec = %player.getEyeVector();

  %targetpos = vectoradd(%pos, vectorscale(%vec, %range));
  %obj = containerraycast(%pos, %targetpos, %mask, %ignore);

  // near the ground
  %pos = %player.GetPosition();

  %targetpos = vectoradd(%pos, vectorscale(%vec, %range));
  %obj2 = containerraycast(%pos, %targetpos, %mask, %ignore);

  if (%obj $= "" && %obj2 !$= "")
    %obj = %obj2;

  return %obj;
}

//-----------------------------------------------------------------------------

function ShapeBase::SightObject(%player, %range, %mask, %ignore)
{
  %obj = %player.Sight(%range, %mask, %ignore);

  return getword(%obj, 0);
}

//-----------------------------------------------------------------------------

function ShapeBase::SightPos(%player, %range, %mask, %ignore)
{
  %obj = %player.Sight(%range, %mask, %ignore);

  return getWords(%obj, 1, 3);
}

//-----------------------------------------------------------------------------

function ShapeBase::isClearToHitPos(%player, %targetpos)
{
  if (%mask $= "")
    %mask = $DefaultAllMask;

  %pos = %player.getEyePoint();

  %obj = containerraycast(%pos, %targetpos, %mask, %player.GetId());

  return getWords(%obj, 1, 3);
}

//-----------------------------------------------------------------------------

function ShapeBase::isClearToHitObj(%player, %targetpos)
{
  if (%mask $= "")
    %mask = $DefaultAllMask;

  %pos = %player.getEyePoint();

  %obj = containerraycast(%pos, %targetpos, %mask, %player.GetId());

  return getWord(%obj, 0);
}

//-----------------------------------------------------------------------------

function SimObject::getDatablockName(%this)
{
  return %this.getDatablock().getName();
}

//-----------------------------------------------------------------------------

function SimObject::GetShapeSize(%obj)
{
  return VectorSub(getWords(%obj.getObjectBox(),3,5),getWords(%obj.getObjectBox(),0,2));
}

//-----------------------------------------------------------------------------

function SimObject::SetRealSize(%obj,%size)
{
  %scale = vectorDivide(%size, %obj.getShapeSize());
  %obj.setScale(%scale);
}

function SimObject::GetRealSize(%obj)
{
  %return = vectorMultiply(%obj.getScale(), %obj.getShapeSize());
  return %return;
}
//-----------------------------------------------------------------------------

function SimObject::setEdge(%obj,%location,%offset)
{
%VirCenter = vectorScale(vectorAdd(getWords(%obj.getObjectBox(),3,5),getWords(%obj.getObjectBox(),0,2)),0.5);
%VirOffset = VectorMultiply(vectorScale(%offset,0.5),%obj.getShapeSize());
%realoffset = RealVec(%obj,VectorMultiply(vectorAdd(%VirCenter,%VirOffset),%obj.getScale()));
%pos = vectorAdd(%location,vectorScale(%realoffset,-1));
%obj.setTransform(%pos SPC %obj.getRotation());
}

function SimObject::GetEdge(%obj,%offset)
{
%VirCenter = vectorScale(vectorAdd(getWords(%obj.getObjectBox(),3,5),getWords(%obj.getObjectBox(),0,2)),0.5);
%VirOffset = VectorMultiply(vectorScale(%offset,0.5),%obj.getShapeSize());
%realoffset = RealVec(%obj,VectorMultiply(vectorAdd(%VirCenter,%VirOffset),%obj.getScale()));
return vectorAdd(GetWords(%obj.getTransform(),0,2),%realoffset);
}

//-----------------------------------------------------------------------------
// returns the closest edge of a piece to a given position.
function SimObject::GetClosestAxis(%this, %pos)
{
  /*
  %axis[0] = "-1 1 -1";
  %axis[1] = "-1 0 -1";
  %axis[2] = "-1 -1 -1";
  %axis[3] = "0 1 -1";
  %axis[4] = "0 0 -1";
  %axis[5] = "0 -1 -1";
  %axis[6] = "1 1 -1";
  %axis[7] = "1 0 -1";
  %axis[8] = "1 -1 -1";
  */

  %axis[0] = "-1 1 -1";
  %axis[1] = "-1 0 -1";
  %axis[2] = "-1 -1 -1";
  %axis[3] = "0 1 -1";
  %axis[4] = "0 0 -1";
  %axis[5] = "0 -1 -1";
  %axis[6] = "1 1 -1";
  %axis[7] = "1 0 -1";
  %axis[8] = "1 -1 -1";
  
  %axis[9] = "1 -1 -1";
  %axis[10] = "0 -1 -1";
  %axis[11] = "-1 -1 -1";
  %axis[12] = "1 0 -1";
  %axis[13] = "0 0 -1";
  %axis[14] = "-1 0 -1";
  %axis[15] = "1 -1 -1";
  %axis[16] = "0 -1 -1";
  %axis[17] = "-1 -1 -1";
  
  %axis[18] = "-1 1 0";
  %axis[19] = "-1 0 0";
  %axis[20] = "-1 -1 0";
  %axis[21] = "0 1 0";
  %axis[22] = "0 0 0";
  %axis[23] = "0 -1 0";
  %axis[24] = "1 1 0";
  %axis[25] = "1 0 0";
  %axis[26] = "1 -1 0";

  %axis[27] = "1 -1 0";
  %axis[28] = "0 -1 0";
  %axis[29] = "-1 -1 0";
  %axis[30] = "1 0 0";
  %axis[31] = "0 0 0";
  %axis[32] = "-1 0 0";
  %axis[33] = "1 -1 0";
  %axis[34] = "0 -1 0";
  %axis[35] = "-1 -1 0";

  %axis[36] = "-1 1 1";
  %axis[37] = "-1 0 1";
  %axis[38] = "-1 -1 1";
  %axis[39] = "0 1 1";
  %axis[40] = "0 0 1";
  %axis[41] = "0 -1 1";
  %axis[42] = "1 1 1";
  %axis[43] = "1 0 1";
  %axis[44] = "1 -1 1";

  %axis[45] = "1 -1 1";
  %axis[46] = "0 -1 1";
  %axis[47] = "-1 -1 1";
  %axis[48] = "1 0 1";
  %axis[49] = "0 0 1";
  %axis[50] = "-1 0 1";
  %axis[51] = "1 -1 1";
  %axis[52] = "0 -1 1";
  %axis[53] = "-1 -1 1";


  %closestside = 10000;
  for (%i = 0; %i < 4; %i++)
  {
     %side_test = %this.getEdge(%axis[%i]);
     %dist = vectorDist(%pos, %side_test);

     if (%dist < %closestside)
     {
       %closestside = %dist;
       %closestInd = %i;
       %closestPos = %side_test;
     }
  }

  return %axis[%closestInd] SPC %closestPos;
}

//-----------------------------------------------------------------------------

function SimObject::GetClosestEdgeAxis(%obj, %pos)
{
  return GetWords(%obj.GetClosestAxis(%pos), 0, 2);
}

function SimObject::GetClosestEdgePos(%obj, %pos)
{
  return GetWords(%obj.GetClosestAxis(%pos), 3, 5);
}


//-----------------------------------------------------------------------------

function SimObject::getIndexInGroup(%this)
{
  for (%i = 0; %i < %this.getGroup().getCount(); %i++)
     if (%this.getGroup().getObject(%i) == %this)
       return %i;
}

//-----------------------------------------------------------------------------

function realvec(%obj,%vec)
{
  %rot = %obj.GetRotation();
  return MatrixMulVector("0 0 0" SPC %rot, %vec);
}

function rot(%loc,%mod)
{
  return %obj.getRotation();
}

function RotFromTransform(%transform)
{
  return GetWords(%transform, 3, 6);
}

function validateVal(%val)
{
  if (strStr(%val,"nan") != -1 || strStr(%val,"inf") != -1)
  {
	%val = "";
	error("Rot error no. " @ $RotErrors++);
  }
  return %val;
}

//-----------------------------------------------------------------------------

function vectormultiply(%vec1,%vec2)
{
  return getWord(%vec1,0) * getWord(%vec2,0) SPC getWord(%vec1,1)*  getWord(%vec2,1) SPC getWord(%vec1,2)*  getWord(%vec2,2);
}

function vectorDivide(%vec1,%vec2)
{
  return getWord(%vec1,0) / getWord(%vec2,0) SPC getWord(%vec1,1)/  getWord(%vec2,1) SPC getWord(%vec1,2)/  getWord(%vec2,2);
}

//-----------------------------------------------------------------------------

// Return angle between two vectors
function getAngle(%vec1, %vec2)
{
  %vec1n = VectorNormalize(%vec1);
  %vec2n = VectorNormalize(%vec2);

  %vdot = VectorDot(%vec1n, %vec2n);
  %angle = mACos(%vdot);

  // convert to degrees and return
  %degangle = mRadToDeg(%angle);
  return %degangle;
}

//-----------------------------------------------------------------------------

// returns the closest axis of a piece to a given position.
function GetClosestSide(%obj, %pos)
{
  %axis[0] = "1 0 0";
  %axis[1] = "-1 0 0";
  %axis[2] = "0 1 0";
  %axis[3] = "0 -1 0";
  %axis[4] = "0 0 1";
  %axis[5] = "0 0 -1";

  %closestside = 10000;
  for (%i = 0; %i < 5; %i++)
  {
     %side_test = %obj.getEdge(%axis[%i]);
     %dist = vectorDist(%pos, %side_test);

     if (%dist < %closestside)
     {
       %closestside = %dist;
       %closestInd = %i;
     }
  }

  return %axis[%closestInd];
}


//-------------------------------------------------------------------------

function GetVectorFromPos(%pos1, %pos2)
{
 return VectorNormalize(VectorSub(%pos1, %pos2));
}


//-----------------------------------------------------------------------------

//Returns a rotation from a normal that matches the normal and faces up the slope
function slopeFromNrm(%up)
{
  %x=getWord(%up,0);
  %y=getWord(%up,1);
  %z=getWord(%up,2);
  %xy = Mpow((Mpow(%x,2)+Mpow(%y,2)),0.5);
  %zrot = MAtan(%y,%x)-1.5708;
  %xrot = -MAtan(%xy,%z);
  return  %xrot SPC "0" SPC %zrot;
}

//-----------------------------------------------------------------------------

function scanArea(%pos, %radius, %mask)
{
   InitContainerRadiusSearch(%pos, %radius, %mask);
   while((%int = ContainerSearchNext()) != 0)
   {
      if(%int)
         return true;
   }

   return false;
}

//-----------------------------------------------------------------------------




//-----------------------------------------------------------------------------

