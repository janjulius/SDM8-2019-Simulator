using Assets.Scripts.Traffic.Participant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Traffic.TrafficObjects
{
    class BoatSensor : Sensor
    {
        private void OnTriggerEnter(UnityEngine.Collider collision)
        {
            Boat boatoe = collision.gameObject.GetComponent<Boat>();
            if (boatoe != null)
                SetStatus(1);
        }

        private void OnTriggerExit(UnityEngine.Collider collision)
        {
            Boat boatoe = collision.gameObject.GetComponent<Boat>();
            if (boatoe != null)
                SetStatus(0);
        }
    }
}
