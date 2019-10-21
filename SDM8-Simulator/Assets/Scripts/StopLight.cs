using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

public class StopLight : TrafficObject
{
    public Vector3 StopCollisionLocation;

    public Vector3 StopCollisionSize;

    public override void SetUp()
    {
        base.SetUp();
        SetStatus(0);
        Subscribe(); 
    }

    public override void SetStatus(int status)
    {
        base.SetStatus(status);
        UpdateStopLight();
    }

    public void UpdateStopLight()
    {
        UnityThread.executeInUpdate(() =>
            SetRendererColor(GetColorByStatus(Status))
        );
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
        UpdateStopLight();
        SetStatus(Status == 2 ? 0 : Status + 1);
        yield return new WaitForSeconds(1f);
        StartCoroutine(StopLightTest());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(StopCollisionLocation, StopCollisionSize);
    }
}
