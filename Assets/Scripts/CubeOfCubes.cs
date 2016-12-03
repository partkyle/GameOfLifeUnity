using UnityEngine;
using System.Collections.Generic;
using System;

public class CubeOfCubes : MonoBehaviour {

    public GameObject tilePrefab;
    public GameObject tileWrapperPrefab;
    public GameObject floorPrefab;
    public Config config;

    float currentGenerationTick;

    GOLManager[] worlds;

    GameObject[][,] worldTiles;

    GameObject container;

    LayerMask tileMask;


    // Use this for initialization
    void Start() {
        tileMask = LayerMask.GetMask("Tiles");
        config = GameObject.FindObjectOfType<Config>() as Config;
        BuildGrid();
    }

    public void BuildGrid() {
        worlds = new GOLManager[6];
        worldTiles = new GameObject[6][,];

        float width = config.tileSize * config.sizeX;
        float height = config.tileSize * config.sizeZ;

        Vector3[] containerPositions = new Vector3[] {
            new Vector3(0,0,0), // left
            new Vector3(width/2f, 0, -width/2f), // front
            new Vector3(width, 0, 0), // right
            new Vector3(width/2f, 0, width/2f), // back
            new Vector3(width/2f, height/2f, 0), // top
            new Vector3(width/2f, -height/2f), // bottom
        };

        Vector3[] containerRotations = new Vector3[] {
            new Vector3(-90, 90, 0), // left
            new Vector3(-90, 0, 0), // front
            new Vector3(-90, -90, 0), // right
            new Vector3(-90, 180, 0), // back
            Vector3.zero, // top
            new Vector3(180, 0, 0),  // bottom
        };

        GameObject theBigWrapper = new GameObject();
        theBigWrapper.name = "The Big Wrapper";
        theBigWrapper.transform.parent = transform;
        theBigWrapper.transform.localPosition = Vector3.zero;

        GameObject[] yawws = new GameObject[6];

        for (int i = 0; i < 6; i++) {
            worlds[i] = new GOLManager(config.sizeX, config.sizeZ);
            worldTiles[i] = new GameObject[config.sizeX, config.sizeZ];

            GameObject yaww = yawws[i] = new GameObject();
            yaww.name = "CenteringTileWrapper " + i;
            yaww.transform.parent = theBigWrapper.transform;
            yaww.transform.position = containerPositions[i];

            container = new GameObject();
            container.name = "TileWrapper_" + i;
            container.transform.parent = yaww.transform;

            GameObject floor = Instantiate(floorPrefab);
            floor.transform.parent = container.transform;
            floor.transform.localPosition = Vector3.zero;
            floor.transform.localScale = new Vector3(width, 1, height);

            container.transform.localPosition = new Vector3(-width / 2f, 0, -height / 2f);

            for (int z = 0; z < config.sizeZ; z++) {
                for (int x = 0; x < config.sizeX; x++) {
                    Vector3 pos = new Vector3(x * config.tileSize, 0, z * config.tileSize);
                    GameObject tile = Instantiate(tilePrefab, container.transform) as GameObject;
                    tile.transform.localPosition = pos;
                    tile.name = "Tile_" + x + "_" + z;

                    Tile tileComponent = tile.GetComponentInChildren<Tile>();
                    tileComponent.x = x;
                    tileComponent.z = z;
                    tileComponent.i = i;

                    MeshRenderer tileRenderer = tile.GetComponentInChildren<MeshRenderer>();

                    if (worlds[i][x, z] == GOLManager.Tile.White) {
                        tileRenderer.enabled = false;
                    } else {
                        tileRenderer.enabled = true;
                    }

                    worldTiles[i][x, z] = tile;
                }
            }

            yaww.transform.eulerAngles = containerRotations[i];
        }
    }

    // Update is called once per frame
    void Update() {
        HandleClick();
        currentGenerationTick += Time.deltaTime;
        if (currentGenerationTick >= config.generationTimer) {
            currentGenerationTick = 0;
            Debug.Log("Generation");

            NextGeneration();
        }
    }

    public void NextGeneration() {

        for (int i = 0; i < 6; i++) {
            List<GOLManager.Change> changes = worlds[i].NextGeneration();

            foreach (GOLManager.Change change in changes) {
                MeshRenderer tileRenderer = worldTiles[i][change.x, change.z].GetComponentInChildren<MeshRenderer>();
                if (change.newState == GOLManager.Tile.White) {
                    tileRenderer.enabled = false;
                } else {
                    tileRenderer.enabled = true;
                }
            }

        }

    }

    void HandleClick() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, tileMask)) {
            Tile tile = hit.transform.GetComponentInChildren<Tile>();
            if (tile != null) {
                if (Input.GetMouseButton(0)) {
                    worlds[tile.i][tile.x, tile.z] = GOLManager.Tile.Black;
                    tile.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
        }
    }
}
