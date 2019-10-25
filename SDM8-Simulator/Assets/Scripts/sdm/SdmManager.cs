using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SdmManager : MonoBehaviour
{
    public string connectedGroup = "8";
    private static SdmManager instance = null;

    private void Awake()
    {
        new SdmManager();
    }

    private SdmManager()
    {

    }

    public static SdmManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new SdmManager();
            }
            return instance;
        }
    }
}
