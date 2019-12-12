using Assets.Scripts.Traffic.Participant;

namespace Assets.Scripts.Traffic.TrafficObjects
{
    internal class BoatSensor : Sensor
    {
        public override void SetUp()
        {
            base.SetUp();
            laneType = LaneType.VESSEL;
        }

        private void OnTriggerEnter(UnityEngine.Collider collision)
        {
            collisionSize++;
            Boat boatoe = collision.gameObject.GetComponent<Boat>();
            if (boatoe != null)
                SetStatus(1);
        }

        private void OnTriggerExit(UnityEngine.Collider collision)
        {
            collisionSize--;
            Boat boatoe = collision.gameObject.GetComponent<Boat>();
            if (boatoe != null && collisionSize <= 0)
                SetStatus(0);
        }
    }
}