using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class DetectLift : MonoBehaviour
{
    public GameObject processorObj;
    public GameObject attachLeftHandObj;
    public GameObject attachRightHandObj;
    public GameObject handObserverLeftObj;
    public GameObject handObserverRightObj;
    public GameObject detectLeapCollisionObj;
    
    private ProcessLift liftProcessor;
    private HandObserver handObserverLeft;
    private HandObserver handObserverRight;
    private DetectLeapCollision detectLeapCollision;

    private GameObject originalParent;

    // used to keep track of lifting hand
    private HandObserver liftedHandObserver = null;
    private Hand liftingHand = null;
    private bool isLifted = false;

    // Start is called before the first frame update
    void Start()
    {
        liftProcessor = processorObj.GetComponent<ProcessLift>();
        liftProcessor.registerAsLiftableObj(GetComponent<DetectLift>());

        handObserverLeft = handObserverLeftObj.GetComponent<HandObserver>();
        handObserverRight = handObserverRightObj.GetComponent<HandObserver>();
        detectLeapCollision = detectLeapCollisionObj.GetComponent<DetectLeapCollision>();

        if (transform.parent != null)
            originalParent = transform.parent.gameObject;
        else
            originalParent = null;
    }

    // Update is called once per frame
    void Update()
    {
        // if this object is being lifted but hand conditions are no longer met
        if(isLifted && liftedHandObserver!=null && !liftedHandObserver.isOpenFaceUp())
        {
            detachFromObject();
            updateLiftVariables(null, null, false);
        }
    }

    // Lift occurs if:
    // - hand is moving "up" (visually)
    // - palm of hand is facing up
    private void OnCollisionEnter(Collision col)
    {
        if(detectLeapCollision.isHandCollision(col.gameObject))
        {
            Hand colHand = detectLeapCollision.getHandFromCollision(col.gameObject);
            if(colHand == null)
                return;

            if (!isMovingDownLocalAxisY(detectLeapCollision.getPalmFromCollision(col.gameObject), colHand.PalmVelocity.ToVector3()))
                return;

            if(colHand.IsLeft && handObserverLeft.isOpenFaceUp())
            {
                attachToObject(attachLeftHandObj);
                updateLiftVariables(handObserverLeft, colHand, true);
            }
            else if(colHand.IsRight && handObserverRight.isOpenFaceUp())
            {
                attachToObject(attachRightHandObj);
                updateLiftVariables(handObserverRight, colHand, true);
            }
        }        
    }

    // Lifting Methods
    //================================================

    public bool isBeingLifted()
    {
        return isLifted;
    }

    public Hand getLiftingHand()
    {
        return liftingHand;
    }

    private void updateLiftVariables(HandObserver ho, Hand h, bool b)
    {
        liftedHandObserver = ho;
        liftingHand = h;
        isLifted = b;
    }

    private void attachToObject(GameObject obj)
    {
        //GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezeRotation; // unfreeze positions
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = obj.transform;
    }
    
    private void detachFromObject()
    {
        //GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezeRotation; // freeze positions
        GetComponent<Rigidbody>().isKinematic = false;
        transform.parent = originalParent.transform;
    }

    private bool isMovingDownLocalAxisY(GameObject obj, Vector3 direction)
    {
        Vector3 localDirection = obj.transform.InverseTransformDirection(direction);

        float absX = Mathf.Round(Mathf.Abs(localDirection.x)*10)/10;
        float absY = Mathf.Round(Mathf.Abs(localDirection.y)*10)/10;
        float absZ = Mathf.Round(Mathf.Abs(localDirection.z)*10)/10;

        if(absY>absX && absY>absZ) // greatest change happening on y axis
        {
            // palm's positive y-axis is pointing out the back of the hand
            // (the direction the fingers point when a hand is made into a fist)
            if(localDirection.y < 0) // going down
                return true;
        }
        return false;
    }
}
