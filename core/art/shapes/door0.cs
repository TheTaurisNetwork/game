
singleton TSShapeConstructor(Door0Dae)
{
   baseShape = "./door0.dae";
};

function Door0Dae::onLoad(%this)
{
   %this.addSequence("ambient", "open", "0", "9", "1", "0");
   %this.addSequence("ambient", "close", "9", "18", "1", "0");
   %this.setSequenceCyclic("close", "0");
//   %this.renameSequence("ambient", "__backup__ambient_3");
//   %this.addSequence("__backup__ambient_3", "ambient", "0", "20", "1", "0");
   %this.setSequenceCyclic("ambient", "0");
   %this.setSequenceCyclic("open", "0");
//   %this.removeSequence("__backup__ambient_3");
//   %this.addNode("Col-1", "", "0 0 0 0 0 1 0", "0");
//   %this.addCollisionDetail("-1", "10-DOP X", "Plane_002", "4", "30", "30", "32", "30", "30", "30");
}
