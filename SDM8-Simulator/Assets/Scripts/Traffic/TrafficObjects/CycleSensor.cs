using Assets.Scripts.Traffic.Participant;

namespace Assets.Scripts.Traffic.TrafficObjects
{
    internal class CycleSensor : Sensor
    {
        private void OnTriggerEnter(UnityEngine.Collider collision)
        {
            Biker biker = collision.gameObject.GetComponent<Biker>();
            if (biker != null)
                SetStatus(1);
        }

        private void OnTriggerExit(UnityEngine.Collider collision)
        {
            Biker biker = collision.gameObject.GetComponent<Biker>();
            if (biker != null)
                SetStatus(0);
        }
    }
}