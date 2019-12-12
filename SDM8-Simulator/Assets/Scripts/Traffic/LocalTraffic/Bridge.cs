using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : TrafficObject
{
    private float t;
    private Vector3 startRotation;
    private Vector3 target;
    private float timeToReachTarget = 10;

    private readonly Vector3 closeState = new Vector3(0, 0, 0);
    private readonly Vector3 openState = new Vector3(90, 0, 0);

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
            Open();
        else
            Close();
    }

    public void Open()
    {
        SetRotationDestination(openState, timeToReachTarget);
    }

    public void Close()
    {
        SetRotationDestination(closeState, timeToReachTarget);
    }

    private void SetRotationDestination(Vector3 dest, float time)
    {
        t = 0;
        UnityThread.executeInUpdate(() => startRotation = transform.rotation.eulerAngles);
        target = dest;
    }

    public void Update()
    {
        t += Time.deltaTime / timeToReachTarget;
        transform.eulerAngles = Vector3.Lerp(startRotation, target, t);
    }
}
