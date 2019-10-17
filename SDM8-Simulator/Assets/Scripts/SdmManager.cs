using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SdmManager : MonoBehaviour
{
    public string connectedGroup;
    private static SdmManager instance = null;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
