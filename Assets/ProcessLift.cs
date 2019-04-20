using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class ProcessLift : MonoBehaviour
{
    private bool isLifted = false;
    private List<DetectLift> liftableObjs;

    void Awake()
    {
        liftableObjs = new List<DetectLift>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void registerAsLiftableObj(DetectLift dl)
    {
        liftableObjs.Add(dl);
    }

    // at least one of the liftable objects is being lifted
    public bool isLiftState()
    {
        foreach(DetectLift dl in liftableObjs)
        {
            if(dl.isBeingLifted())
                return true;
        }
        return false;
    } 

    // the passed Hand is currently lifting a liftable object
    public bool isLiftingHand(Hand h)
    {
        foreach (DetectLift dl in liftableObjs)
        {
            if (dl.getLiftingHand() == null) // this liftable object is not being lifted 
            {
                // skip
            }
            else if ((h.IsRight && dl.getLiftingHand().IsRight) || (h.IsLeft && dl.getLiftingHand().IsLeft))
            {
                return true;
            }
        }
        return false;
    }
}
