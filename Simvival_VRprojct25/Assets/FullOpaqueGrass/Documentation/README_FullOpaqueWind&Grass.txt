Full Opaque Grass Physics & Wind - README  
--------------------------------------------------------------  

Overview  
--------------------------------------------------------------  
The Full Opaque Grass Physics & Wind package is a lightweight, stylized grass system designed for mobile, VR, and stylized projects.  
It delivers high-performance, fully opaque grass effects optimized for Unity's Universal Render Pipeline (URP).  
This pack includes dynamic grass physics with wind simulation, mesh placement tools, ambient grass particles, and a demo scene.  

--------------------------------------------------------------  
Dynamic Grass Physics (Beta)  
--------------------------------------------------------------  
A physics-based interaction system is included to simulate grass reacting to moving targets (e.g., a player), creating trails in the grass as they move.  

Three shader options are available:  
- S_Vegetation_Interactive (with VFX_TargetPosition_5Point)  
  High-quality trail system using 5 points for smooth interaction.  

- S_Vegetation_Interactive_Simple (with VFX_TargetPosition_2Point)  
  Optimized version using 2 points for lower performance cost.  

- S_Vegetation  
  Lightweight version without any physics, ideal for distant grass or simpler setups.  

To use:  
1. Add the desired VFX_TargetPosition script (2Point or 5Point) to your player or moving object.  
2. Assign your player (or moving target) in the **Target** field in the script inspector. Limit to 1 Target.  
3. Apply the corresponding vegetation shader (Interactive or Simple) to your grass material.  

⚠️ This system is in beta. If you encounter issues, feel free to contact me.  

--------------------------------------------------------------  
Mesh Display Tool  
--------------------------------------------------------------  
The Mesh Display Tool allows you to manually paint grass directly on 3D mesh surfaces.  

How to Use:  
1. Select your mesh in the scene.  
2. Add the DisplayGrass Script  
3. Select the SM_Grass Prefab and adjust parameters such as density, size, and randomness.  
4. Generate.  

--------------------------------------------------------------  
Terrain Detail Mesh Setup (Unity 2022)  
--------------------------------------------------------------  
You can also use the grass prefab as a **detail mesh** on Unity terrains for precise placement.  

How to Add a Grass Detail Mesh to a Terrain in Unity 2022:  
1. Select your terrain object in the scene.  
2. In the Terrain Inspector, go to the **Paint Details** tab.  
3. Click on **Edit Details** → **Add Detail Mesh**.  
4. In the popup window:  
   - Assign the SM_Grass prefab as the Detail mesh.  
   - Set **Render Mode** to **Vertex Lit** or **Grass** depending on your desired style.  
   - Adjust settings like Min/Max Width & Height, and Noise Spread.  
5. Make sure to check the option **Align to Terrain** — this is important, as it ensures the grass follows the slope of the terrain.  
   The grass shader automatically adjusts color based on slope for a more natural visual effect.  
6. Use the terrain brush to paint grass exactly where you want it.  

--------------------------------------------------------------  
Wind Manager  
--------------------------------------------------------------  
The WindManager controls global wind direction and strength affecting all grass elements and VFX (like fireflies and floating grass).  

Customizable Parameters:  
- Wind Direction (Vector2): Controls wind direction on X and Y axes.  
- Wind Size (Float): Controls the size of the wind waves.  
- Wind Strength (Float): Determines how strongly grass is pushed.  
- Wind Speed (Float): Controls the speed at which wind moves.  

--------------------------------------------------------------  
Grass & Firefly VFX  
--------------------------------------------------------------  
Two ambient VFX systems are included in the package:  
- **Grass Particles VFX**  
- **Firefly Particles VFX**  

To use them, simply **drag and drop** the VFX prefab into your scene and **attach it to your player** or any moving object.  
These VFX are synchronized with the Wind Manager for natural ambient movement.  

--------------------------------------------------------------  
Copyright Notice  
--------------------------------------------------------------  
Copyright Roman Chacornac 2025  
All rights reserved. This product is protected by copyright and may not be distributed, modified, or resold without the prior permission of the author. Any unauthorized use is prohibited.  

--------------------------------------------------------------  
Support  
--------------------------------------------------------------  
If you encounter any issues or need assistance, feel free to reach out to us at:  
Email: optifx.fr@gmail.com  
--------------------------------------------------------------  
