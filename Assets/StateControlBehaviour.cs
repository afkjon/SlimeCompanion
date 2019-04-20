using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateControlBehaviour : MonoBehaviour
{ 
    public GameObject[] bottomOuterObjs;
    private const string ENVIRONMENT_TAG = "Environment";
    private bool isStayingInPlaceOnEnvironment = false;

    // Expression
    //----------------------
    public Material expressionNeutral;
    public Material expressionBlink;
    public Material expressionHit;
    public Material expressionPetted;
    public GameObject meshObj;
    private Renderer objRenderer;
    private Material[] objMaterials;

    private const float MIN_WAIT_BLINK_TIME_SECONDS = 1f;
    private const float MAX_WAIT_BLINK_TIME_SECONDS = 4f;
    private const float BLINK_DUR_SECONDS = 0.1f;
    private Coroutine blinkCoroutine;

    // States (for expressions)
    //----------------------
    private enum State
    {
        Neutral,
        Hit,
        Pet
    }
    private State currentState;
    private State previousState;

    // Processors
    //----------------------
    private ProcessHit hitProcessor;
    private ProcessPet petProcessor;
    private ProcessLift liftProcessor;
    private ProcessGrab grabProcessor;
    private ProcessBeckon beckonProcessor;

    // Start is called before the first frame update
    void Start()
    {
        objRenderer = meshObj.GetComponent<Renderer>();
        objMaterials = objRenderer.materials;

        hitProcessor = GetComponent<ProcessHit>();
        petProcessor = GetComponent<ProcessPet>();
        liftProcessor = GetComponent<ProcessLift>();
        grabProcessor = GetComponent<ProcessGrab>();
        beckonProcessor = GetComponent<ProcessBeckon>();

        currentState = State.Neutral;
        previousState = currentState;

        updateState();
        updateExpression();
    }

    // Update is called once per frame
    void Update()
    {
        updateState();
        //Debug.Log(previousState +", "+ currentState + "-- " + (previousState != currentState));

        // if state changed, update expression
        if(previousState != currentState)
            updateExpression();
    }

    private void updateState()
    {
        previousState = currentState;

        if(hitProcessor.isHitState())
        {
            currentState = State.Hit;
            petProcessor.resetPetStateTriggers();
        }
        else if(beckonProcessor.isBeckonState())
        {
            currentState = State.Neutral; // allow blinking expression
        }
        else if(grabProcessor.isGrabState())
        {
            currentState = State.Neutral; // allow blinking expression
        }
        else if(petProcessor.isPetState())
        {
            currentState = State.Pet;
            // attach to environment during petting if none of the following are true:
            // - is currently being lifted
            // - is already attach to environment
            if(!liftProcessor.isLiftState() && !isStayingInPlaceOnEnvironment)
                toogleStayInPlaceOnEnvironment();
        }
        else
        {
            currentState = State.Neutral;
        }

        if(isStayingInPlaceOnEnvironment
            && previousState==State.Pet && currentState!=State.Pet) // changing from pet state
            toogleStayInPlaceOnEnvironment();
    }

    private void updateExpression()
    {
        switch(currentState)
        {
            case State.Hit:
                setExpression(expressionHit);
                break;
            case State.Pet:
                setExpression(expressionPetted);
                break;
            case State.Neutral:
                setExpression(expressionNeutral);
                blinkCoroutine = StartCoroutine(BlinkExpression());
                break;
        }

        //handle blinkCoroutine
        if(previousState==State.Neutral && currentState!=State.Neutral) // changed from neutral
            StopCoroutine(blinkCoroutine);
    }

    private IEnumerator BlinkExpression()
    {
    	while(currentState == State.Neutral)
    	{
	    	yield return new WaitForSeconds(Random.Range(MIN_WAIT_BLINK_TIME_SECONDS, MAX_WAIT_BLINK_TIME_SECONDS));
	    	setExpression(expressionBlink);

	    	yield return new WaitForSeconds(BLINK_DUR_SECONDS);
	    	setExpression(expressionNeutral);
    	}
    }

    private void setExpression(Material exprMat)
    {
        objMaterials[0] = exprMat;
        objRenderer.materials = objMaterials;
    }

    private void toogleStayInPlaceOnEnvironment()
    {
        if(!allBottomObjsTouchingEnvironment()) 
            return;

        for(int i = 0; i < bottomOuterObjs.Length; i++)
        {
            //if(!isStayingInPlaceOnEnvironment) 
            //    bottomOuterObjs[i].GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePosition;
            //else // unfreeze
            //    bottomOuterObjs[i].GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;
            if (!isStayingInPlaceOnEnvironment)
                bottomOuterObjs[i].GetComponent<Rigidbody>().isKinematic = true;
            else // unfreeze
                bottomOuterObjs[i].GetComponent<Rigidbody>().isKinematic = false;
        }

        isStayingInPlaceOnEnvironment = !isStayingInPlaceOnEnvironment;
    }
    private bool allBottomObjsTouchingEnvironment()
    {
        bool allBottomObjsTouchingEnvironment = true;

        for(int i = 0; i < bottomOuterObjs.Length; i++)
        {
            // (ref: https://answers.unity.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html)
            // "do a short Raycast in the down direction to check if the ground is there. 
            // "short" in this case means the distance from the player pivot to the ground (distToGround); 
            // in most cases, collider.bounds.extents.y is this distance (unless collider.bounds.center isn't 0,0,0). 
            // It's advisable to add a small margin (say, 0.1) to compensate for small ground irregularities or inclination"
            float distToGround = bottomOuterObjs[i].GetComponent<Collider>().bounds.extents.y + 0.1f;
            Vector3 rayOrigin = bottomOuterObjs[i].transform.position;
            Vector3 direction = -Vector3.up;
            Ray ray = new Ray(rayOrigin, direction);
            RaycastHit hit;

            //Debug.DrawRay(rayOrigin, direction, Color.red, distToGround);
            bool rayHitEnvironment = Physics.Raycast(ray, out hit, distToGround) && (hit.collider.tag == ENVIRONMENT_TAG);
            
            if(!rayHitEnvironment)
                allBottomObjsTouchingEnvironment = false;
        }

        //Debug.Log(allBottomObjsTouchingEnvironment);
        return allBottomObjsTouchingEnvironment;
    }
}
