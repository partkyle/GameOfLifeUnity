GameOfLifeUnity
===============

A game of life implementation in unity.

## Cube Of Cubes

[scene](Assets/Scenes/CubeOfCubes.unity) 

This is Game of Life, but on all of the cube faces. 

Note: This has interesting behavior around the corners of the cube, because there are only 7 neighbors, so it is not a true Game of Life, but it is was an interesting experiment.

Scene Objects:

### Main Camera: [script](Assets/Scripts/MouseOrbit.cs)

A camera controller that allow for panning with WASD and rotation using the right mouse button.

### Config: [script](Assets/Scripts/Config.cs)

A script for configuring the amount of tile and the size of the tiles to render.

It also handles pausing and the time scale. These values are set in the Canvas items and set values on this script.

### CubeOfCubes: [script](Assets/Scripts/CubeOfCubes.cs)

This is the main script for this scene. This is where the CubeOfCubes is built based on the Config. This has the Update that runs the next generations and also handles the left click for placing down a tile.

It uses 2 helper scripts:

- [AdjancencyGOL](Assets/Scripts/AdjancencyGOL.cs)

Manages the adjacency lists for all tiles, including the wrap around of the cube faces.

- [MultiGOL](Assets/Scripts/MultiGOL.cs)

Manages multiple GOL boards and executes the next generation of each one. Stores a list of changes to apply to a running game.
