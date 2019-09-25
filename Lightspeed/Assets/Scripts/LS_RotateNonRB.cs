using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_RotateNonRB : MonoBehaviour {
    [SerializeField] float maxRotationSpeed = 7;
    float rotSpeed = 0;
    private void Start() {
        rotSpeed = Random.Range(maxRotationSpeed/2, maxRotationSpeed);
    }

    void Update () {
        transform.Rotate(0, rotSpeed * Time.deltaTime,0);
	}
}
