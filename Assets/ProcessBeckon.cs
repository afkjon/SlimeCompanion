using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessBeckon : MonoBehaviour
{
    public GameObject beckonScriptObj;
    private BeckonBehavior beckonScript;

    void Awake()
    {
        beckonScript = beckonScriptObj.GetComponent<BeckonBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isBeckonState()
    {
        return beckonScript.isRespondingToBait();
    }
}
