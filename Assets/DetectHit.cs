using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap.Unity.Interaction;
using Leap.Unity.Query;

public class DetectHit : MonoBehaviour
{
    public GameObject processorObj;
    private ProcessHit hitProcessor;

    // Start is called before the first frame update
    void Start()
    {
        hitProcessor = processorObj.GetComponent<ProcessHit>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision col)
    {
        Vector3 impactForce = col.impulse / Time.fixedDeltaTime;
        //Debug.Log(impactForce.magnitude);

        if(impactForce.magnitude >= ProcessHit.HIT_THRESHOLD) // considered hit
        {
            hitProcessor.registerHit();
            //Debug.Log("OW");
        }
    }
}
