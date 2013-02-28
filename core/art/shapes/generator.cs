
singleton TSShapeConstructor(GeneratorDae)
{
   baseShape = "./generator.dae";
};

function GeneratorDae::onLoad(%this)
{
   %this.addNode("Col-1", "", "0 0 0 0 0 1 0", "0");
   %this.addNode("ColBox-1", "Col-1", "0.0453393 0.00823048 0.845916 -0.000698159 -1 0.000175674 1.5708", "0");
   %this.addCollisionDetail("-1", "Box", "Bounds", "4", "30", "30", "32", "30", "30", "30");
}
