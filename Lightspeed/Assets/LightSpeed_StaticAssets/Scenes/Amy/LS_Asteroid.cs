using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_Asteroid : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private GameObject asteroid;
    public Rigidbody rb;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3 (Random.value, Random.value, Random.value);
        rb.position = new Vector3 (Random.value, Random.value, Random.value);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
