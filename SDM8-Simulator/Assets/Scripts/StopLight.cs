using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

public class StopLight : SdmSub
{
    private int status = 0;

    private GameObject ConnectedTrafficLight;

    public Vector3 StopCollisionLocation;

    public Vector3 StopCollisionSize;

    // Start is called before the first frame update
    new void Start()
    {
        ConnectedTrafficLight = gameObject;
    }

    public override void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        base.Client_MqttMsgPublishReceived(sender, e);
        try
        {
            int i = Convert.ToInt32(e.Message[e.Message.Length - 1]);
            SetStatus(i);
        }
        catch (InvalidCastException fe)
        {
            print($"Incorrect format: {e.Message}, {fe.StackTrace}");
        }
        catch(Exception r)
        {
            print(r);
        }
    }

    public void SetStatus(int status)
    {
        this.status = status;
        UnityThread.executeInUpdate(() =>
        {
            UpdateStopLight();
        });
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
