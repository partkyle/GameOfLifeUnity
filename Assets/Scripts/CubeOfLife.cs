using UnityEngine;
using System.Collections.Generic;
using System;

public class CubeOfLife : MonoBehaviour {

    public GameObject tilePrefab;
    public Material whiteMaterial;
    public Material blackMaterial;

    public int sizeX = 4;
    public int sizeZ = 2;
    public float tileSize = 1.0f;
    public float generationTimer = 1.0f;

    float currentGenerationTick;

    GOLManager gol;

    GameObject[,] tiles;

    LayerMask tileMask;


    // Use this for initialization
    void Start() {
        tileMask = LayerMask.GetMask("Tiles");
        BuildGrid();
    }

    public void BuildGrid() {
        gol = new GOLManager(sizeX, sizeZ);
        tiles = new GameObject[sizeX, sizeZ];

        gol[1, 4] = TileStatus.Black;

        for (int z = 0; z < sizeZ; z++) {
            for (int x = 0; x < sizeX; x++) {
                Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform) as GameObject;
                tile.name = "Tile_" + x + "_" + z;

                Tile tileComponent = tile.GetComponentInChildren<Tile>();
                tileComponent.x = x;
                tileComponent.z = z;

                MeshRenderer tileRenderer = tile.GetComponentInChildren<MeshRenderer>();
                tileRenderer.material = blackMaterial;

                if (gol[x, z] == TileStatus.White) {
                    //tileRenderer.material = whiteMaterial;
                    tileRenderer.enabled = false;
                } else {
                    //tileRenderer.material = blakMaterial;
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
        if (currentGenerationTick >= generationTimer) {
            currentGenerationTick = 0;
            Debug.Log("Generation");

            NextGeneration();
        }
    }

    public void NextGeneration() {

        List<GOLManager.Change> changes = gol.NextGeneration();

        foreach (GOLManager.Change change in changes) {
            MeshRenderer tileRenderer = tiles[change.x, change.z].GetComponentInChildren<MeshRenderer>();
            if (change.newState == TileStatus.White) {
                //tileRenderer.material = whiteMaterial;
                tileRenderer.enabled = false;
            } else {
                tileRenderer.material = blackMaterial;
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
                    gol[tile.x, tile.z] = TileStatus.Black;
                    //tile.gameObject.GetComponentInChildren<MeshRenderer>().material = blackMaterial;
                    tile.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
        }
    }
}
