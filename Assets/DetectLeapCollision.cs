using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap.Unity.Interaction;
using Leap;

public class DetectLeapCollision : MonoBehaviour
{
    public GameObject handModelLeft;
    public GameObject handModelRight;

    private const string INTERACTION_HAND_PALM_TRANSFORM = "Palm Transform";
    private const string LEFT = "Left";
    private const string RIGHT = "Right";

    // Collision occurs with "Contact Fingerbone" and "Contact Palm Bone" objects
    // These objects contain a "Contact Bone" script.
    // - if the colliding object has this script component, then it is a leap hand
    public bool isHandCollision(GameObject col)
    {
        return col.GetComponent<ContactBone>() != null;
    }

    public HandModelBase getHandModelFromCollision(GameObject col)
    {
        if (!isHandCollision(col))
            return null;

        GameObject interactionHand = col.GetComponent<ContactBone>().interactionHand.gameObject;
        string interactionHandName = interactionHand.name;

        if (interactionHandName.Contains(LEFT))
            return handModelLeft.GetComponent<HandModelBase>();
        else if (interactionHandName.Contains(RIGHT))
            return handModelRight.GetComponent<HandModelBase>();
        return null;
    }

    public Hand getHandFromCollision(GameObject col)
    {
        if (isHandCollision(col) && getHandModelFromCollision(col)!=null)
            return getHandModelFromCollision(col).GetLeapHand();
        return null;
    }

    // ContactBone script has a public InteractionHand attribute
    // The "Interaction Hand" object in the scene has a "Palm Transform" child object
    public GameObject getPalmFromCollision(GameObject col)
    {
        if(!isHandCollision(col))
            return null;

        GameObject interactionHand = col.GetComponent<ContactBone>().interactionHand.gameObject;
        return interactionHand.transform.Find(INTERACTION_HAND_PALM_TRANSFORM).gameObject;
    }
}
