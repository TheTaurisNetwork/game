//-----------------------------------------------------------------------------
// Copyright (c) 2012 The Tauris Network
//-----------------------------------------------------------------------------



//-----------------------------------------------------------------------------



//-----------------------------------------------------------------------------
//  Simgroup Function
//-----------------------------------------------------------------------------

function pieceGroup::Add(%this, %obj)
{
  parent::add(%this, %obj);

  %obj.setPowerState( %this.getGroup().getPowerGroup().getBranchState(%obj.pwrBranch) );
}

//-----------------------------------------------------------------------------

function pieceGroup::remove(%this, %obj)
{
  %obj.setPowerState( 0 );


  parent::remove(%this, %obj);
}

//-----------------------------------------------------------------------------






//-----------------------------------------------------------------------------
//  Helpers
//-----------------------------------------------------------------------------



