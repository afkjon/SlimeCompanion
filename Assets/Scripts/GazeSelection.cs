using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSelection : MonoBehaviour
{
    // Currently selected Object
    GameObject selectedObject;

    // Main Camera Object for gaze
    Camera mainCamera;

    // Stored Material to change colour
    [SerializeField]
    public Material highlightMaterial;

    // Temporarily store original material of the object
    Material originalMaterial;

    GameObject grabbedObject;
    bool grabbing;

    // Hand
    public GameObject handObserverRightObj;
    HandObserver handObserverRight;   


    void Start()
    {
        grabbedObject = null;
        selectedObject = null;
        mainCamera = (Camera) GameObject.FindGameObjectWithTag("MainCamera").GetComponent("Camera");

        originalMaterial = null;
        grabbing = false;

        handObserverRight = handObserverRightObj.GetComponent<HandObserver>();
    }

    // Update is called once per frame
    void Update()
    {

        // Selection
        selectedObject = AttemptObjectSelection();


        // Spacebar or detecting a fist on right hand
        if (handObserverRight.isFist())
        {
            if (!grabbing)
                AttemptGrab(selectedObject);                
        }
        else
        {
            if (grabbing)
                ReleaseGrab();
        }
    }



    //============================================================================
    // Selection Logic
    //============================================================================
    GameObject AttemptObjectSelection()
    {
        GameObject obj = null;

        RaycastHit hit;
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit))
        {
            if (obj != selectedObject)
            {
                RestoreMaterial(selectedObject);
                HighlightSelection(obj);
                print("Looking at: " + selectedObject.name);
            }

            obj = hit.transform.gameObject;
        }

        return obj;
    }

    void HighlightSelection(GameObject obj)
    {
        // Do not highlight environment objets
        if (!obj)
            return;

        if (obj.tag == "Environment")
            return;

        originalMaterial = obj.GetComponent<MeshRenderer>().material;
        obj.GetComponent<MeshRenderer>().material = highlightMaterial;
    }

    void RestoreMaterial(GameObject obj)
    {
        if (!originalMaterial)
            return;

        // Do not highlight environment objets
        if (!obj)
            return;

        if (obj.tag == "Environment")
            return;

        selectedObject.GetComponent<MeshRenderer>().material = originalMaterial;
    }

    //============================================================================
    // Grabbing Logic
    //============================================================================
    void AttemptGrab(GameObject obj)
    {
        // Check if object exists
        if (!obj)
            return;

        // Do not grab environment objets
        if (obj.tag == "Environment")
            return;

        while (obj.transform.parent)
            obj = obj.transform.parent.gameObject;

        RecursiveToggleGravity(obj, false);
            
        obj.transform.position = this.transform.position;
        obj.transform.parent = this.transform;
        grabbedObject = obj;
        grabbing = true;
    }

    void ReleaseGrab()
    {
        grabbedObject.transform.parent = null;
        RecursiveToggleGravity(grabbedObject, true);
        grabbing = false;
    }

    void RecursiveToggleGravity(GameObject obj, bool b)
    {
        Rigidbody childRB = obj.GetComponent<Rigidbody>();
        if (childRB)
            childRB.useGravity = b;

        // Disable Gravity on all rigidbodies
        if (obj.transform.childCount > 0)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                RecursiveToggleGravity(obj.transform.GetChild(i).gameObject, b);
            }
        }
    }

    public bool isBeingGrabbed() // (Added by C.)
    {
        return grabbing;
    }
}