using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class ProcessPet : MonoBehaviour
{   
    private const float PET_STATE_STOP_SECONDS = 0.5f;
    private float timeSinceLastPet = 0.5f;

    private const int PET_METER_LEVEL_CHANGE_STATE = 2; // level of meter when petting recognized
    private const int PET_METER_BUFFER = 1; // cushion state change level
    private float petMeter = 0; // range is [0, PET_METER_LEVEL_CHANGE_STATE + PET_METER_BUFFER]

    public GameObject liftProcessorObj;
    private ProcessLift liftProcessor;

    // Start is called before the first frame update
    void Start()
    {
        liftProcessor = liftProcessorObj.GetComponent<ProcessLift>();
    }

    // Update is called once per frame
    void Update()
    {
        updatePettingConditions();
    }

    public void notifyReceivingPetMotion(Hand h)
    {
        // if the petting hand is not currently lifting, then acknowledge
        if(!liftProcessor.isLiftingHand(h))
            timeSinceLastPet = 0;
    }

    public bool isPetState()
    {
        return petMeter >= PET_METER_LEVEL_CHANGE_STATE;
    }
    public void resetPetStateTriggers()
    {
        petMeter = 0;
    }

    private void updatePettingConditions()
    {        
        //Debug.Log("petMeter=" + petMeter);
        //Debug.Log("timeSinceLastPet="+timeSinceLastPet);

        if(timeSinceLastPet < PET_STATE_STOP_SECONDS)
        {
            timeSinceLastPet += Time.deltaTime;
            // check addition doesn't go over limit
            if(petMeter+Time.deltaTime <= PET_METER_LEVEL_CHANGE_STATE+PET_METER_BUFFER)
                petMeter += Time.deltaTime;
            else
                petMeter = PET_METER_LEVEL_CHANGE_STATE + PET_METER_BUFFER;
        }
        else
        {
            // check subtraction doesn't go under limit
            if(petMeter<PET_METER_LEVEL_CHANGE_STATE && petMeter+PET_METER_BUFFER>=PET_METER_LEVEL_CHANGE_STATE) // passed state change lv, subtract a large chunk
                petMeter -= PET_METER_BUFFER;
            else if(petMeter-Time.deltaTime > 0)
                petMeter -= Time.deltaTime;
            else
                petMeter = 0;
        }  
    }
}
