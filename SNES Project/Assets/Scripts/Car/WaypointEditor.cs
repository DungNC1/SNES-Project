using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoints))]
public class WaypointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Waypoints script = (Waypoints)target;

        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Angle Size Checkpoint Walls") == true)
        {
            script.AngleSizeCheckpointWalls();
        }

        GUILayout.Label(script.Description());
    }
}
