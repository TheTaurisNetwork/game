
singleton TSShapeConstructor(plasmaturretDae)
{
   baseShape = "./plasmaturret.dae";
};

function plasmaturretDae::onLoad(%this)
{
   %this.addNode("mountPoint", "", "0 0 0 0 0 1 0", "0");
   %this.setNodeTransform("Sphere", "0 0 1.36466 1 0 0 0", "1");
   %this.addNode("muzzlePoint", "mountPoint", "0 -2.66411 0.563649 1 0 0 0", "1");
   %this.addNode("ejectPoint", "mountPoint", "0 -2.15442 0.377502 1 0 0 0", "1");
   %this.addNode("eyeMount", "mountPoint", "0 0 1.5 1 0 0 0", "1");
   %this.addNode("camNode", "", "0 1.64695 2.53352 1 0 0 0", "1");
}
