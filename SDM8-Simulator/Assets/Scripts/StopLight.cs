using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopLight : SdmSub
{
    private int status = 0;

    private GameObject ConnectedTrafficLight;

    public Vector3 StopCollisionLocation;

    public Vector3 StopCollisionSize;

    // Start is called before the first frame update
    void Start()
    {
        ConnectedTrafficLight = gameObject;
        base.Start();
        //StartCoroutine(StopLightTest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStatus(int status)
    {
        this.status = status;
        UpdateStopLight();
    }
    
    public void UpdateStopLight()
    {
        ConnectedTrafficLight.GetComponent<Renderer>().material.SetColor("_Color", GetColorByStatus(status));
    }

    public Color GetColorByStatus(int status)
    {
        if (status == 0)
            return Color.red;
        if (status == 1)
            return Color.yellow;
        if (status == 2)
            return Color.green;

        return Color.black;
    }

    IEnumerator StopLightTest()
    {
        print("test");
        UpdateStopLight();
        status = status == 2 ? 0 : status + 1;
        yield return new WaitForSeconds(1f);
        StartCoroutine(StopLightTest());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(StopCollisionLocation, StopCollisionSize);
    }
}
