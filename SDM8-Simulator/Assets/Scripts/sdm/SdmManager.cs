using Assets.Scripts.Traffic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SdmManager : MonoBehaviour
{
    public string connectedGroup = "8";
    //private static SdmManager instance = null;
    
    //[HideInInspector]
    public List<TrafficParticipant> trafficParticipants = new List<TrafficParticipant>();

    //private void Awake()
    //{
    //    new SdmManager();
    //}
    //
    //private SdmManager()
    //{
    //
    //}
    //
    //public static SdmManager Instance
    //{
    //    get
    //    {
    //        if(instance == null)
    //        {
    //            instance = new SdmManager();
    //        }
    //        return instance;
    //    }
    //}
}
