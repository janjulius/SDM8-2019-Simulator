using Assets.Scripts.Traffic.TrafficObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class WarningLight : StopableTrafficObject
    {
        private Coroutine routine;

        [Header("IMPORTANT: This is overriden by LaneType Track and Vessel")]
        public WarningLightType warningLightType;

        void Awake()
        {
            warningLightType = (
                laneType == LaneType.TRACK ? WarningLightType.TRAIN : 
                laneType == LaneType.VESSEL ? WarningLightType.BOAT : 
                warningLightType);
        }

        public override void SetStatus(int s)
        {
            base.SetStatus(s);
            if (Status >= 1)
            {
                UnityThread.executeInUpdate(() => stopCube?.SetActive(true));
                UnityThread.executeCoroutine(WarningLightIEnumerator());
            }
            else
            {
                UnityThread.executeInUpdate(() => stopCube?.SetActive(false));
            }
        }

        IEnumerator WarningLightIEnumerator()
        {
            if (Status < 1)
                yield break; //stop this routine
            SetRendererColor(Color.white);
            yield return new WaitForSeconds(1f);
            SetRendererColor(Color.yellow);
            yield return new WaitForSeconds(1f);
            StartCoroutine(WarningLightIEnumerator());
        }
    }
}
