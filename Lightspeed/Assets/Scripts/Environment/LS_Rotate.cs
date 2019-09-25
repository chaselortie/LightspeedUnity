using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_Rotate : MonoBehaviour {
    [SerializeField] float rotationMagnitude = 1;
    private Rigidbody rb;
    private void Start() {
        rb = GetComponent<Rigidbody>();
        UnityEngine.Assertions.Assert.IsNotNull(rb);
        var myRotation = new Vector3(
            Random.Range(0, rotationMagnitude),
            Random.Range(0, rotationMagnitude),
            Random.Range(0, rotationMagnitude));
        rb.angularVelocity = myRotation;
    }
}
