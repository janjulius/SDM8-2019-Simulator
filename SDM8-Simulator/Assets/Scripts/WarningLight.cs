using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class WarningLight : TrafficObject
    {
        private Coroutine routine;

        [HeaderAttribute("IMPORTANT: This is overriden by LaneType Track and Vessel")]
        public WarningLightType warningLightType;

        void Awake()
        {
            base.Awake();
            warningLightType = (
                laneType == LaneType.TRACK ? WarningLightType.TRAIN : 
                laneType == LaneType.VESSEL ? WarningLightType.BOAT : 
                warningLightType);
        }

        public override void SetStatus(int s)
        {
            base.SetStatus(s);
            print("testing overriden setstatus " + s);
            UnityThread.executeCoroutine(WarningLightIEnumerator());
        }

        IEnumerator WarningLightIEnumerator()
        {
            if (Status != 1)
                yield break; //stop this routine
            print("test:");
            SetRendererColor(Color.white);
            yield return new WaitForSeconds(1f);
            print("test:2");
            SetRendererColor(Color.yellow);
            yield return new WaitForSeconds(1f);
            StartCoroutine(WarningLightIEnumerator());
        }
    }
}
