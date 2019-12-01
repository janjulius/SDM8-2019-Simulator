using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Assets.Scripts
{
    /// <summary>
    /// Represetns a traffic object that has a mqtt connection
    /// </summary>
    public class TrafficObject : SdmClient
    {
        public int Status { private set; get; } = 0;

        protected delegate void StatusUpdatedDelegate(int i);

        protected event StatusUpdatedDelegate StatusUpdatedEvent;
        
        public override void SetUp()
        {
            base.SetUp();
            StatusUpdatedEvent += new StatusUpdatedDelegate(SetStatus);
        }

        public void SetRendererColor(Color c)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", c);
        }
        
        public override void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            base.Client_MqttMsgPublishReceived(sender, e);
            try
            {
                int i = BitConverter.ToInt32(e.Message, 0);
                SetStatus(i);
                if(!isValidStatus(i, Constants.Constants.STATUS_BOUNDARY_MIN, Constants.Constants.STATUS_BOUNDARY_MAX))
                    Debug.LogError($"A status was set to invalid values: {i} {this}");
            }
            catch (InvalidCastException fe)
            {
                print($"Incorrect format: {e.Message}, {fe.StackTrace}");
            }
            catch (Exception r)
            {
                print(r);
            }
        }

        private bool isValidStatus(int s, int min, int max)  => s >= min && s <= max;
        
        public virtual void SetStatus(int i)
        {
            Status = i;
        }
    }
}
