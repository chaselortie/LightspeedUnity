using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_GenerateAsteroids : MonoBehaviour {

    [SerializeField] private GameObject asteroid;
    [SerializeField] private GameObject planet;
    [SerializeField] private GameObject rock;
    [SerializeField] private Transform parentOf_Obstacles;
    public int numAsteroids;
    public int numPlanets;
    public int numRocks;
    private BoxCollider box; 
	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider>();
		for (int i = 0; i < numPlanets; i++) {
			Vector3 position = new Vector3(((float)Random.Range(-50, 50) / 100) * transform.localScale.x, ((float)Random.Range(-50, 50) / 100) * transform.localScale.y, ((float)Random.Range(-50, 50) / 100) * transform.localScale.z);
			GameObject clone = Instantiate(planet, position, Quaternion.identity, parentOf_Obstacles);
		}
        for (int i = 0; i < numAsteroids; i++) {
            Vector3 position = new Vector3(((float)Random.Range(-50,50)/100)*transform.localScale.x, ((float)Random.Range(-50,50) / 100) *transform.localScale.y, ((float)Random.Range(-50,50) / 100)*transform.localScale.z);
            GameObject clone = Instantiate(asteroid, position, Quaternion.identity, parentOf_Obstacles);
            //clone.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1,1), Random.Range(-1, 1), Random.Range(-1, 1));
        }
        for (int i = 0; i < numRocks; i++) {
			Vector3 position = new Vector3(((float)Random.Range(-50, 50) / 100) * transform.localScale.x, ((float)Random.Range(-50, 50) / 100) * transform.localScale.y, ((float)Random.Range(-50, 50) / 100) * transform.localScale.z);
			GameObject clone = Instantiate(rock, position, Quaternion.identity, parentOf_Obstacles);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
