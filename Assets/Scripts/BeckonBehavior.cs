using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeckonBehavior : MonoBehaviour
{
    // Test Objects
    bool respondToBait;
    GameObject bait;

    // Slime Objects
    GameObject slime;
    GameObject center;

    float moveSpeed = 0.01f;

    void Start()
    {
        slime = this.gameObject;
        center = this.gameObject.transform.Find("Armature").Find("CENTER").gameObject;

        if (bait)
            respondToBait = true;
    }

    void Update()
    {
        FindBait();

        if (bait && respondToBait)
            MoveTowards(bait.transform.position);            
    }

    // Moves the slime towards a specified position
	void MoveTowards(Vector3 position)
	{
        slime.transform.LookAt(-position);
        slime.transform.position = Vector3.Lerp(slime.transform.position, position, moveSpeed);
    }

    void FindBait()
    {
        GameObject[] baitList = GameObject.FindGameObjectsWithTag("Bait");

        // Don't respond if >= 2 bait objects
        if (baitList.Length >= 2)
        {
            respondToBait = false;
        }
        else if (baitList.Length == 1)
        {
            bait = baitList[0];
            respondToBait = true;
        }
    }

    public bool isRespondingToBait() // (Added by C.)
    {
        return bait && respondToBait;
    }
}
