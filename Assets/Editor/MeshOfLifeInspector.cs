using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(MeshOfLife))]
public class MeshOfLifeInspector : Editor {

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        if (GUILayout.Button("NextGeneration")) {
            MeshOfLife mol = (MeshOfLife)target;
            mol.NextGeneration();
        }
    }
}
