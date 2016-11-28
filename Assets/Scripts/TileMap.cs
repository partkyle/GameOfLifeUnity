using UnityEngine;
using System.Collections;

public class TileMap
{
    int sizeX;
    int sizeY;

    int[,] tiles;

    public TileMap(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        tiles = new int[sizeX, sizeY];
    }

    public void GenerateRandom()
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                tiles[x, y] = Random.Range(0, 2);
            }
        }
    }

    public int GetTileAt(int x , int y)
    {
        return tiles[x, y];
    }


    public void SetTileAt(int x, int y, int tile)
    {
        tiles[x, y] = tile;
    }
}
