
singleton TSShapeConstructor(Cube2Dae)
{
   baseShape = "./cube2.dae";
};

function Cube2Dae::onLoad(%this)
{
   %this.addNode("Col-1", "", "0 0 0 0 0 1 0", "0");
   %this.addCollisionDetail("-1", "10-DOP X", "Bounds", "4", "30", "30", "32", "30", "30", "30");
}
