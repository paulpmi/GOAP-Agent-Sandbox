using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AILogic))]
public class FOVEditor : Editor
{

    void OnSceneGUI()
    {
        DrawForClass();
    }

    void DrawForClass() {
        AILogic fow = (AILogic)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.black;
        foreach (Transform visibleTarget in fow.possibleWaypoints)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }

}