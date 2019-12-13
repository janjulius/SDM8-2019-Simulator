using System;
using System.Text;
using System.Threading;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Assets.Scripts
{
    /// <summary>
    /// Represents the SdmClient built to hold all mqtt behaviour
    /// </summary>
    public class SdmClient : MonoBehaviour
    {
        protected MqttClient mqttClient;

        protected string teamId = "8";
        public LaneType laneType;
        public string groupId;
        public ComponentType componentType;
        public string componentId;

        private void Awake()
        {
            UnityThread.initUnityThread(); //may have to reduce this
        }

        public void Start()
        {
            teamId = Camera.main.GetComponent<SdmManager>().connectedGroup;
            new Thread(() =>
            {
                mqttClient = Connect($"{Constants.Constants.ADDRESS}", Constants.Constants.PORT);
                SetUp();
            }).Start();
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

        /// <summary>
        /// Connect to a host
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="brokerPort">The port</param>
        /// <returns>The resulting MqttClient</returns>
        private MqttClient Connect(string host, int brokerPort)
        {
            MqttClient client = new MqttClient(host, brokerPort, false, null);
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            if (Constants.Constants.SHOW_CONNECTED_MESSAGES)
                print($"Connection started to {host}:{brokerPort} ({ToString()})");
            return client;
        }

        /// <summary>
        /// Auto subscribe to set values and find mqttclient if not existing
        /// </summary>
        protected void Subscribe() => Subscribe(mqttClient ?? Connect(Constants.Constants.ADDRESS, Constants.Constants.PORT));

        /// <summary>
        /// Subscribe to this mqtt topic
        /// </summary>
        /// <param name="client">The client</param>
        private void Subscribe(MqttClient client)
        {
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            var topics = new string[] { ToString() };
            var qos = new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
            Subscribe(client, topics, qos);
            if (Constants.Constants.SHOW_CONNECTED_MESSAGES)
                print($"Started subscription on: {Constants.Constants.ADDRESS}:{Constants.Constants.PORT}, topic: {ToString()}");
        }

        private void Subscribe(MqttClient client, string[] topics, byte[] qos)
        {
            try
            {
                client.Subscribe(topics, qos);
            }
            catch (MqttCommunicationException e)
            {
                Thread.Sleep(200);
                Subscribe(client, topics, qos);
            }
            catch (Exception e)
            {
                Thread.Sleep(200);
                Subscribe(client, topics, qos);
            }
        }

        /// <summary>
        /// Publishes a message on the current topic
        /// </summary>
        /// <param name="message">The message</param>
        protected void Publish(string message)
            =>
            Publish(mqttClient ?? Connect(Constants.Constants.ADDRESS, Constants.Constants.PORT), ToString(), Encoding.ASCII.GetBytes(message));

        /// <summary>
        /// Publishes a message on the current topic
        /// </summary>
        /// <param name="message">The message</param>
        protected void Publish(int message) => Publish(message.ToString());

        /// <summary>
        /// Publishes a message on a custom topic and client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void Publish(MqttClient client, string topic, string message)
        {
            Publish(client, topic, Encoding.ASCII.GetBytes(message));
        }

        /// <summary>
        /// Publishes a message on a custom topic and client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void Publish(MqttClient client, string topic, byte[] message)
        {
            //if (Constants.Constants.SHOW_CONNECTED_MESSAGES)
            //print($"Sent message: {message.ToString()} to topic: {topic.ToString()}");
            string clientId = Guid.NewGuid().ToString();
            if (!client.IsConnected)
                client.Connect(clientId);
            client.Publish(topic, message);
        }

        public virtual void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Debug.Log($"Received message on: {e.Topic} {Encoding.UTF8.GetString(e.Message)}");
        }

        public override string ToString()
        {
            return $"{teamId}/{laneType.ToString().ToLower()}/{groupId}/{componentType.ToString().ToLower()}/{componentId}";
        }
    }
}