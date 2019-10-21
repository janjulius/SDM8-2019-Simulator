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
    public string groupId;
    public string subgroupId;
    public ComponentType componentType;
    public string componentId;

    public void Awake()
    {
        UnityThread.initUnityThread();
    }

    public void Start()
    {
        teamId = SdmManager.Instance.connectedGroup;
        client = Connect($"{Constants.ADDRESS}", Constants.PORT);
        Subscribe(client, ToString());
    }

    private void Update()
    {
        if(client != null)
            if (!client.IsConnected)
            {
                client = Connect($"{Constants.ADDRESS}", Constants.PORT);
                Subscribe(client, ToString());
            }
    }

    public override string ToString()
    {
        return $"{teamId}/{laneType.ToString().ToLower()}/{groupId}/{subgroupId}/{componentType.ToString().ToLower()}/{componentId}";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, 5);
    }

    public MqttClient Connect(string host, int brokerPort)
    {
        MqttClient client = new MqttClient(host, brokerPort, false, null);
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);
        return client;
    }

    public void Subscribe(MqttClient client, string topic)
    {
        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);
        client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        if(Constants.SHOW_CONNECTED_MESSAGES)
            print($"Started subscription on: {Constants.ADDRESS}:{Constants.PORT}, topic: {ToString()}");
    }

    public virtual void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log(e.Topic + " : " + Encoding.UTF8.GetString(e.Message));
    }
}
