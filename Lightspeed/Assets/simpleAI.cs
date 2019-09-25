using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleAI : MonoBehaviour {

    public Transform target;
    public float speed;

	// Use this for initialization
	void Start () {
        int frame;
	}
	
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

}
