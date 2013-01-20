
singleton TSShapeConstructor(CubeDae)
{
   baseShape = "./cube.dae";
   loadLights = "0";
};

function CubeDae::onLoad(%this)
{
   %this.setNodeTransform("mesh32", "0 0 0 1 0 0 0", "1");
   %this.addNode("Col-1", "", "0 0 0 0 0 1 0", "0");
   %this.addCollisionDetail("-1", "10-DOP X", "Bounds", "4", "30", "30", "32", "30", "30", "30");
}
