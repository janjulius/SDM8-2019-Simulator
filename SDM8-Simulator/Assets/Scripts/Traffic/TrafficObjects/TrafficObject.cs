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
                int i = Convert.ToInt32(e.Message[e.Message.Length - 1]);
                SetStatus(i);
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

        public virtual void SetStatus(int i)
        {
            Status = i;
        }
    }
}
