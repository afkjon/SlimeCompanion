using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpringJointsToObjArray : MonoBehaviour
{
    public GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < objects.Length; i++)
		{
            SpringJoint sj = gameObject.AddComponent<SpringJoint>() as SpringJoint;
            sj.spring = 38;
            sj.connectedBody = objects[i].GetComponent<Rigidbody>();
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
