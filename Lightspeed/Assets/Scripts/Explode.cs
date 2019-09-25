using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

    public GameObject planetFull;
    public GameObject explodedPlanet;
    public GameObject explosion;

    public float planetHealth = 20;

	// Use this for initialization
	void Start () {
        planetHealth = 10;
        planetFull.SetActive(true);
        explodedPlanet.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetMouseButton(0)) {
            planetHealth -= 10;

        }
        */
        if (planetHealth <= 0) {
            explodedPlanet.SetActive(true);
            planetFull.SetActive(false);
        }
	}

    /*
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "bullet") {
            planetHealth -= 10;
        }
    }
    */

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
        {
            return;
        }
        Instantiate(explosion, transform.position, transform.rotation);
        if (other.tag == "Player")
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            //GameManager.GameOver(); TODO: implement gameover function
        }
        Destroy(other.gameObject);
        planetHealth -= 10;
    }
    /*

    void OnCollisionEnter(Collision col)
    {
        planetHealth -= 10;
        Instantiate(explosion, col.contacts[0].point, Quaternion.identity);
        Destroy(col.gameObject);
    }
    */

    public void DecreaseHealth() {
        planetHealth -= 10;
        //Debug.Log("decrease health" + "  " + planetHealth);
        //Instantiate(explosion, col.contacts[0].point, Quaternion.identity);
        //Destroy(col.gameObject);
    }

}
