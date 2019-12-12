using Assets.Scripts.Traffic.Participant;

namespace Assets.Scripts.Traffic.TrafficObjects
{
    internal class FootSensor : Sensor
    {
        private void OnTriggerEnter(UnityEngine.Collider collision)
        {
            Walker walker = collision.gameObject.GetComponent<Walker>();
            if (walker != null)
                SetStatus(1);
        }

        private void OnTriggerExit(UnityEngine.Collider collision)
        {
            Walker walker = collision.gameObject.GetComponent<Walker>();
            if (walker != null)
                SetStatus(0);
        }
    }
}