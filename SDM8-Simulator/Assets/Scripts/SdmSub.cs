using Assets.Scripts.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

/// <summary>
/// Something that is subscribed to a mqtt thing
/// </summary>
public class SdmSub : MonoBehaviour
{
    MqttClient client;

    private string teamId = "8";
    public LaneType laneType;
    public Direction direction;
    public string groupId;
    public string subgroupId;
    public ComponentType componentType;
    public string componentId;

    public void Start()
    {
        teamId = SdmManager.Instance.connectedGroup;
        client = Connect(Constants.ADDRESS);
        Subscribe(client, ToString());
        print($"Started subscription on: {Constants.ADDRESS}, topic: {ToString()}");
    }
    
    public override string ToString()
    {
        return $"{teamId}/{laneType.ToString().ToLower()}/{direction.ToString().ToLower()}/{groupId}/{subgroupId}/{componentType.ToString().ToLower()}/{componentId}";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, 5);
    }

    public static MqttClient Connect(string host)
    {
        MqttClient client = new MqttClient(host);
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);
        return client;
    }

    public static void Subscribe(MqttClient client, string topic)
    {
        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);
        client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    public static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log(e.Topic + " : " + Encoding.UTF8.GetString(e.Message));
    }
}
