using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Traffic
{
    public class Car : TrafficParticipant
    {
        private int tailGatingDistance = 14;

        public override void TrafficUpdate()
        {
            base.TrafficUpdate();
            int layerMask = 1 << 8;
            layerMask = ~layerMask;
            print(layerMask);
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(transform.position, fwd, out hit, tailGatingDistance, layerMask))
            {
                Speed = 0;
            }
            else
            {
                Speed = origSpeed;
            }
            
            Debug.DrawRay(transform.position, fwd * tailGatingDistance, Color.green);
        }
    }
}
