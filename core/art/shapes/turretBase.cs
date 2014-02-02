
singleton TSShapeConstructor(TurretBaseDae)
{
   baseShape = "./turretBase.dae";
};

function TurretBaseDae::onLoad(%this)
{
   %this.addNode("heading", "", "0 0 0.5 1 0 0 0", "1");
   %this.addNode("pitch", "heading", "0 0 0 0 0 1 0", "0");
   %this.addNode("weaponMount0", "pitch", "0 0 0.5 1 0 0 0", "1");
   %this.addNode("Col-1", "", "0 0 0 1 0 0 0", "1");
   %this.addNode("ColBox-1", "Col-1", "0.00746639 -0.0263866 0.314916 0.889136 -0.323615 0.323589 1.68804", "0");
   %this.addCollisionDetail("-1", "Box", "Bounds", "4", "30", "30", "32", "30", "38.2979", "29.7872");
   %this.addNode("scanPoint", "pitch", "0 0 2 1 0 0 0", "1");
   %this.addNode("aimPoint", "pitch", "0 0 0.768078 1 0 0 0", "1");
   %this.setNodeTransform("Cylinder", "0 0 0 1 0 0 0", "1");
}
