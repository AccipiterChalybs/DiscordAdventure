
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WorldBuilderScript))]
public class EditorWorldBuild : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WorldBuilderScript myScript = (WorldBuilderScript)target;
        if (GUILayout.Button("Build World"))
        {
            myScript.BuildWorld();
        }
    }
}