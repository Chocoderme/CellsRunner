using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Camera_FollowTrack), true)]
public class Camera_FollowTrack_Editor : Editor {

    public override void OnInspectorGUI()
    {
        var obj = (Camera_FollowTrack)target;

        if (GUILayout.Button("Add Point"))
        {
            if (obj.Points.Count > 0)
            {
                Vector2 pos = obj.Points[obj.Points.Count - 1].B;
                pos.x += 1;
                obj.AddPoint(pos);
                SceneView.RepaintAll();
            }
            else
            {
                obj.AddPoint();
                SceneView.RepaintAll();
            }
        }

        if (GUILayout.Button("Remove Point"))
        {
            if (obj.Points.Count > 0)
            {
                obj.Points.RemoveAt(obj.Points.Count - 1);
                SceneView.RepaintAll();
            }
        }

        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        Draw();
    }

    void Draw()
    {
        var obj = (Camera_FollowTrack)target;

        Camera_FollowTrack.BezierPoint old = null;
        foreach (var pt in obj.Points)
        {
            ///
            /// Middle point
            ///
            Handles.color = Color.red;
            Vector2 newPos = Handles.FreeMoveHandle(pt.B, Quaternion.identity, .1f, Vector2.zero, Handles.CircleHandleCap);
            if (pt.B != newPos)
            {
                Undo.RecordObject(obj, "Move point");
                var offSet = newPos - pt.B;
                pt.A += offSet;
                pt.C += offSet;
                pt.B = newPos;
            }

            ///
            /// Prev point
            ///
            Handles.color = Color.green;
            Vector2 newPos2 = Handles.FreeMoveHandle(pt.A, Quaternion.identity, .1f, Vector2.zero, Handles.CircleHandleCap);
            if (pt.A != newPos2)
            {
                Undo.RecordObject(obj, "Move Tangent");
                pt.A = newPos2;
            }

            ///
            /// Next point
            ///
            Handles.color = Color.green;
            Vector2 newPos3 = Handles.FreeMoveHandle(pt.C, Quaternion.identity, .1f, Vector2.zero, Handles.CircleHandleCap);
            if (pt.C != newPos3)
            {
                Undo.RecordObject(obj, "Move Tangent");
                pt.C = newPos3;
            }

            ///
            /// Links
            ///
            Handles.color = Color.black;
            Handles.DrawLine(pt.A, pt.B);
            Handles.DrawLine(pt.B, pt.C);
            if (old != null)
            {
                Handles.DrawBezier(old.B, pt.B, old.C, pt.A, Color.green, null, 2);
            }

            old = pt;
        }
    }
}
