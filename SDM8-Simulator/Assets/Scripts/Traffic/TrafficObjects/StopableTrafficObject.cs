using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Traffic.TrafficObjects
{
    public class StopableTrafficObject : TrafficObject
    {
        /// <summary>
        /// The location of the stopping area
        /// </summary>
        public Vector3 StopCollisionLocation;

        /// <summary>
        /// The size of the stopping area
        /// </summary>
        public Vector3 StopCollisionSize;

        /// <summary>
        /// The object that represents the stopping area
        /// </summary>
        protected GameObject stopCube;

        /// <summary>
        /// Setting up the stopping area (needs to be called)
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();

            UnityThread.executeInUpdate(() =>
            {
                stopCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                stopCube.transform.position = StopCollisionLocation;
                stopCube.transform.localScale = StopCollisionSize;
                stopCube.layer = Constants.Constants.STOP_LAYER;
                stopCube.GetComponent<Collider>().isTrigger = true;
                stopCube.name = $"Cube: {ToString()}";
                stopCube.SetActive(true);
            });
        }

    }
}
