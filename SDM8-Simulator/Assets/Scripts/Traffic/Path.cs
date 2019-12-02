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
    
    public GameObject[] SpawnableObjects;
    
    [Header("How much delay between each spawn, in seconds.")]
    public float SpawnDelay = 1;

    [Header("The chance on every SpawnDelay to spawn (1/SpawnChance)")]
    public int SpawnChance = 50;

    [Header("Wont spawn anything for this amount of seconds after spawning something.")]
    public int GracePeriod = 1;

    private int gracePeriod;

    public bool SpawnBothWays = false;

    public int blockSize = 10;
    
    /// <summary>
    /// The location of the stopping area
    /// </summary>
    private Vector3 StopCollisionLocation;

    /// <summary>
    /// The size of the stopping area
    /// </summary>
    private Vector3 StopCollisionSize;

    void Start()
    {
        gracePeriod = GracePeriod;

        if (SpawnableObjects == null)
            return;
        //if (Points.Length > 0)
        //    SpawnBlockArea.center = Points[0];

        //spawnBlockAreaCollider = gameObject.AddComponent<BoxCollider>();
        //spawnBlockAreaCollider.size = SpawnBlockArea.size;
        //spawnBlockAreaCollider.center = Points[0];


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
        if (gracePeriod <= 0)
        {
            if (SpawnableObjects.Length > 0)
            {
                var spawneyboy = SpawnableObjects[Random.Range(0, SpawnableObjects.Length)];
                Vector3 spawnLoc = SpawnBothWays ? Random.Range(0, 1) == 1 ? Points[Points.Length - 1] : Points[0] : Points[0];

                foreach(TrafficParticipant p in Camera.main.GetComponent<SdmManager>().trafficParticipants)
                {
                    if (p == null)
                        continue;
                    if (Vector3.Distance(p.transform.position, spawnLoc) <= 3)
                    {
                        print("Someone is too close for spawning a new object");
                        return null;
                    }
                }

                GameObject obj = Instantiate(spawneyboy, spawnLoc, Quaternion.identity);
                obj.GetComponent<TrafficParticipant>().SetPath(this);
                Camera.main.GetComponent<SdmManager>().trafficParticipants.Add(obj.GetComponent<TrafficParticipant>());
                return obj.GetComponent<TrafficParticipant>();
            }
        }
        else
        {
            gracePeriod--;
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
        Gizmos.DrawCube(Points[0], new Vector3(blockSize, blockSize, blockSize));
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
