using Assets.Scripts.Traffic;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Vector3[] Points;

    private Color GizmoColor;

    private BoxCollider spawnBlockAreaCollider;

    public PathTypes PathType;

    [Header("Center is automatically the first point if it exists")]
    public Bounds SpawnBlockArea;

    [Header("Stoplights units on this path should stop for")]
    public StopLight[] StopForStopLights;

    //todo change to array
    public GameObject[] SpawnableObjects;

    public float SpawnDelay = 1;

    public int SpawnChance = 1;

    void Start()
    {
        if (SpawnableObjects == null)
            return;
        if (Points.Length > 0)
            SpawnBlockArea.center = Points[0];

        spawnBlockAreaCollider = gameObject.AddComponent<BoxCollider>();
        spawnBlockAreaCollider.size = SpawnBlockArea.size;

        SdmManager.trafficParticipants.Add(SpawnTrafficParticipant());

        StartCoroutine(ContiniousSpawning());
    }

    IEnumerator ContiniousSpawning()
    {
        yield return new WaitForSeconds(SpawnDelay);
        int r = Random.Range(1, SpawnChance);
        if (r == 1)
            SpawnTrafficParticipant();
        StartCoroutine(ContiniousSpawning());
    }

    public TrafficParticipant SpawnTrafficParticipant()
    {
        if (SpawnableObjects.Length > 0)
        {
            GameObject obj = Instantiate(SpawnableObjects[Random.Range(0, SpawnableObjects.Length)], Points[0], Quaternion.identity);
            obj.GetComponent<TrafficParticipant>().SetPath(this);
            return obj.GetComponent<TrafficParticipant>();
        }
        return null;
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
        Gizmos.color = new Color(255, 0, 0, 0.2f);
        Gizmos.DrawCube(SpawnBlockArea.center, SpawnBlockArea.size);
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
