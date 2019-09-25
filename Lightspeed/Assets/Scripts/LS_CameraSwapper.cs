using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_CameraSwapper : MonoBehaviour {
    [SerializeField] Camera c1;
    [SerializeField] Camera c2;
    public float secondsBetweenSwap = 15;
    public float timeLastSwap = 0;

    private void Start() {
        UnityEngine.Assertions.Assert.IsNotNull(c1);
        UnityEngine.Assertions.Assert.IsNotNull(c2);
        c1.gameObject.SetActive(true);
        c2.gameObject.SetActive(false);
    }

    void Update() {
        if(Time.fixedTime > timeLastSwap + secondsBetweenSwap) {
            Swap();
            timeLastSwap = Time.fixedTime;
        }
    }

    void Swap() {
        if(c1.gameObject.activeInHierarchy) {
            c1.gameObject.SetActive(false);
            c2.gameObject.SetActive(true);
        }
        else {
            c1.gameObject.SetActive(true);
            c2.gameObject.SetActive(false);
        }
    }
}
