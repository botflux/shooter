# shooter
A shoter game

## To-do list:

- Spawner accessing MapGenerator scripts (only GameManager should access and set informations after to the spawner)
- Events listener as OnPlayerDeath should be in GameManager class and not in Spawner. At least OnEnemyDeath must be in Spawner because it is only use in the class

## Set-up scene and params

### Set-up Player

- Drag the Player prefab from the prefab folder to the scene.
- Drag the Crosshair prefab from the prefab folder to the Crosshair field of Player script.
- Drag a gun from prefab/guns folder into the Starting Gun fild of Gun Controller script.

If you want to try Player speed and stuff, you can use it in a simple plane gameobject.


