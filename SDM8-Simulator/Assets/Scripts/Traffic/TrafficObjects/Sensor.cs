using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class Sensor : TrafficObject
    {
        private int previousStatus = 0;

        protected int collisionSize = 0;

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
