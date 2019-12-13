namespace Assets.Scripts.Traffic.TrafficObjects
{
    internal class RoadDeckSensor : Sensor
    {
        public override void SetUp()
        {
            base.SetUp();
        }

        private void OnTriggerEnter(UnityEngine.Collider collision)
        {
            collisionSize++;
            print(collisionSize);

            SetStatus(1);
        }

        private void OnTriggerExit(UnityEngine.Collider collision)
        {
            collisionSize--;
            if (collisionSize <= 0)
                SetStatus(0);
        }
    }
}