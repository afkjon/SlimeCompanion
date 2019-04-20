using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =================================================
// Beckon Object:
// An Invisible object that disappears once slime comes into range
// =================================================
public class BeckonObject : MonoBehaviour
{
    int groundPlaneY = 0;

    void OnCollisionEnter(Collision col)
    {
        // Destroy on collision with anything other than environment
        if (col.gameObject.tag != "Environment")
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        // Destroy this object when it falls below the ground plane
        if (this.transform.position.y < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
