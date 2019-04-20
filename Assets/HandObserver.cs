using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObserver : MonoBehaviour
{
    private bool open = false;
    private bool fist = false;
    private bool openFaceUp = false;

    public void openTrue() {
        open = true;
    }
    public void openFalse() {
        open = false;
    }
    public bool isOpen() {
        return open;
    }

    public void fistTrue() {
        fist = true;
    }
    public void fistFalse() {
        fist = false;
    }
    public bool isFist() {
        return fist;
    }

    public void openFaceupTrue() {
        openFaceUp = true;
    }
    public void openFaceupFalse() {
        openFaceUp = false;
    }
    public bool isOpenFaceUp() {
        return openFaceUp;
    }
}
