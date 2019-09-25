using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_HaloGlow : MonoBehaviour {
    [SerializeField] float smallestRange = 50;
    [SerializeField] float largestRange = 70;
    [SerializeField] float speed = 1;
    private bool goingUp;
    private Light myglow;
    private void Start() {
        myglow =  GetComponent<Light>();
        UnityEngine.Assertions.Assert.IsNotNull(myglow);
    }
    private void Update() {
        float currRange = myglow.range;
        float newRange = goingUp ? currRange + speed*Time.deltaTime : currRange - speed*Time.deltaTime;
        myglow.range = newRange;

        if(newRange > largestRange)
            goingUp = false;
        else if(newRange < smallestRange)
            goingUp = true;
    }
}
