using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the networked side of the player (such as requesting entry to purchase something or enter a race)
/// This script should really only handle talking and listening to the network.
/// It will turn the local controller on or off based on if it needs to do more than that.
/// </summary>
public class LS_NetworkedPlayer : MonoBehaviour {
    private Camera playerCamera;
    private AudioListener playerListener;
    private PhotonView photonView;
    [SerializeField] private int health = 100;
    Vector3 startPos = Vector3.zero;
    float startTime = 0;

    private void Start () {
        playerListener = GetComponentInChildren<AudioListener>();
		playerCamera = GetComponentInChildren<Camera>();
        photonView = GetComponent<PhotonView>();
        
        //UnityEngine.Assertions.Assert.IsNotNull(playerListener);
        //UnityEngine.Assertions.Assert.IsNotNull(playerCamera);
        UnityEngine.Assertions.Assert.IsNotNull(photonView);

        Initialize();
	}
    
    // Kills functionality if this networked player does not belong to the active client.
    private void Initialize () {
        Transform parentOf_Players = GameObject.Find("parentOf_Players").transform; // TODO pass this through init
        LS_PlayerController localClient = GetComponent<LS_PlayerController>();
        PlayerFlightControl localClient2 = GetComponent<PlayerFlightControl>();
        UnityEngine.Assertions.Assert.IsNotNull(parentOf_Players);
        
        // TODO: This is a major hack and needs to be removed! It prevents zombie players from spawning
        if(localClient2 == null) {
            Destroy(this.gameObject);
            return;
        }
            
        UnityEngine.Assertions.Assert.IsNotNull(localClient2);

        transform.SetParent(parentOf_Players);
        if(photonView.isMine) {
            // This check prob not needed TODO
            var preExistingPlayer = parentOf_Players.Find("PLAYER");
            if(null != preExistingPlayer) {
                Destroy(preExistingPlayer);
            }
            gameObject.name = "PLAYER";
        }
        else {
            gameObject.name = "ENEMY";
            playerCamera.gameObject.SetActive(false);
            //playerListener.gameObject.SetActive(false);
            //Destroy(GetComponent<Rigidbody>());
            GetComponent<Rigidbody>().isKinematic = true;
            // Disable both the network and local controller for this player
            localClient.enabled = false;
            localClient2.enabled = false;
            
            //this.enabled = false;
            startTime = Time.realtimeSinceStartup;
            startPos = transform.position;

        }
	}

    private void Update() {
        // Only Execute locally
        if(!photonView.isMine) {
            if(startTime + 3 < Time.realtimeSinceStartup) {
                if(Vector3.Distance(startPos, transform.position) < 1) {
                    Debug.LogWarning("Killed a empty ship of dist: " + Vector3.Distance(startPos, transform.position));
                    Destroy(gameObject);
                }
            }
            return;
        }
            

        if(Input.GetKeyDown(KeyCode.F)) {
            health -= 5;
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if(stream.isWriting) {
            stream.SendNext(health);
        }
        else if(stream.isReading) {
            health = (int)stream.ReceiveNext();
        }
    }

    public void PowerUp() {
        Debug.Log("powered up!");
        health += 50;
    }

}
