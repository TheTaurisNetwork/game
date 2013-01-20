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


//-----------------------------------------------------------------------------
// Functions that process commands sent from the server.


// This function is for chat messages only; it is invoked on the client when
// the server does a commandToClient with the tag ChatMessage.  (Cf. the
// functions chatMessage* in core/scripts/server/message.cs.)

// This just invokes onChatMessage, which the mod code must define.

function clientCmdChatMessage(%sender, %voice, %pitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
   onChatMessage(detag(%msgString), %voice, %pitch);
}


// Game event descriptions, which may or may not include text messages, can be
// sent using the message* functions in core/scripts/server/message.cs.  Those
// functions do commandToClient with the tag ServerMessage, which invokes the
// function below.

// For ServerMessage messages, the client can install callbacks that will be
// run, according to the "type" of the message.

function clientCmdServerMessage(%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
   // Get the message type; terminates at any whitespace.
   %tag = getWord(%msgType, 0);

   // First see if there is a callback installed that doesn't have a type;
   // if so, that callback is always executed when a message arrives.
   for (%i = 0; (%func = $MSGCB["", %i]) !$= ""; %i++) {
      call(%func, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
   }

   // Next look for a callback for this particular type of ServerMessage.
   if (%tag !$= "") {
      for (%i = 0; (%func = $MSGCB[%tag, %i]) !$= ""; %i++) {
         call(%func, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
      }
   }
}




