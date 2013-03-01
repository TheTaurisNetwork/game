//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//  Deployable Index
//-----------------------------------------------------------------------------

$dep[0] = "Block1";
$dep[0, c] = "1";
$dep[1] = "GeneratorStandard";
$dep[1, c] = "0";
$dep[2] = "GeneratorLarge";
$dep[2, c] = "0";
$dep[3] = "Console";
$dep[3, c] = "0";
$dep[4] = "HelmConsole";
$dep[4, c] = "0";
$dep[5] = "HyperDriveMKI";
$dep[5, c] = "0";
$dep[6] = "HyperDriveMKII";
$dep[6, c] = "0";
$dep[7] = "HyperDriveMKIII";
$dep[7, c] = "0";
$dep[8] = "small_corridor0.dae";
$dep[8, c] = "1";
$dep[9] = "medium_corridor0.dae";
$dep[9, c] = "1";
$dep[10] = "small_room0.dae";
$dep[10, c] = "1";
$dep[11] = "ship0.dae";
$dep[11, c] = "2";
$dep[12] = "InventoryStation";
$dep[12, c] = "0";
$dep[13] = "Lights";
$dep[13, c] = "0";
$dep[14] = "upElevator";
$dep[14, c] = "0";
$dep[15] = "downElevator";
$dep[15, c] = "0";
$dep[16] = "doorframe0.dae";
$dep[16, c] = "1";
$dep[17] = "Door";
$dep[17, c] = "0";
$dep[18] = "doorSwitch";
$dep[18, c] = "0";

$dep[max] = 19;

$nameToInv["Block1"] = "Block";
$nameToInv["GeneratorStandard"] = "Standard Generator";
$nameToInv["GeneratorLarge"] = "Large Generator";
$nameToInv["Console"] = "Console";
$nameToInv["HelmConsole"] = "Helm";
$nameToInv["HyperDriveMKI"] = "Hyper-Drive MKI";
$nameToInv["HyperDriveMKII"] = "Hyper-Drive MKII";
$nameToInv["HyperDriveMKIII"] = "Hyper-Drive MKIII";
$nameToInv["small_corridor0.dae"] = "Small Corridor";
$nameToInv["medium_corridor0.dae"] = "Medium Corridor";
$nameToInv["small_room0.dae"] = "Small Room";
$nameToInv["doorframe0.dae"] = "Small Doorframe";
$nameToInv["ship0.dae"] = "Exterior 0";
$nameToInv["InventoryStation"] = "Inventory Station";
$nameToInv["Lights"] = "Interior Lighting";
$nameToInv["upElevator"] = "Up Elevator";
$nameToInv["downElevator"] = "Down Elevator";
$nameToInv["door"] = "Door";
$nameToInv["doorSwitch"] = "Door Control";

//-----------------------------------------------------------------------------
//  Shapebase functions
//-----------------------------------------------------------------------------

function shapeBase::testDeploy(%this, %data, %col)
{
  if (isObject(%this.testingDeploy))
    %this.testingDeploy.delete();

  echo(%data.getname());

  if (%data.getClassName() !$= "")
    %this.testingDeploy = spawnStaticShape(%data);
  else if (%col == 2)
    %this.testingDeploy = spawnTSStaticShape(%data, 1, 1);
  else
    %this.testingDeploy = spawnTSStaticShape(%data, %col);

  missionCleanup.add(%this.testingDeploy);

  deployFollowPoint(%this, %this.testingDeploy);

  return %this.testingDeploy;
}

//-----------------------------------------------------------------------------

function deployFollowPoint(%this, %obj)
{
  if (%this.testingDeploy != %obj)
    return;

  %pos = %this.SightPos(20, $DefaultLOSMask, %obj);
  
  if (getWordCount(%pos) == 3)
    %obj.setEdge(%pos, "0 0 -1");
  else
    %pos = vectoradd(%this.getEyePoint(), vectorscale(%this.getEyeVector(), 20));

  %this.deployFollowPointLoop = schedule(250, 0, deployFollowPoint, %this, %obj);
}

//-----------------------------------------------------------------------------

function shapeBase::deployObject(%this, %obj)
{
  cancel(%this.deployFollowPointLoop);

  %this.deployFollowPointLoop = "";
  %this.testingDeploy = "";

  %pos = %this.SightPos(20, $DefaultLOSMask);

  %obj.deployer = %this.client;

  if (getWordCount(%pos) == 3)
    %obj.setEdge(%pos, "0 0 -1");
  else
    %pos = vectoradd(%this.getEyePoint(), vectorscale(%this.getEyeVector(), 20));

  if (%obj.getClassName() !$= "TSStatic")
  {
    if (%obj.getDatablock().onDeploy(%obj, %this))
      game.deployObject(%obj, %this);
    //else
    //  %obj.delete();
  }
}

//-----------------------------------------------------------------------------
//     Asset Class
//-----------------------------------------------------------------------------

function Asset::onDeploy(%data, %this, %player)
{
  %this.needsPower = %data.needsPower;
  if (%this.needsPower)
  {
    %this.pwrBranch = 0;
    %this.pwrRequired = %data.pwrRequired;
  }

  switch$(%data.getName())
  {
       case "Console":

//           %this.pwrRequired = %data.pwrRequired;
//           %this.pwrRequired = %data.pwrRequired;

  }
  return true;
}

//-----------------------------------------------------------------------------

function Asset::setUp(%data, %this, %player)
{

}

//-----------------------------------------------------------------------------

function Asset::onInteract(%data, %this, %player)
{

}


//-----------------------------------------------------------------------------

