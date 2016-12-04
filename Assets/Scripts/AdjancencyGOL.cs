using UnityEngine;
using System.Collections.Generic;

public class AdjancencyGOL {

    public int sizeX;
    public int sizeZ;
    public int sizeI;

    public List<Coord>[,,] adjacencyList;

    public AdjancencyGOL(int sizeX, int sizeZ, int sizeI) {
        this.sizeX = sizeX;
        this.sizeZ = sizeZ;
        this.sizeI = sizeI;
    }

    public void Build() {
        adjacencyList = new List<Coord>[sizeI, sizeZ, sizeX];

        for (int i = 0; i < sizeI; i++) {
            for (int z = 0; z < sizeZ; z++) {
                for (int x = 0; x < sizeX; x++) {
                    adjacencyList[i, z, x] = new List<Coord>();

                    for (int dz = -1; dz <= 1; dz++) {
                        for (int dx = -1; dx <= 1; dx++) {
                            if (dx == 0 && dz == 0) continue;

                            if (x + dx < 0 || x + dx >= sizeX) continue;
                            if (z + dz < 0 || z + dz >= sizeZ) continue;

                            adjacencyList[i, z, x].Add(new Coord { i = i, z = z+dz, x = x+dx });
                        }
                    }
                }
            }
        }
    }
}
