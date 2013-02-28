
%ship =  new SimGroup() {
      class = "ShipGroup";
      canSave = "1";
      canSaveDynamicFields = "1";
         builder = "3880";
         loading = "1";
         nameBase = "avenger";

      new ScriptObject() {
         internalName = "shipObject";
         class = "shipObject";
         canSave = "1";
         canSaveDynamicFields = "1";
      };
      new SimGroup() {
         internalName = "assetGroup";
         class = "assetGroup";
         canSave = "1";
         canSaveDynamicFields = "1";
            Branch0 = "1";
            Branch1 = "1";
            Branch2 = "1";
            Branchmax = "3";
            powerLoad = "0";
            power = "0";
      };
      new SimGroup() {
         internalName = "pieceGroup";
         class = "pieceGroup";
         canSave = "1";
         canSaveDynamicFields = "1";

         new TSStatic() {
            shapeName = "core/art/shapes/corridor_small5.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Collision Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "72.363 50.9736 18.4193";
            rotation = "1 0 0 0";
            scale = "1 1 1";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new TSStatic() {
            shapeName = "core/art/shapes/elevator_small0.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Collision Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "70.2545 31.1514 13.3459";
            rotation = "0 0 1 90";
            scale = "1 1 1";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new TSStatic() {
            shapeName = "core/art/shapes/ship0.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Visible Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "67.2942 37.587 17.7649";
            rotation = "1 0 0 0";
            scale = "1 1 1";
            internalName = "exterior";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new TSStatic() {
            shapeName = "core/art/shapes/elevator_small0.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Visible Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "72.8026 45.4516 22.43";
            rotation = "1 0 0 0";
            scale = "1 1 1";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new TSStatic() {
            shapeName = "core/art/shapes/corridor_small0.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Visible Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "67.2444 39.3158 23.2323";
            rotation = "1 0 0 0";
            scale = "1 1 1";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new TSStatic() {
            shapeName = "core/art/shapes/elevator_small0.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Visible Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "61.7204 16.2271 17.8949";
            rotation = "0 0 1 180";
            scale = "1 1 1";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new TSStatic() {
            shapeName = "core/art/shapes/corridor_small0.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Visible Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "67.2567 27.6323 23.2229";
            rotation = "1 0 0 0";
            scale = "1 1 1";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new TSStatic() {
            shapeName = "core/art/shapes/corridor_small2.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Visible Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "65.6881 25.0202 18.6851";
            rotation = "1 0 0 0";
            scale = "1 1 1";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new TSStatic() {
            shapeName = "core/art/shapes/corridor_small5.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Collision Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "62.2761 39.9402 18.4205";
            rotation = "0 0 1 180";
            scale = "1 1 1";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new TSStatic() {
            shapeName = "core/art/shapes/room_engine0.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Collision Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "79.3666 56.8002 18.4273";
            rotation = "1 0 0 0";
            scale = "1 1 1";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new TSStatic() {
            shapeName = "core/art/shapes/corridor_small4.dae";
            playAmbient = "1";
            meshCulling = "0";
            originSort = "0";
            collisionType = "Visible Mesh";
            decalType = "Visible Mesh";
            allowPlayerStep = "1";
            renderNormals = "0";
            forceDetail = "-1";
            position = "72.363 39.2996 18.4193";
            rotation = "1 0 0 0";
            scale = "1 1 1";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
      };
};
%ship.onLoaded();
