using System.Collections.Generic;

public class GOLManager {

    public enum Tile {
        White,
        Black,
        NotATile
    }

    int sizeX;
    int sizeZ;
    Tile[,] tiles;

    public Tile this[int x, int z] {
        get {
            if (x < 0) {
                return Tile.NotATile;
            }
            if (z < 0) {
                return Tile.NotATile;
            }
            if (x >= sizeX) {
                return Tile.NotATile;
            }
            if (z >= sizeZ) {
                return Tile.NotATile;
            }

            return tiles[x, z];

        }
        set {
            tiles[x, z] = value;
        }
    }

    public GOLManager(int x, int z) {
        // this will default to all white
        sizeX = x;
        sizeZ = z;
        tiles = new Tile[x, z];
    }

    public class Change {
        public int x;
        public int z;
        public Tile newState;

        public override string ToString() {
            return "x: " + x + ", z: " + x + ", newState: " + newState;
        }
    }

    // this is the actual game of life
    // returns the changes as a list to make them faster to process
    public List<Change> NextGeneration() {
        List<Change> changes = new List<Change>();

        for (int z = 0; z < sizeZ; z++) {
            for (int x = 0; x < sizeX; x++) {
                int liveCount = CountLivingNeighbors(x,z);

                if (this[x, z] == Tile.Black) {
                    // Any live cell with fewer than two live neighbours dies, as if caused by under-population.
                    // Any live cell with two or three live neighbours lives on to the next generation.
                    // Any live cell with more than three live neighbours dies, as if by over-population.
                    if (liveCount >= 2 && liveCount <= 3) {
                        // only living condition
                        // not a change
                    } else {
                        // die
                        Change c = new Change();
                        c.x = x;
                        c.z = z;
                        c.newState = Tile.White;
                        changes.Add(c);
                    }
                } else {
                    // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                    if (liveCount == 3) {
                        // live
                        Change c = new Change();
                        c.x = x;
                        c.z = z;
                        c.newState = Tile.Black;
                        changes.Add(c);
                    }
                }
            }
        }

        // make sure these changes always happen
        foreach (Change c in changes) {
            this[c.x, c.z] = c.newState;
        }

        return changes;
    }

    public int CountLivingNeighbors(int x, int z) {
        int count = 0;
        for (int dz = -1; dz <= 1; dz++) {
            for (int dx = -1; dx <= 1; dx++) {
                // to thine ownself, skip
                if (dx == 0 && dz == 0) continue;

                if (this[x + dx, z + dz] == Tile.Black) {
                    count++;
                }
            }
        }

        return count;
    }

}
