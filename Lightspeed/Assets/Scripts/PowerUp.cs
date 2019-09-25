using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private AudioSource Power_Up;
    private AudioClip sound;

	// Use this for initialization
    void Start () {
        Power_Up = GetComponent<AudioSource>();
        //sound = Power_Up.clip();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.SendMessage("PowerUp");
            Power_Up.Play();
            Destroy(gameObject);
        }
    }
}
