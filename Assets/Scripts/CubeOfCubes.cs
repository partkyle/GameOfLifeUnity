using UnityEngine;
using System.Collections.Generic;
using System;

public class CubeOfCubes : MonoBehaviour {

    public GameObject tilePrefab;
    public GameObject tileWrapperPrefab;
    public Config config;

    float currentGenerationTick;

    GOLManager gol;

    GameObject[,] tiles;

    GameObject container;

    LayerMask tileMask;


    // Use this for initialization
    void Start() {
        tileMask = LayerMask.GetMask("Tiles");
        config = GameObject.FindObjectOfType<Config>() as Config;
        BuildGrid();
    }

    public void BuildGrid() {
        gol = new GOLManager(config.sizeX, config.sizeZ);
        tiles = new GameObject[config.sizeX, config.sizeZ];
        container = Instantiate(tileWrapperPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;

        for (int z = 0; z < config.sizeZ; z++) {
            for (int x = 0; x < config.sizeX; x++) {
                Vector3 pos = new Vector3(x * config.tileSize, 0, z * config.tileSize);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, container.transform) as GameObject;
                tile.name = "Tile_" + x + "_" + z;

                Tile tileComponent = tile.GetComponentInChildren<Tile>();
                tileComponent.x = x;
                tileComponent.z = z;

                MeshRenderer tileRenderer = tile.GetComponentInChildren<MeshRenderer>();

                if (gol[x, z] == GOLManager.Tile.White) {
                    tileRenderer.enabled = false;
                } else {
                    tileRenderer.enabled = true;
                }

                tiles[x, z] = tile;
            }
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

        List<GOLManager.Change> changes = gol.NextGeneration();

        foreach (GOLManager.Change change in changes) {
            MeshRenderer tileRenderer = tiles[change.x, change.z].GetComponentInChildren<MeshRenderer>();
            if (change.newState == GOLManager.Tile.White) {
                tileRenderer.enabled = false;
            } else {
                tileRenderer.enabled = true;
            }
        }
    }

    void HandleClick() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, tileMask)) {
            Tile tile = hit.transform.GetComponentInChildren<Tile>();
            if (tile != null) {
                if (Input.GetMouseButton(0)) {
                    gol[tile.x, tile.z] = GOLManager.Tile.Black;
                    tile.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
        }
    }
}
