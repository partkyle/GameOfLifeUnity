using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(CubeOfLife))]
public class CubeoFlifeInspector : Editor {

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        if (GUILayout.Button("Regenerate")) {
            CubeOfLife tileMap = (CubeOfLife)target;
            tileMap.BuildGrid();
        }

        if (GUILayout.Button("Next Generation")) {
            CubeOfLife tileMap = (CubeOfLife)target;
            tileMap.NextGeneration();
        }
    }
}
