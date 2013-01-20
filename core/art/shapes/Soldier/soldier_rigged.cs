
singleton TSShapeConstructor(SoldierDAE)
{
   baseShape = "./soldier_rigged.dae";
   loadLights = "0";
   unit = "1.2";
   upAxis = "DEFAULT";
   lodType = "DetectDTS";
   singleDetailSize = "2";
   matNamePrefix = "soldier_";
   ignoreNodeScale = "0";
   adjustCenter = "0";
   adjustFloor = "0";
};

function SoldierDAE::onLoad(%this)
{
   %this.addSequence("ambient", "root", "0", "60", "1", "0");
   %this.addSequence("ambient", "run", "1510", "1530", "1", "0");
   %this.addSequence("ambient", "back", "1610", "1630", "1", "0");
   %this.addSequence("ambient", "side", "1560", "1580", "1", "0");
   %this.addSequence("ambient", "crouch_root", "330", "390", "1", "0");
   %this.addSequence("ambient", "crouch_forward", "1660", "1690", "1", "0");
   %this.addSequence("ambient", "crouch_backward", "1720", "1750", "1", "0");
   %this.addSequence("ambient", "crouch_side", "1780", "1810", "1", "0");
   %this.addSequence("ambient", "death1", "1230", "1270", "1", "0");
   %this.setSequenceCyclic("death1", "0");
   %this.addSequence("ambient", "death2", "1300", "1340", "1", "0");
   %this.setSequenceCyclic("death2", "0");
   %this.addSequence("ambient", "death3", "1370", "1410", "1", "0");
   %this.setSequenceCyclic("death3", "0");
   %this.addSequence("ambient", "jump", "1040", "1050", "1", "0");
   %this.setSequenceCyclic("jump", "0");
   %this.addSequence("ambient", "land", "1950", "1960", "1", "0");
   %this.setSequenceCyclic("land", "0");
   %this.addSequence("ambient", "fall", "1890", "1905", "1", "0");
   %this.setSequenceCyclic("fall", "0");
   %this.addSequence("ambient", "sitting", "1840", "1840", "1", "0");
   %this.addSequence("ambient", "look", "1990", "2000", "1", "0");
   %this.setSequenceCyclic("look", "0");
   %this.setSequenceBlend("look", "1", "root", "0");
   %this.setNodeTransform("cam", "0 -0.104758 2.03657 1 0 0 0", "1");
   %this.setNodeTransform("eye", "0.180498 0.488794 1.80048 0.639455 -0.245395 -0.728614 0", "1");
}
