using UnityEngine;
using System.Collections.Generic;
using System;

public class CubeOfCubes : MonoBehaviour {

    public GameObject tilePrefab;
    public GameObject tileWrapperPrefab;
    public GameObject floorPrefab;
    public Config config;

    float currentGenerationTick;

    //GOLManager[] worlds;

    GameObject[][,] worldTiles;

    GameObject container;

    LayerMask tileMask;

    MultiGOL multiGOL;

    int faces = 6;

    // Use this for initialization
    void Start() {
        tileMask = LayerMask.GetMask("Tiles");
        config = GameObject.FindObjectOfType<Config>() as Config;
        multiGOL = new MultiGOL(config.sizeX, config.sizeZ, faces);

        //// testing a boat
        //multiGOL.tiles[1][2, 4] = TileStatus.Black;
        //multiGOL.tiles[1][2, 5] = TileStatus.Black;
        //multiGOL.tiles[1][2, 6] = TileStatus.Black;
        //multiGOL.tiles[1][2, 6] = TileStatus.Black;
        //multiGOL.tiles[1][3, 3] = TileStatus.Black;
        //multiGOL.tiles[1][3, 6] = TileStatus.Black;
        //multiGOL.tiles[1][4, 4] = TileStatus.Black;
        //multiGOL.tiles[1][4, 6] = TileStatus.Black;
        //multiGOL.tiles[1][5, 5] = TileStatus.Black;

        // solid flower for testing
        multiGOL.tiles[2][config.sizeX - 1, 5] = TileStatus.Black;
        multiGOL.tiles[2][config.sizeX - 1, 6] = TileStatus.Black;
        multiGOL.tiles[4][0, 4] = TileStatus.Black;
        multiGOL.tiles[4][1, 5] = TileStatus.Black;
        multiGOL.tiles[4][1, 6] = TileStatus.Black;
        multiGOL.tiles[4][0, 7] = TileStatus.Black;

        // solid flower for testing
        multiGOL.tiles[4][config.sizeX - 1, 5] = TileStatus.Black;
        multiGOL.tiles[4][config.sizeX - 1, 6] = TileStatus.Black;
        multiGOL.tiles[5][0, 4] = TileStatus.Black;
        multiGOL.tiles[5][1, 5] = TileStatus.Black;
        multiGOL.tiles[5][1, 6] = TileStatus.Black;
        multiGOL.tiles[5][0, 7] = TileStatus.Black;

        // solid flower for testing
        multiGOL.tiles[0][config.sizeX - 1, 5] = TileStatus.Black;
        multiGOL.tiles[0][config.sizeX - 1, 6] = TileStatus.Black;
        multiGOL.tiles[2][0, 4] = TileStatus.Black;
        multiGOL.tiles[2][1, 5] = TileStatus.Black;
        multiGOL.tiles[2][1, 6] = TileStatus.Black;
        multiGOL.tiles[2][0, 7] = TileStatus.Black;

        // solid flower for testing
        multiGOL.tiles[5][config.sizeX - 1, 5] = TileStatus.Black;
        multiGOL.tiles[5][config.sizeX - 1, 6] = TileStatus.Black;
        multiGOL.tiles[0][0, 4] = TileStatus.Black;
        multiGOL.tiles[0][1, 5] = TileStatus.Black;
        multiGOL.tiles[0][1, 6] = TileStatus.Black;
        multiGOL.tiles[0][0, 7] = TileStatus.Black;

        // solid flower for testing
        multiGOL.tiles[1][3, 0] = TileStatus.Black;
        multiGOL.tiles[1][4, 1] = TileStatus.Black;
        multiGOL.tiles[1][5, 0] = TileStatus.Black;
        multiGOL.tiles[2][3, config.sizeZ - 1] = TileStatus.Black;
        multiGOL.tiles[2][4, config.sizeZ - 2] = TileStatus.Black;
        multiGOL.tiles[2][5, config.sizeZ - 1] = TileStatus.Black;


        // solid flower for testing
        multiGOL.tiles[2][3, 0] = TileStatus.Black;
        multiGOL.tiles[2][4, 1] = TileStatus.Black;
        multiGOL.tiles[2][5, 0] = TileStatus.Black;
        multiGOL.tiles[3][3, config.sizeZ - 1] = TileStatus.Black;
        multiGOL.tiles[3][4, config.sizeZ - 2] = TileStatus.Black;
        multiGOL.tiles[3][5, config.sizeZ - 1] = TileStatus.Black;
        

        BuildGrid();
    }

    public void BuildGrid() {
        worldTiles = new GameObject[6][,];

        float width = config.tileSize * config.sizeX;
        float height = config.tileSize * config.sizeZ;

        Vector3[] containerPositions = new Vector3[] {
            new Vector3(0,0,0), // left
            new Vector3(width/2f, height/2f, 0), // top
            new Vector3(width/2f, 0, -width/2f), // front
            new Vector3(width/2f, -height/2f), // bottom
            new Vector3(width, 0, 0), // right
            new Vector3(width/2f, 0, width/2f), // back
        };

        Vector3[] containerRotations = new Vector3[] {
            new Vector3(-90, 90, 0), // left
            Vector3.zero, // top
            new Vector3(-90, 0, 0), // front
            new Vector3(180, 0, 0),  // bottom
            new Vector3(-90, -90, 0), // right
            new Vector3(-90, 180, 0), // back
        };

        GameObject theBigWrapper = new GameObject();
        theBigWrapper.name = "The Big Wrapper";
        theBigWrapper.transform.parent = transform;
        theBigWrapper.transform.localPosition = Vector3.zero;

        GameObject[] yawws = new GameObject[6];

        for (int i = 0; i < 6; i++) {
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
                    tileComponent.Adjacency = multiGOL.adjacencyGOL.adjacencyList[i, z, x];

                    MeshRenderer tileRenderer = tile.GetComponentInChildren<MeshRenderer>();

                    if (multiGOL.tiles[i][x, z] == TileStatus.White) {
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

            multiGOL.PrecalculateGeneration();

            NextGeneration();
        }
    }

    public void NextGeneration() {

        for (int i = 0; i < faces; i++) {
            foreach (Change change in multiGOL.NextGeneration(i)) {
                MeshRenderer tileRenderer = worldTiles[i][change.x, change.z].GetComponentInChildren<MeshRenderer>();
                if (change.newState == TileStatus.White) {
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
                    multiGOL.tiles[tile.i][tile.x, tile.z] = TileStatus.Black;
                    tile.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
        }
    }
}
