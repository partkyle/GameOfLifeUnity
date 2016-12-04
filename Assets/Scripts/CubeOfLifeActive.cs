using UnityEngine;
using System.Collections.Generic;
using System;

public class CubeOfLifeActive : MonoBehaviour {

    public GameObject tilePrefab;
    public GameObject selectionTilePrefab;
    public GameObject floorPrefab;

    public int sizeX = 4;
    public int sizeZ = 2;

    public float tileSize = 1.0f;
    public float generationTimer = 1.0f;

    float currentGenerationTick;

    GOLManager gol;

    GameObject[,] tiles;
    GameObject selectionTile;

    GameObject floor;
    MeshCollider floorCollider;

    // Use this for initialization
    void Start() {
        floor = Instantiate(floorPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
        floorCollider = floor.GetComponentInChildren<MeshCollider>();

        selectionTile = Instantiate(selectionTilePrefab, transform) as GameObject;
        selectionTile.SetActive(false);

        BuildGrid();
    }

    public void BuildGrid() {
        gol = new GOLManager(sizeX, sizeZ);
        tiles = new GameObject[sizeX, sizeZ];

        // we know the size of the grid, so let's make the floor fit perfect
        float width = sizeX * tileSize;
        float height = sizeZ * tileSize;

        floor.transform.localScale = new Vector3(width, 0, height);

        for (int z = 0; z < sizeZ; z++) {
            for (int x = 0; x < sizeX; x++) {
                Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform) as GameObject;
                tile.name = "Tile_" + x + "_" + z;

                Tile tileComponent = tile.GetComponentInChildren<Tile>();
                tileComponent.x = x;
                tileComponent.z = z;

                if (gol[x, z] == TileStatus.White) {
                    tile.SetActive(false);
                } else {
                    tile.SetActive(true);
                }

                tile.SetActive(false);

                tiles[x, z] = tile;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        HandleClick();
        currentGenerationTick += Time.deltaTime;
        if (currentGenerationTick >= generationTimer) {
            currentGenerationTick = 0;

            NextGeneration();
        }
    }

    public void NextGeneration() {

        List<Change> changes = gol.NextGeneration();

        foreach (Change change in changes) {
            int x = change.x;
            int z = change.z;

            if (change.newState == TileStatus.White) {
                tiles[x, z].SetActive(false);
            } else {
                tiles[x, z].SetActive(true);
            }
        }
    }

    void HandleClick() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (floorCollider.Raycast(ray, out hit, 1000f)) {
            int x = (int)(hit.point.x / tileSize);
            int z = (int)(hit.point.z / tileSize);
            Debug.Log("x: " + x + ", z: " + z);
            selectionTile.transform.position = new Vector3(x * tileSize, 0, z * tileSize);
            selectionTile.SetActive(true);

            if (Input.GetMouseButton(0)) {
                tiles[x, z].SetActive(true);
                gol[x, z] = TileStatus.Black;
            }
        } else {
            selectionTile.SetActive(false);
        }
    }
}
