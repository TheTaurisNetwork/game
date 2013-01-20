singleton Material(cube2_y1)
{
   mapTo = "y1";
   diffuseColor[0] = "0.64 0.64 0.64 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";
};

singleton Material(cube2_x1)
{
   mapTo = "x1";
   diffuseColor[0] = "0.64 0.64 0.64 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";
};

singleton Material(cube2_z0)
{
   mapTo = "z0";
   diffuseColor[0] = "0.64 0.64 0.64 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";
};

singleton Material(cube2_y0)
{
   mapTo = "y0";
   diffuseColor[0] = "0.64 0.64 0.64 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";
};

singleton Material(cube2_x0)
{
   mapTo = "x0";
   diffuseColor[0] = "0.64 0.64 0.64 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";
};

singleton Material(cube2_z1)
{
   mapTo = "z1";
   diffuseColor[0] = "0.64 0.64 0.64 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";
};

singleton Material(material1)
{
   mapTo = "cube1";
   diffuseColor[0] = "0.64 0.64 0.64 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "20";
   doubleSided = "1";
   translucentBlendOp = "None";

   textureGun = 1;
};

singleton Material(material2)
{
   mapTo = "cube2";
   diffuseColor[0] = "0.14902 0.0666667 0 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "1";
   doubleSided = "1";
   translucentBlendOp = "None";

   textureGun = 1;
};

singleton Material(material3)
{
   mapTo = "cube3";
   diffuseColor[0] = "0.270588 0.270588 0.270588 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";

   textureGun = 1;
};

singleton Material(material4)
{
   mapTo = "cube4";
   diffuseColor[0] = "0.556863 0.827451 0.972549 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "Mul";
   emissive[0] = "1";
   castShadows = "0";
   translucent = "1";
   glow[0] = "0";
   translucentZWrite = "1";

   textureGun = 1;
};


singleton Material(section1_corridor0)
{
   mapTo = "corridor0";

   diffuseMap[0] = "~/art/textures/section1";
//   normalMap[0] = "~/art/shapes/Soldier/Soldier_n.tga";
//   specularMap[0] = "~/art/shapes/Soldier/Soldier_Spec.dds";

//   diffuseColor[0] = "1 1 1 1";
   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

//------------------------------------------------------------------------------

singleton Material(floortile0)
{
   mapTo = "floortile0";

   diffuseMap[0] = "~/art/textures/floor3_color";
   normalMap[0] = "~/art/textures/floor3_normal";
   specularMap[0] = "~/art/textures/floor3_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(floortile1)
{
   mapTo = "floortile1";

   diffuseMap[0] = "~/art/textures/metal_floor_01_r";
   normalMap[0] = "~/art/textures/metal_floor_01_n";
   specularMap[0] = "~/art/textures/metal_floor_01_s";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(floortile2)
{
   mapTo = "floortile2";

   diffuseMap[0] = "~/art/textures/metal_floor_03_r";
   normalMap[0] = "~/art/textures/metal_floor_03_n";
   specularMap[0] = "~/art/textures/metal_floor_03_s";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

//------------------------------------------------------------------------------

singleton Material(wall0)
{
   mapTo = "wall0";

   diffuseMap[0] = "~/art/textures/panel2_color4";
   normalMap[0] = "~/art/textures/panel2_normal";
   specularMap[0] = "~/art/textures/panel2_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(wall1)
{
   mapTo = "wall1";

   diffuseMap[0] = "~/art/textures/metal_wall_10_r";
   normalMap[0] = "~/art/textures/metal_wall_10_n";
   specularMap[0] = "~/art/textures/metal_wall_10_s";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(wall2)
{
   mapTo = "wall2";

   diffuseMap[0] = "~/art/textures/metal_border_01_r";
   normalMap[0] = "~/art/textures/metal_border_01_n";
   specularMap[0] = "~/art/textures/metal_border_01_s";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};


singleton Material(wall3)
{
   mapTo = "wall3";

   diffuseMap[0] = "~/art/textures/metal_wall_07_d";
   normalMap[0] = "~/art/textures/metal_wall_07_n";
   specularMap[0] = "~/art/textures/metal_wall_07_s";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

//------------------------------------------------------------------------------

singleton Material(rooftile0)
{
   mapTo = "rooftile0";

   diffuseMap[0] = "~/art/textures/wall_plate2_color3";
   normalMap[0] = "~/art/textures/wall_plate2_normal";
   specularMap[0] = "~/art/textures/wall_plate2_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(rooftile1)
{
   mapTo = "rooftile1";

   diffuseMap[0] = "~/art/textures/metal_wall_14_r";
   normalMap[0] = "~/art/textures/metal_wall_14_n";
   specularMap[0] = "~/art/textures/metal_wall_14_s";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

//------------------------------------------------------------------------------

singleton Material(column0)
{
   mapTo = "column0";

   diffuseMap[0] = "~/art/textures/support4_color";
   normalMap[0] = "~/art/textures/support4_normal";
   specularMap[0] = "~/art/textures/support4_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(column1)
{
   mapTo = "column1";

   diffuseMap[0] = "~/art/textures/column_color";
   normalMap[0] = "~/art/textures/column_normal";
   specularMap[0] = "~/art/textures/column_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

//------------------------------------------------------------------------------

singleton Material(metalBase0)
{
   mapTo = "metalBase0";

   diffuseMap[0] = "~/art/textures/metal_base_02_d";
   normalMap[0] = "~/art/textures/metal_base_02_n";
   specularMap[0] = "~/art/textures/metal_base_02_s";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(doorjam0)
{
   mapTo = "doorjam0";

   diffuseMap[0] = "~/art/textures/support_baked";
//   normalMap[0] = "~/art/textures/wall_plate2_normal";
//   specularMap[0] = "~/art/textures/wall_plate2_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(doorframe0)
{
   mapTo = "doorframe0";

   diffuseMap[0] = "~/art/textures/doorFrame_color";
   normalMap[0] = "~/art/textures/doorFrame_normal";
   specularMap[0] = "~/art/textures/doorFrame_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(corridor0)
{
   mapTo = "corridor0";

   diffuseMap[0] = "~/art/textures/groundtex_color";
   normalMap[0] = "~/art/textures/groundtex_normal";
   specularMap[0] = "~/art/textures/groundtex_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(processDeco0)
{
   mapTo = "processDeco0";

   diffuseMap[0] = "~/art/textures/panel4_color";
   normalMap[0] = "~/art/textures/panel_normal";
   specularMap[0] = "~/art/textures/panel_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(processDeco1)
{
   mapTo = "processDeco1";

   diffuseMap[0] = "~/art/textures/wall_plate2_color3";
   normalMap[0] = "~/art/textures/wall_plate2_normal";
   specularMap[0] = "~/art/textures/wall_plate2_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(ventHatch0)
{
   mapTo = "ventHatch0";

   diffuseMap[0] = "~/art/textures/wall_color";
   normalMap[0] = "~/art/textures/wall_normal";
   specularMap[0] = "~/art/textures/wall_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};

singleton Material(column1)
{
   mapTo = "column0";

   diffuseMap[0] = "~/art/textures/column_color";
   normalMap[0] = "~/art/textures/column_normal";
   specularMap[0] = "~/art/textures/column_spec";

   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = 10;

   doubleSided = false;
   translucent = false;
   translucentBlendOp = "None";
};
