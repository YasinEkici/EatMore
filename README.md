# EatMore!
**EatMore!** is a survival game where players must strategically interact with their environment to stay alive. 
The gameplay revolves around collecting baits, using portals, and managing health, all in a fast-paced, single-scene WebGL application.

**Playable Game: https://enesevki.itch.io/eat-more**

## How to Play
Move: Use WASD or Arrow Keys to navigate.
Dash: Press Left Shift to quickly evade or reposition.
Interact: Collect baits and use portals to survive and maximize your score.

## Gameplay Overview
1. Objective: Survive as long as possible by collecting beneficial baits, avoiding harmful ones, and using portals strategically.
2. Health Mechanics: Your health decreases over time. Collect health baits to recover and avoid harmful baits that drain your health further.
3. Portals: Teleport across the map to escape tricky situations and reposition strategically.
4. Different Baits: The different baits have different effects on player:
   1. The Yellow Bait: Called "GoodBait". Heals the Player upon collecting.
   2. The Red Bait: Called "BadBait". Damages the Player upon collecting.
   3. The Purple Bait: Called "SpeedBait". Gives movement speed for a time upon collecting.

## Aksiyon Listesi
### Yasin Ekici - 21360859029
1. Spawning baits Spawn_Manager: 50-71
2. Destroying baits: Bait.cs 140-148
3. Increasing player's health upon bait interaction Player.cs: 128-134
4. Decreasing player's health upon bait interaction Player.cs: 137-144
5. Increasing player's speed upon bait interaction Player.cs 147-166
6. Player-Bait Collision Handling Bait.cs 100-134
7. Bait movement Bait.cs 83-97
8. Score tracking Bait.cs 37-40
9. Adjusting bait positions upon spawning SpawnManager.cs: 110-122

### Enes Şevki Dönmez - 21360859079
1. Player movement Player.cs: 67-98
2. Player Dash Player.cs 36-62
3. Destroying Player Player.cs: 170-178
4. Player's health decreasing over time Player.cs: 119-126
5. Spawning portals Spawn_Manager: 74-107
6. Destroying portals Portal.cs: 27-37
7. Player-Portal Collision Handling, Teleporting Player: Portal.cs 14-31
8. Health Bar and Score UI
  1. Health Bar healthBar.cs
  2. Score Score.cs
9. Wall interactions using Raycast
  1. Player Player.cs: 101-117
  2. Bait Bait.cs: 64-80


## References
Hearth asset: https://assetstore.unity.com/packages/2d/gui/icons/heart-in-pixel-287862
