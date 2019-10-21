using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using Assets.Scripts.Constants;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Assets.Scripts
{
    public class SdmClient : MonoBehaviour
    {
        protected MqttClient mqttClient;

        protected string teamId = "8";
        public LaneType laneType;
        public string groupId;
        public string subgroupId;
        public ComponentType componentType;
        public string componentId;

        private void Awake()
        {
            UnityThread.initUnityThread(); //may have to reduce this
        }

        public void Start()
        {
            teamId = SdmManager.Instance.connectedGroup;
            mqttClient = Connect($"{Constants.Constants.ADDRESS}", Constants.Constants.PORT);
            SetUp();
        }

        private void Update()
        {
            Refresh();
            if (mqttClient != null)
            {
                if (!mqttClient.IsConnected)
                {
                    mqttClient = Connect($"{Constants.Constants.ADDRESS}", Constants.Constants.PORT);
                }
                if (mqttClient.IsConnected)
                    ConnectedRefresh();
            }
        }

        /// <summary>
        /// Method is called at the start, after Start
        /// </summary>
        public virtual void SetUp() { }


        /// <summary>
        /// Method is called every frame, after Update
        /// </summary>
        public virtual void Refresh() { }

        /// <summary>
        /// Gets called if connected, after Update and after <see cref="Refresh"/>
        /// </summary>
        public virtual void ConnectedRefresh() { }

        public MqttClient Connect(string host, int brokerPort)
        {
            MqttClient client = new MqttClient(host, brokerPort, false, null);
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            if (Constants.Constants.SHOW_CONNECTED_MESSAGES)
                print($"Connection started to {host}:{brokerPort}");
            return client;
        }

        public void Subscribe()
        {
            Subscribe(ToString());
        }

        public void Subscribe(string topic)
        {
            Subscribe(mqttClient ?? Connect(Constants.Constants.ADDRESS, Constants.Constants.PORT), topic);
        }
        
        public void Subscribe(MqttClient client, string topic)
        {
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            if (Constants.Constants.SHOW_CONNECTED_MESSAGES)
                print($"Started subscription on: {Constants.Constants.ADDRESS}:{Constants.Constants.PORT}, topic: {ToString()}");
        }

        public void Publish(string topic, string message)
        {
            Publish(mqttClient ?? Connect(Constants.Constants.ADDRESS, Constants.Constants.PORT), topic, Encoding.ASCII.GetBytes(message));
        }

        public void Publish(MqttClient client, string topic, string message)
        {
            Publish(client, topic, Encoding.ASCII.GetBytes(message));
        }

        public void Publish(MqttClient client, string topic, byte[] message)
        {
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            client.Publish(topic, message);
        }
        
        public virtual void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Debug.Log($"Received message on: {e.Topic} {Encoding.UTF8.GetString(e.Message)}");
        }

        public override string ToString()
        {
            return $"{teamId}/{laneType.ToString().ToLower()}/{groupId}/{subgroupId}/{componentType.ToString().ToLower()}/{componentId}";
        }

    }
}
