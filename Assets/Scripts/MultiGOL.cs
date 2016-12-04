using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiGOL {

    public int sizeX;
    public int sizeZ;
    public int sizeI;

    AdjancencyGOL adjacencyGOL;

    public TileStatus[][,] tiles;

    List<Change>[] nextGenerations;

    public MultiGOL(int sizeX, int sizeZ, int sizeI) {
        this.sizeX = sizeX;
        this.sizeZ = sizeZ;
        this.sizeI = sizeI;

        adjacencyGOL = new AdjancencyGOL(sizeX, sizeZ, sizeI);
        adjacencyGOL.Build();

        tiles = new TileStatus[sizeI][,];
        nextGenerations = new List<Change>[sizeI];
        for (int i = 0; i < sizeI; i++) {
            tiles[i] = new TileStatus[sizeX, sizeZ];
            nextGenerations[i] = new List<Change>();
        }
    }

    public void PrecalculateGeneration() {
        for (int i = 0; i < sizeI; i++) {
            // reset the generation
            nextGenerations[i].Clear();

            List<Change> currentGenerationChanges = nextGenerations[i];
            // This is the game of life
            for (int z = 0; z < sizeZ; z++) {
                for (int x = 0; x < sizeX; x++) {
                    int count = 0;
                    // count neighbors
                    foreach (Coord coord in adjacencyGOL.adjacencyList[i, z, x]) {
                        if (tiles[coord.i][coord.x, coord.z] == TileStatus.Black) {
                            count++;
                        }
                    }

                    if (i == 1 && x == 7 && z == 7) {
                        Debug.Log(count);
                    }

                    // Any live cell with fewer than two live neighbours dies, as if caused by under-population.
                    // Any live cell with two or three live neighbours lives on to the next generation.
                    // Any live cell with more than three live neighbours dies, as if by over-population.
                    if (tiles[i][x, z] == TileStatus.Black) {
                        if (count < 2 || count > 3) {
                            // DIE
                            Change c = new Change() {
                                x = x,
                                z = z,
                                newState = TileStatus.White
                            };
                            currentGenerationChanges.Add(c);
                        }
                    } else {
                        // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                        if (count == 3) {
                            // REPRODUCE
                            Change c = new Change() {
                                x = x,
                                z = z,
                                newState = TileStatus.Black,
                            };
                            currentGenerationChanges.Add(c);
                        }
                    }
                }
            }
        }
    }

    // return the generation and commit the changes
    public Change[] NextGeneration(int i) {
        Change[] result = new Change[nextGenerations[i].Count];
        nextGenerations[i].CopyTo(result);

        foreach (Change c in nextGenerations[i]) {
            tiles[i][c.x, c.z] = c.newState;
        }

        return result;
    }
}
