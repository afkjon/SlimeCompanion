using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessHit : MonoBehaviour
{
    public const int HIT_THRESHOLD = 1300;
    private const int HIT_DUR_SECONDS = 1;
    private bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isHitState()
    {
        return isHit;
    }

    // acknowledge hit for short duration
    public void registerHit()
    {
        // prevent starting multiple coroutines
        if(isHit) // a hit has already been registered
            return;

        isHit = true;
        StartCoroutine(RegisterHit());
    }
    private IEnumerator RegisterHit()
    {
        if(isHit)
        {
            yield return new WaitForSeconds(HIT_DUR_SECONDS);
            isHit = false;
        }
    }
}
