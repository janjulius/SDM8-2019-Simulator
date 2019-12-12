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
        public BarrierRotateStates rotateState;

        private float t;
        private Vector3 startRotation;
        private Vector3 target;
        private float timeToReachTarget = 4;

        private Vector3 closeState = new Vector3(0, 0, 0);
        private Vector3 openState = new Vector3(90, 0, 0);

        public override void SetUp()
        {
            base.SetUp();
            SetStatus(0);
            Subscribe();
            DetermineStates();
        }

        public override void SetStatus(int i)
        {
            base.SetStatus(i);
            if (Status > 0)
            {
                Close();
                UnityThread.executeInUpdate(() => stopCube?.SetActive(true));
            }
            else
            {
                Open();
                UnityThread.executeInUpdate(() => stopCube?.SetActive(false));
            }
        }
        
        private void DetermineStates()
        {
            Vector3 cur = gameObject.transform.eulerAngles;
            openState = cur;
            switch (rotateState)
            {
                case BarrierRotateStates.ZM:
                    closeState = new Vector3(cur.x, cur.y, cur.z - 90);
                    break;
                case BarrierRotateStates.ZP:
                    closeState = new Vector3(cur.x, cur.y, cur.z + 90);
                    break;
                case BarrierRotateStates.XM:
                    closeState = new Vector3(cur.x - 90, cur.y, cur.z);
                    break;
                case BarrierRotateStates.XP:
                    closeState = new Vector3(cur.x - 90, cur.y, cur.z);
                    break;
            }
        }

        public void Open()
        {
            SetRotationDestination(openState, timeToReachTarget);
        }

        public void Close()
        {
            SetRotationDestination(closeState, timeToReachTarget);
        }

        private void SetRotationDestination(Vector3 dest, float time)
        {
            t = 0;
            UnityThread.executeInUpdate(() => startRotation = transform.rotation.eulerAngles);
            target = dest;
        }

        public void Update()
        {
            t += Time.deltaTime / timeToReachTarget;
            transform.eulerAngles = Vector3.Lerp(startRotation, target, t);
        }
    }

    enum BarrierRotateStates
    {
        ZP,
        ZM,
        XP,
        XM
    }

}
