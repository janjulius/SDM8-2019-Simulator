using Assets.Scripts.Traffic.Participant;

namespace Assets.Scripts.Traffic.TrafficObjects
{
    internal class FootSensor : Sensor
    {
        public override void SetUp()
        {
            base.SetUp();
            laneType = LaneType.FOOT;
        }

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