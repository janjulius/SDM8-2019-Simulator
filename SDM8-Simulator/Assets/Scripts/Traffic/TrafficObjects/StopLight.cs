using Assets.Scripts;
using Assets.Scripts.Constants;
using Assets.Scripts.Traffic.TrafficObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

public class StopLight : StopableTrafficObject
{

    public override void SetUp()
    {
        base.SetUp();
        SetStatus(0);
        Subscribe();
    }

   //public override void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
   //{
   //    print("New message on stoplight " + this.name);
   //    SetStatus(Convert.ToInt32(Encoding.UTF8.GetString(e.Message)));
   //}

    public override void SetStatus(int status)
    {
        base.SetStatus(status);
        UpdateStopLight();

        if (status >= 2)
            UnityThread.executeInUpdate(() => stopCube?.SetActive(false));
        else
            UnityThread.executeInUpdate(() => stopCube?.SetActive(true));
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
