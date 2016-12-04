using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MeshOfLife : MonoBehaviour {

    public int sizeX = 4;
    public int sizeZ = 2;

    public float tileSize = 5f;


    public float textureTileSize = 16f;


    public bool paused = false;
    public float generationTimer = 1f;


    MeshCollider mc;
    MeshFilter mf;

    GOLManager gol;

    float ticker = 0f;

    // texture helpers
    float low = 0.1f;
    float lowMid = 0.4f;
    float highMid = 0.6f;
    float high = 0.9f;

    // Use this for initialization
    void Start() {
        mc = GetComponent<MeshCollider>();
        mf = GetComponent<MeshFilter>();

        gol = new GOLManager(sizeX, sizeZ);

        BuildMesh();
    }

    // Update is called once per frame
    void Update() {
        HandleClick();

        if (!paused) {
            ticker += Time.deltaTime;
            if (ticker >= generationTimer) {
                ticker = 0;
                NextGeneration();
            }
        }
    }

    public void NextGeneration() {
        int start = System.Environment.TickCount;
        List<Change> changes = gol.NextGeneration();

        Debug.Log("Calculating generation " + (System.Environment.TickCount - start) + "ms");

        foreach (Change change in changes) {
            DrawSpot(change.x, change.z, change.newState);
        }

        Debug.Log("Updating mesh " + (System.Environment.TickCount - start) + "ms");


        Debug.Log("NextGeneration took " + (System.Environment.TickCount - start) + "ms");
    }

    private void HandleClick() {
        if (Input.GetMouseButton(0)) {
            Debug.Log("click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (mc.Raycast(ray, out hit, 1000f)) {
                int x = (int)(hit.point.x / tileSize);
                int z = (int)(hit.point.z / tileSize);
                Debug.Log("x: " + x + ", z: " + z);
                gol[x, z] = TileStatus.Black;
                DrawSpot(x, z, TileStatus.Black);
            }
        }
    }

    void DrawSpot(int x, int z, TileStatus color) {
        Vector2[] uv = mf.mesh.uv;
        int cellNum = sizeX * z + x;

        // create the 4 vectors that make this shape
        int vertexStart = cellNum * 4;


        if (color == TileStatus.White) {
            uv[vertexStart + 0] = new Vector2(low, low);
            uv[vertexStart + 1] = new Vector2(low, lowMid);
            uv[vertexStart + 2] = new Vector2(lowMid, low);
            uv[vertexStart + 3] = new Vector2(lowMid, lowMid);
        } else {
            uv[vertexStart + 0] = new Vector2(highMid, highMid);
            uv[vertexStart + 1] = new Vector2(highMid, high);
            uv[vertexStart + 2] = new Vector2(high, highMid);
            uv[vertexStart + 3] = new Vector2(high, high);
        }

        mf.mesh.uv = uv;
    }

    public void BuildMesh() {
        int vertexPerRow = sizeX * 2;
        int vertexPerCol = sizeZ * 2;

        Vector3[] vertices = new Vector3[vertexPerRow * vertexPerCol];
        Vector3[] normals = new Vector3[vertices.Length];
        int[] triangles = new int[sizeX * sizeZ * 2 * 3];
        Vector2[] uv = new Vector2[vertices.Length];


        for (int z = 0; z < sizeZ; z++) {
            //blackblackblackWhiteWhite++;
            for (int x = 0; x < sizeX; x++) {
                int cellNum = sizeX * z + x;

                // create the 4 vectors that make this shape
                int vertexStart = cellNum * 4;
                vertices[vertexStart + 0] = new Vector3(x * tileSize, 0, z * tileSize);
                vertices[vertexStart + 1] = new Vector3(x * tileSize + tileSize, 0, z * tileSize);
                vertices[vertexStart + 2] = new Vector3(x * tileSize, 0, z * tileSize + tileSize);
                vertices[vertexStart + 3] = new Vector3(x * tileSize + tileSize, 0, z * tileSize + tileSize);

                // set normals
                normals[vertexStart + 0] = Vector3.up;
                normals[vertexStart + 1] = Vector3.up;
                normals[vertexStart + 2] = Vector3.up;
                normals[vertexStart + 3] = Vector3.up;

                // build the triangles
                int triangleStart = cellNum * 2 * 3;
                triangles[triangleStart + 0] = vertexStart + 0;
                triangles[triangleStart + 1] = vertexStart + 3;
                triangles[triangleStart + 2] = vertexStart + 1;

                triangles[triangleStart + 3] = vertexStart + 0;
                triangles[triangleStart + 4] = vertexStart + 2;
                triangles[triangleStart + 5] = vertexStart + 3;

                // build the uv

                //if (blackblackblackWhiteWhite++ % 2 == 0) {
                uv[vertexStart + 0] = new Vector2(low, low);
                uv[vertexStart + 1] = new Vector2(low, lowMid);
                uv[vertexStart + 2] = new Vector2(lowMid, low);
                uv[vertexStart + 3] = new Vector2(lowMid, lowMid);
                //} else {
                //    uv[vertexStart + 0] = new Vector2(.5f, .5f);
                //    uv[vertexStart + 1] = new Vector2(.5f, 1f);
                //    uv[vertexStart + 2] = new Vector2(1f, .5f);
                //    uv[vertexStart + 3] = new Vector2(1f, 1f);
                //}
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "Generated Mesh";
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mf.mesh = mesh;
        mc.sharedMesh = mesh;
    }
}
