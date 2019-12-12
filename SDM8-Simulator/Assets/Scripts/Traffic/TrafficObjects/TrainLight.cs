using Assets.Scripts.Traffic.TrafficObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainLight : StopableTrafficObject
{
    public override void SetUp()
    {
        base.SetUp();
        SetStatus(0);
        Subscribe();
    }

    public override void SetStatus(int i)
    {
        base.SetStatus(i);
        if (Status > 0)
            UnityThread.executeInUpdate(() => stopCube?.SetActive(false));
        else
            UnityThread.executeInUpdate(() => stopCube?.SetActive(true));
        UpdateTrainLight();
    }

    public void UpdateTrainLight()
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
            return Color.green;

        return Color.black;
    }
}
