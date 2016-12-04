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

                            adjacencyList[i, z, x].Add(new Coord { i = i, z = z + dz, x = x + dx });
                        }
                    }
                }
            }
        }

        // handle the horizontal faces laterally
        for (int z = 0; z < sizeZ; z++) {
            for (int dz = -1; dz <= 1; dz++) {
                int coordz = z + dz;
                if (coordz < 0 || coordz >= sizeZ) continue;

                {
                    int left = 0;
                    int right = 2;
                    adjacencyList[left, z, sizeX - 1].Add(new Coord { i = right, x = 0, z = coordz });
                    adjacencyList[right, z, 0].Add(new Coord { i = left, x = sizeX - 1, z = coordz });
                }
                {
                    int left = 2;
                    int right = 4;
                    adjacencyList[left, z, sizeX - 1].Add(new Coord { i = right, x = 0, z = coordz });
                    adjacencyList[right, z, 0].Add(new Coord { i = left, x = sizeX - 1, z = coordz });
                }
                {
                    int left = 4;
                    int right = 5;
                    adjacencyList[left, z, sizeX - 1].Add(new Coord { i = right, x = 0, z = coordz });
                    adjacencyList[right, z, 0].Add(new Coord { i = left, x = sizeX - 1, z = coordz });
                }
                {
                    int left = 5;
                    int right = 0;
                    adjacencyList[left, z, sizeX - 1].Add(new Coord { i = right, x = 0, z = coordz });
                    adjacencyList[right, z, 0].Add(new Coord { i = left, x = sizeX - 1, z = coordz });
                }
            }
        }

        for (int x = 0; x < sizeX; x++) {
            for (int dx = -1; dx <= 1; dx++) {
                int coordx = x + dx;
                if (coordx < 0 || coordx >= sizeX) continue;

                {
                    int top = 1;
                    int bottom = 2;
                    adjacencyList[top, 0, x].Add(new Coord { i = bottom, x = coordx, z = sizeZ - 1 });
                    adjacencyList[bottom, sizeZ - 1, x].Add(new Coord { i = top, x = coordx, z = 0 });
                }
                {
                    int top = 2;
                    int bottom = 3;
                    adjacencyList[top, 0, x].Add(new Coord { i = bottom, x = coordx, z = sizeZ - 1 });
                    adjacencyList[bottom, sizeZ - 1, x].Add(new Coord { i = top, x = coordx, z = 0 });
                }
            }
        }
    }
}
