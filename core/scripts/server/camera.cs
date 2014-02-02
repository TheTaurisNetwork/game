//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

// Global movement speed that affects all cameras.  This should be moved
// into the camera datablock.
$Camera::movementSpeed = 30;

function Observer::onTrigger(%this,%obj,%trigger,%state)
{
   // state = 0 means that a trigger key was released
   if (%state == 0)
      return;

   // Default player triggers: 0=fire 1=altFire 2=jump
   %client = %obj.getControllingClient();
   switch$ (%obj.mode)
   {
      case "Observer":
         // Do something interesting.

      case "Destroyed":
         // Viewing dead corpse, so we probably want to respawn.
         %client.spawnPlayer();

         // Set the camera back into observer mode, since in
         // debug mode we like to switch to it.
         %this.setMode(%obj,"Observer");
   }
}

function Observer::setMode(%this,%obj,%mode,%arg1,%arg2,%arg3)
{
   switch$ (%mode)
   {
      case "Observer":
         // Let the player fly around
         %obj.setFlyMode();

      case "Control":
         // Lock the camera down in orbit around the corpse,
         // which should be arg1
         %transform = %arg1.getTransform();
         //%obj.setOrbitMode(%arg1, %transform, 0, 200, 0, 20);
         %scale = strLen(%obj.GetShapeSize());

         %obj.zoomIncs = %scale;

         %obj.setOrbitMode(%arg1, %transform, %scale, %scale * 50, %scale * 10);
         //%obj.setOrbitObject(%arg1, mDegToRad(50) @ " 0 0", 0, 10, 5);
   }
   %obj.mode = %mode;
}


//-----------------------------------------------------------------------------
// Camera methods
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------

function Camera::onAdd(%this,%obj)
{
   // Default start mode
   %this.setMode(%this.mode);
}

function Camera::setMode(%this,%mode,%arg1,%arg2,%arg3)
{
   // Punt this one over to our datablock
   %this.getDatablock().setMode(%this,%mode,%arg1,%arg2,%arg3);
}
