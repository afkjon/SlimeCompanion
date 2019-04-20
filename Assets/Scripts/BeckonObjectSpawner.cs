using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeckonObjectSpawner : MonoBehaviour
{

    public GameObject handObserverLeftObj;
    public GameObject objectToSpawn;

    HandObserver handObserverLeft;

    // Start is called before the first frame update
    void Start()
    {
        handObserverLeft = handObserverLeftObj.GetComponent<HandObserver>();
    }

    // Update is called once per frame
    void Update()
    {
        // Beckon Using Fist
        if (objectToSpawn && (handObserverLeft.isFist()))
        {
            if (!GameObject.FindGameObjectWithTag("Beckon"))
                DropObject(handObserverLeft.transform.position);
        }
    }

    void DropObject(Vector3 pos)
    {
        objectToSpawn.transform.position = pos;
        Instantiate(objectToSpawn);
    }
}
