using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Vector3[] Points;

    private Color GizmoColor;

    public PathTypes PathType;

    [Header("Stoplights units on this path should stop for")]
    public StopLight[] StopForStopLights;
    
    void Start()
    {

    }
    
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        GizmoColor = GetPathColor(PathType);
        for (int i = 0; i < Points.Length; i++)
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawSphere(Points[i], 1f);
            if (i == Points.Length - 1)
                continue;
            Gizmos.color = new Color(GizmoColor.r, GizmoColor.g, GizmoColor.b, 0.5f);
            Gizmos.DrawLine(Points[i], Points[i + 1]);
        }
    }

    private void OnDrawGizmosSelected()
    {
        for(int i = 0; i < Points.Length; i++)
        {
            Vector3 p = Points[i];
            Handles.Label(p, $"P:{i} X: {p.x}, Y: {p.y}, Z: {p.z} T: {PathType}");
        }
    }

    private Color GetPathColor(PathTypes p)
    {
        if (p == PathTypes.BICYCLE)
            return Color.magenta;
        if (p == PathTypes.BOAT)
            return Color.green;
        if (p == PathTypes.CAR)
            return Color.blue;
        if (p == PathTypes.FEET)
            return Color.red;
        if (p == PathTypes.TRAIN)
            return Color.yellow;

        return Color.black;
    }
}
