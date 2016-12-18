using UnityEngine;
using System.Collections;

[System.Serializable]
public class Config : MonoBehaviour {
    public int sizeX = 4;
    public int sizeZ = 2;
    public float tileSize = 1.0f;

    public bool paused { get; set; }
    public float generationTimer { get; set; }
}
