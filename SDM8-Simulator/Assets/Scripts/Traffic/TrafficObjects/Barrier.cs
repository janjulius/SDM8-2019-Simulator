using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class Barrier : TrafficObject
    {
        public override void SetUp()
        {
            base.SetUp();
            Subscribe();
        }
    }
}
