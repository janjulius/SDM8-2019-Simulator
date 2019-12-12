namespace Assets.Scripts
{
    internal class Sensor : TrafficObject
    {
        private int previousStatus = 0;

        protected int collisionSize = 0;

        public override void SetUp()
        {
            base.SetUp();
            componentType = ComponentType.SENSOR;
        }

        public override void ConnectedRefresh()
        {
            base.ConnectedRefresh();
        }

        public override void SetStatus(int i)
        {
            base.SetStatus(i);
            if (previousStatus != Status)
            {
                previousStatus = Status;
                Publish(previousStatus);
            }
        }
    }
}