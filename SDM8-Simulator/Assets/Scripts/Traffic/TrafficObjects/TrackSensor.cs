using Assets.Scripts.Traffic.Participant;

namespace Assets.Scripts.Traffic.TrafficObjects
{
    internal class TrackSensor : Sensor
    {
        private void OnTriggerEnter(UnityEngine.Collider collision)
        {
            Train train = collision.gameObject.GetComponent<Train>();
            if (train != null)
                SetStatus(1);
        }

        private void OnTriggerExit(UnityEngine.Collider collision)
        {
            Train train = collision.gameObject.GetComponent<Train>();
            if (train != null)
                SetStatus(0);
        }
    }
}