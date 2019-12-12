namespace Assets.Scripts.Traffic.TrafficObjects
{
    internal class RoadSensor : Sensor
    {
        public override void SetUp()
        {
            base.SetUp();
        }

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