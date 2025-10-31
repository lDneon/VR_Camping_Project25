## VR_Camping_Project25
Immersive camping survival game. The player can cook and defend themselves, avoid or fight the bear and gather wood to keep a campfire burning 

### Mechanics
- **Campfire & Cooking System-** fuel,heat propagation, temp-based cooking. Food changes color from; raw->cooked->burnt based on temp
- **Combat System & Bear Behavior-**  player weapons deal damage using IDamageable(physics based melee). Using field view(already a VR hardware based-optic) + raycasting(used to detect colission) so that the bear can detect the player. WHen that happen, the bear can chase and attack the player. If the bear dies in the proccess then it drops meat for the player to collect.
- **Health System-** player HP, healing with First Aid medicine bottle
- **Evironmental Simulation-** fire drains fuel over time; logs restore it
  ---

### Dynamics
- **Resource Manangement-** fuel must be managed punctuated by sudden bear attacks
- **Risk Vs Reward-** approaching the fire to cook food exposes the player to potential danger
- **learning Curve-** player learns ideal cooking times and tempatures from visual feedback
- **EmergentFlow-** balancing exploration, combat, and survival without traditional UI

### Aesthetics
- **Sensation-** warmth and cozy fire light in VR
- **Challenge-** balancing survival with limited resources
- **Discovery-** experimenting with cooking, exploring the campsite
- **Fantasy-** experiencing self-reliant wilderness life in VR
- **flow-** relaxation mixed with bursts of intensity 
