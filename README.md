# Project-Arachnid
New 3rd person prototype with enemy AI (scriptable objects &amp; state machines). #MadeWithUnity

Enemy AI detects Player via two methods - Radius (now commented out in the script) and line of sight (field of view detection) which seemed a more realistic way of detection.

A baked NavMesh has been implemented to match the size of the spider meshes by width and the line of sight to the player is broken through walls. 

Each enemy is a scriptable object which allows creation of new types of enemies further down the line. The attributes are fed into the enemy canvas to display the name and health levels.

A dedicated animation state machine aligns with the 4 main states of the AI controller script - patrolling (walking), chasing (running), attacking & death.

Patrolling is determined by an array of waypoints in which a random array index will determine the AI NavAgent's next destination.



![](docs/scrn1.png)

![](docs/scrn2.png)


Line of sight detection FOV gizmos.
![](docs/scrn3.png)

Array of waypoints laid out across the level.
![](docs/scrn4.png)


<a href="./docs/RadiusVideo.mp4">Play Radius Video</a>

<a href="./docs/AttackVideo.mp4">Play Attack Video</a>