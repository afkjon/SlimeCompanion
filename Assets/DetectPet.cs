using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class DetectPet : MonoBehaviour
{
    public GameObject processorObj;
    public GameObject handObserverLeftObj;
    public GameObject handObserverRightObj;
    public GameObject detectLeapCollisionObj;

    private ProcessPet petProcessor;
    private HandObserver handObserverLeft;
    private HandObserver handObserverRight;
    private DetectLeapCollision detectLeapCollision;

    // Start is called before the first frame update
    void Start()
    {
        petProcessor = processorObj.GetComponent<ProcessPet>();
        handObserverLeft = handObserverLeftObj.GetComponent<HandObserver>();
        handObserverRight = handObserverRightObj.GetComponent<HandObserver>();
        detectLeapCollision = detectLeapCollisionObj.GetComponent<DetectLeapCollision>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Collision and Trigger Detection Methods
    //================================================
    private void OnCollisionStay(Collision col)
    {
        if(detectLeapCollision.isHandCollision(col.gameObject))
            determineIfHandPetting(col.gameObject);
    }

    // Trigger detection needed. LEAP hand colliders become trigger on collision.
    // (If there are constant collisons [hand phasing through body], it will stay trigger and OnCollision methods will not fire)
    private void OnTriggerStay(Collider col)
    {
        if (detectLeapCollision.isHandCollision(col.gameObject))
            determineIfHandPetting(col.gameObject);
    }

    // Petting Methods
    //================================================

    // Interaction considered petting if:
    // - hand is moving side to side
    // - and hand is open
    private void determineIfHandPetting(GameObject col)
    {
        Hand colHand = detectLeapCollision.getHandFromCollision(col);
        if(colHand == null)
            return;

        bool handOpen;
        if(colHand.IsLeft)
            handOpen = handObserverLeft.isOpen();
        else if(colHand.IsRight)
            handOpen = handObserverRight.isOpen();
        else
            return;
       
        if(isMovingAlongLocalAxisX(detectLeapCollision.getPalmFromCollision(col), colHand.PalmVelocity.ToVector3()) && handOpen)
            petProcessor.notifyReceivingPetMotion(colHand);
    }

    private bool isMovingAlongLocalAxisX(GameObject obj, Vector3 direction)
    {
        Vector3 localDirection = obj.transform.InverseTransformDirection(direction);

        float absX = Mathf.Round(Mathf.Abs(localDirection.x)*10)/10;
        float absY = Mathf.Round(Mathf.Abs(localDirection.y)*10)/10;
        float absZ = Mathf.Round(Mathf.Abs(localDirection.z)*10)/10;

        if(absX>absY && absX>absZ) // greatest change happening on x axis
        {
            if(localDirection.x != 0) // moving left or right
                return true;
        }
        return false;
    }
}
