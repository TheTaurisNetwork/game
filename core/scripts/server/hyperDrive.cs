//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
//     HyperDrive functions
//-----------------------------------------------------------------------------

function Drive::onDeploy(%data, %this, %player)
{
  return Asset::onDeploy(%data, %this, %player);
}

//-----------------------------------------------------------------------------

function Drive::setUp(%data, %this, %player)
{
  if (isObject(%this.getGroup()-->HyperDrive))
    %this.delete();
  %this.internalName = "HyperDrive";
  %ShipObject = %this.getGroup().getGroup().getShipObject();

  %this.setPwrBranch( %this.pwrBranch );

  %this.setRechargeRate( %data.rechargeRate );

  return %ShipObject.calcHDfieldExtent();
}

//-----------------------------------------------------------------------------

function Drive::onEnabled(%data, %this)
{
  %this.getGroup().addLoad( %data.pwrRequired );
  warn(%data.getName()@"::onEnabled(%data, %this)");
}

//-----------------------------------------------------------------------------

function Drive::onDisabled(%data, %this)
{
  if (!%this.preloadPower)
    %this.getGroup().removeLoad( %data.pwrRequired );
  %this.preloadPower = "";
  if (%this.powering)
  {
    if (%this.getGroup().getGroup().jumpSeq > 1)
      echo("%blowup");
    else
      %this.getGroup().getGroup().getGameObject().setErrCode( %this, 3 );
  }
  warn(%data.getName()@"::onDisabled(%data, %this)");
}

//-----------------------------------------------------------------------------

function Drive::onDestroyed(%data, %this)
{
  if (%this.powering)
    %this.getGroup().getGroup().getGameObject().setErrCode( %this );

  warn(%data.getName()@"::onDestroyed(%data, %this)");
}

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function Drive::onInteract(%data, %this)
{
  %gameObject = %this.getGroup().getGroup().getGameObject();
  if (%gameObject.jumpSeq == 4)
  {
    %gameObject.setErrCode( %this, 5 );
    return;
  }
  if (%this.powering)
    %gameObject.setErrCode( %this, 0 );

  %coords = %gameObject.getJumpCoords();
  %code = %data.testJumpPossibility(%this, %coords);

  if (%code)
  {
    if ( %this.decEnergyLevel( 5 ) )
    {
      %data.startJumpSequence(%this, %gameObject);
      //messageShip(%this.getGroup().getGroup(), 'shipMsg', "Hyper-drive ignition... Prepare for jump");
    }
    else
      %gameObject.setErrCode( %this, 1 );
  }
  else if (!%code)
    %gameObject.setErrCode( %this, 2 );
  else
    %gameObject.setErrCode( %this, 4 );
}

//-----------------------------------------------------------------------------

function Drive::testJumpPossibility(%data, %this, %coords)
{
  if (getwordCount(%coords) < 3)
    return -1;

//  %gameObject = %this.getGroup().getGroup().getGameObject();
  %shipObject = %this.getGroup().getGroup().getShipObject();

  %dist = vectorDist(%shipObject.getCenterPoint(), %coords);

  if (%data.getMaxJumpDist(%this) > %dist)
    return true;
  return false;
}

//-----------------------------------------------------------------------------

function Drive::getMaxJumpDist(%data, %this)
{
  %shipObject = %this.getGroup().getGroup().getShipObject();
  return ((%this.getEnergyLevel() - 5) * %data.driveModifier / %shipObject.getShipMass()) * 1000;
}

//-----------------------------------------------------------------------------

function Drive::startJumpSequence(%data, %this, %gameObject)
{
  %this.powering = true;
  
  %this.stopEnergyRecharge();

  %shipObject = %this.getGroup().getGroup().getShipObject();
  %dist = vectorDist(%shipObject.getCenterPoint(), %gameObject.getJumpCoords());

  %gameObject.jumpSeq = 1;
  %gameObject.Edelta = (%shipObject.getShipMass() * (%dist/1000)) / %data.driveModifier;

  messageShip(%this.getGroup().getGroup(), 'shipMsg', "++Hyper-drive > Ignition engaged++");

  %data.schedule(4900, jumpSequence, %this, %gameObject);
  %gameObject.driveCountDown(5);
  %gameObject.schedule(1000, driveCountDown);
  %gameObject.schedule(2000, driveCountDown);
  %gameObject.schedule(3000, driveCountDown);
  %gameObject.schedule(4000, driveCountDown);
}

//-----------------------------------------------------------------------------

function Drive::jumpSequence(%data, %this, %gameObject)
{
  if (%gameObject.jumpSeq == 1)
  {
    if (%this.decEnergyLevel( %gameObject.Edelta ))
    {
      %time = 100;
      %gameObject.jumpSeq++;
    }
    else
      %gameObject.setErrCode( %this, 3 );
  }
  else if (%gameObject.jumpSeq == 2)
  {
     %shipObject = %this.getGroup().getGroup().getShipObject();

     %cp = %shipObject.getCenterPoint();
     %offset = VectorSub(%gameObject.getJumpCoords(), %cp);

     InitContainerRadiusSearch(%cp, %shipObject.calcHDfieldExtent(), $DefaultAllMask);

     while ((%obj = ContainerSearchNext()) != 0)
     {
       if (%obj !$= "GroundPlane")
       {
         %obj.setPosition( VectorAdd(%obj.getPosition(), %offset) );
         if (%obj.className() !$= "TSStatic")
           %obj.getDatablock().onHyperjump(%obj);
       }
     }

     %time = 3000;
     %gameObject.jumpSeq++;

  }
  else if (%gameObject.jumpSeq == 3)
  {
     messageShip(%this.getGroup().getGroup(), 'shipMsg', "++Jump complete : Regaining normality++");
     %gameObject.jumpSeq++;

     %time = 20000;
  }
  else if (%gameObject.jumpSeq == 4)
  {
     %gameObject.jumpSeq = 0;

     %this.powering = false;
     %this.startEnergyRecharge();
  }

  if (%time !$= "")
    %this.jumpSeq = %data.schedule(%time, JumpSequence, %this, %gameObject);
  else
    %this.jumpSeq = "";
}

//-----------------------------------------------------------------------------





//-----------------------------------------------------------------------------
//        StaticShape
//-----------------------------------------------------------------------------

function StaticShapeData::onHyperjump(%this, %obj)
{

}






