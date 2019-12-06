using Assets.Scripts.Traffic.TrafficObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class Barrier : StopableTrafficObject
    {

        public override void SetUp()
        {
            base.SetUp();
            SetStatus(0);
            Subscribe();
        }

        public override void SetStatus(int status)
        {
            base.SetStatus(status);
            if (status >= 1)
                stopCube?.SetActive(true);
            else
                stopCube?.SetActive(false);
            UpdateBarrier();
        }

        public void UpdateBarrier()
        {
            UnityThread.executeInUpdate(() =>
            {
                if (Status >= 1)
                    transform.Rotate(transform.rotation.x, transform.rotation.y, 90);
                else
                    transform.Rotate(transform.rotation.x, transform.rotation.y, 180);
            });
        }
    }
}
