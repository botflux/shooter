using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator map = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            map.GenerateMap();
            map.BuildNavMesh();
        }

        if (GUILayout.Button("Generate map"))
        {
            map.GenerateMap();
            map.BuildNavMesh();
        }

    }
}
