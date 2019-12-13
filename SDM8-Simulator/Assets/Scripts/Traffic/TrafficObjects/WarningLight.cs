using Assets.Scripts.Traffic.TrafficObjects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    internal class WarningLight : StopableTrafficObject
    {
        private Coroutine routine;

        public override void SetUp()
        {
            base.SetUp();
            SetStatus(0);
            Subscribe();
            componentId = componentId == "" ? "0" : componentId;
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

        private IEnumerator WarningLightIEnumerator()
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