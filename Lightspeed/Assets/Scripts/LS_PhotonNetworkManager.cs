using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DONT USE, replaced with local controller
/// </summary>
public class LS_PhotonNetworkManager : MonoBehaviour {
    private const string VERSION_NUMBER = "0.1";
    private const string ROOM_NAME = "newRoom";
    [SerializeField] private TMPro.TextMeshProUGUI txtNotification;
    [SerializeField] private LS_PlayerController prefab_Player; 
    [SerializeField] private Transform parentOf_SpawnPoints; 
    [SerializeField] private Transform parentOf_Players; 

    private void Awake() {
        UnityEngine.Assertions.Assert.IsNotNull(txtNotification);
        UnityEngine.Assertions.Assert.IsNotNull(prefab_Player);
        UnityEngine.Assertions.Assert.IsNotNull(parentOf_SpawnPoints);
        UnityEngine.Assertions.Assert.IsNotNull(parentOf_Players);
    }
    
 //   // Use this for initialization
	//private void Start () {
	//	PhotonNetwork.ConnectUsingSettings(VERSION_NUMBER);
	//}
	
 //   public virtual void OnJoinedLobby() {
 //       Debug.Log("You have entered room: " + ROOM_NAME);
 //       PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, null, null);
 //   }

 //   public virtual void OnJoinedRoom() {
 //      GameObject newPlayer = PhotonNetwork.Instantiate(prefab_Player.name, SelectSpawnPoint().position, prefab_Player.transform.rotation, 0);
 //      newPlayer.transform.SetParent(parentOf_Players);
 //   }

    public Transform SelectSpawnPoint() {
        int spawnIndex = Random.Range(0, parentOf_SpawnPoints.childCount-1);
        return parentOf_SpawnPoints.GetChild(spawnIndex);
    }

	//// Update is called once per frame
	//private void Update () {
 //       // TODO: replace this with an event subscription
	//	txtNotification.text = PhotonNetwork.connectionStateDetailed.ToString();
	//}
}
