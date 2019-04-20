using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessGrab : MonoBehaviour
{
    public GameObject grabScriptObj;
    private GazeSelection grabScript;

    void Awake()
    {
        grabScript = grabScriptObj.GetComponent<GazeSelection>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isGrabState()
    {
        return grabScript.isBeingGrabbed();
    }
}
