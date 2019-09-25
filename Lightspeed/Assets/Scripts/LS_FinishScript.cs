using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_FinishScript : MonoBehaviour {
    public LS_RaceManager manager;
    //public GameObject newGameButton;
    //public GameObject endGameButton;
	// Use this for initialization
	void Start () {
        //newGameButton.SetActive(false);
        //endGameButton.SetActive(false); 
        //// on collision if object is user count time
        //// show results and replay option --> start
	}

    void OnTriggerEnter(Collider other)
    {
        if(manager == null) {
            Debug.LogError("No Manager was set. The manager should set itself as manager.");
        }
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Detected!");
            manager.End(true);
        } 
    }
}
