using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Traffic.TrafficObjects
{
    class RoadSensor : Sensor
    {
        private void OnTriggerEnter(UnityEngine.Collider collision)
        {
            Car car = collision.gameObject.GetComponent<Car>();
            if (car != null)
                SetStatus(1);
        }

        private void OnTriggerExit(UnityEngine.Collider collision)
        {
            Car car = collision.gameObject.GetComponent<Car>();
            if (car != null)
                SetStatus(0);
        }
    }
}
