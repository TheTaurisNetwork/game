
singleton TSShapeConstructor(Cube4Dae)
{
   baseShape = "./cube4.dae";
};

function Cube4Dae::onLoad(%this)
{
   %this.addNode("Col-1", "", "0 0 0 0 0 1 0", "0");
   %this.addCollisionDetail("-1", "10-DOP X", "Bounds", "4", "30", "30", "32", "30", "30", "30");
}
