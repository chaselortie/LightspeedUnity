using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// </summary>
public class LS_LocalGameController : MonoBehaviour {
    private const string VERSION_NUMBER = "0.1";
    private const string ROOM_NAME = "newRoom";
    public const string NAME_OF_START_SCENE = "StartScreen";
    public const string NAME_OF_RACE_SCENE = "Raceway";
    public const string NAME_OF_GARAGE_SCENE = "Garage";
    public const int DEFAULT_HULL_COMPONENT_INDEX = 3;

    [SerializeField] private bool resetGameData;
    [SerializeField] private bool muteBG;
   

    public bool RaceHasBegun = false;

    // Need to programatically assign these TODO
    //private TMPro.TextMeshProUGUI txtNotification;
    private LS_PlayerController prefab_Player;
    private Transform parentOf_SpawnPoints;
    private Transform parentOf_Players;
    public static LS_LocalGameController instance = null;

    private bool initRaceVars = false;
    private Player player;
    private List<ShipComponent> allUnlockableComponents;


    private GameObject OutOfBoundsDisplay;
    private const float HEALTH_TICK = 25f;
    private float lastHealthTick = 0;
    private LS_PlayerController currentPlayer = null;
    bool inGame = false; // TODO generalize to a broader game state
    bool inBounds = true;
    int numOutOfBounds = 0;
    private void Awake() {
        if(transform.parent )
        UnityEngine.Assertions.Assert.IsNull(transform.parent, "This object needs to have no parent! To retain singleton-status.");
        AwakeSingleton();
        // TODO this should instead be a check as soon as we enter the game scene? maybe move to ship controller...
        //UnityEngine.Assertions.Assert.IsNotNull(txtNotification);
        //UnityEngine.Assertions.Assert.IsNotNull(prefab_Player);
        //UnityEngine.Assertions.Assert.IsNotNull(parentOf_SpawnPoints);
        //UnityEngine.Assertions.Assert.IsNotNull(parentOf_Players);
        LoadGameData();
        if(resetGameData) {
            Debug.LogWarning("resetGameData");
            PlayerPrefs.DeleteAll();
            player = new Player();
            SaveData();
        }
        if(muteBG) {
            Debug.LogWarning("muteBG");
            GetComponent<AudioSource>().enabled = false;
        }
    }

    public void SetCurrentPlayer(LS_PlayerController playerController) {
        currentPlayer = playerController;
    }

    public void StartRace() {
        numOutOfBounds = 0;
        inGame = true;
        inBounds = true;
        Debug.Log("Starting a race.");
        player.ResetHealth();
        UnityEngine.SceneManagement.SceneManager.LoadScene(LS_LocalGameController.NAME_OF_START_SCENE);
    }

    public void GoToGarage() {
        inGame = false;
        Debug.Log("Going to garage!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LS_LocalGameController.NAME_OF_GARAGE_SCENE);
    }

    public List<ShipComponent> GetUnlockableComponents() {
        return allUnlockableComponents;
    }

    public Player GetPlayer() {
        return player;
    }

    internal void JoinGame() {
        InitRaceVars();
        SpawnNewPlayer();
    }

    private void LoadGameData() {
        player = LS_DataManager.LoadPlayerData();
        allUnlockableComponents = LS_DataManager.LoadAllUnlockables();
        
        // Examples:
        // Get the player's ship:           LS_LocalGameController.instance.player.ship
        // Get all possible unlockables:    LS_LocalGameController.instance.allUnlockableComponents

        Debug.Log("This player has now played our game " + player.numTimesLoggedIn + " times!");
        player.numTimesLoggedIn++;
         LS_DataManager.SavePlayerData(player);
    }

    // This will save teh attributes (attached components) of the player's ship
    // Also will save the components that the player has unlocked
    public void SaveData() {
        LS_DataManager.SavePlayerData(player);
    }

    private void AwakeSingleton() {
        //Check if instance already exists
        if(instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if(instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    private void InitRaceVars() {
        parentOf_Players = GameObject.Find("parentOf_Players").transform;
        parentOf_SpawnPoints = GameObject.Find("parentOf_SpawnPoints").transform;
        var prefab_PlayerGO = Resources.Load("prefab_Player4") as GameObject;
        //OutOfBoundsDisplay = GameObject.Find("OutOfBounds");
        //if(OutOfBoundsDisplay == null)
        //    Debug.Log("Cant find out of bounds display!");


        prefab_Player = prefab_PlayerGO.GetComponent<LS_PlayerController>();
        if(parentOf_Players == null || parentOf_SpawnPoints == null || prefab_Player == null)
            Debug.LogError("Failed to init race vars.");
        initRaceVars = true;
        Debug.Log("InitRaceVars");
    }

    private GameObject SpawnNewPlayer() {
        int numChilds = parentOf_Players.childCount;
        if(!initRaceVars) {
                Debug.LogError("Did not init racevars.");
        }
        // TODO here we need to read form local variable player and dynamically load our objects onto the ship
        GameObject newPlayer = PhotonNetwork.Instantiate(prefab_Player.name, SelectSpawnPoint().position, prefab_Player.transform.rotation, 0);
        newPlayer.transform.SetParent(parentOf_Players);
        return newPlayer;
    }

    private Transform SelectSpawnPoint() {
        int spawnIndex = UnityEngine.Random.Range(0, parentOf_SpawnPoints.childCount - 1);
        return parentOf_SpawnPoints.GetChild(spawnIndex);
    }

    // Leaving our entering bounds
    public void TransitionBounds(bool leavingBounds) {
        // TODO: HACK! we ignore the first collision (likely caused by spawning the player in the bounds)
        numOutOfBounds++;
        Debug.Log("Num times " + numOutOfBounds);
        if(numOutOfBounds <= 1)
            return;


        
        OutOfBoundsDisplay = GameObject.Find("Canvas").transform.Find("OutOfBounds").gameObject;
        if(OutOfBoundsDisplay == null)
            Debug.Log("========Cant find out of bounds display!");
        //else
        //    Debug.Log("========Found!");

        //Debug.Log("Leaving bounds: " + leavingBounds);
        OutOfBoundsDisplay.SetActive(leavingBounds);
        inBounds = !leavingBounds;
    }

    private unsafe void ChangeHealth(int delta) {
        if(delta < 0) {
            int deltaMagnitude = Mathf.Abs(delta);
            int healthAllocation = Player.AmountHealthToSubtract(player.health_armor, deltaMagnitude);
            player.health_armor -= healthAllocation;
            deltaMagnitude -= healthAllocation;

            if(deltaMagnitude > 0){
            healthAllocation = Player.AmountHealthToSubtract(player.health_thruster, deltaMagnitude);
            player.health_thruster -= healthAllocation;
            deltaMagnitude -= healthAllocation;
            }

            if(deltaMagnitude > 0){
            healthAllocation = Player.AmountHealthToSubtract(player.health_weapon, deltaMagnitude);
            player.health_weapon -= healthAllocation;
            deltaMagnitude -= healthAllocation;
            }
            
            if(deltaMagnitude > 0){
            healthAllocation = Player.AmountHealthToSubtract(player.health_hull, deltaMagnitude);
            player.health_hull -= healthAllocation;
            deltaMagnitude -= healthAllocation;
            }
        }
        Debug.Log("Health: " + player.health_armor + " | " + player.health_hull + " | "
            + player.health_thruster + " | " + player.health_weapon);
        currentPlayer.UpdateHealth();
    }

    private void Update() {
        if(inGame && !inBounds) {
            if(Time.fixedTime > lastHealthTick + HEALTH_TICK*Time.deltaTime) {
                ChangeHealth(LS_Bounds.BOUNDS_PENALTY);
                lastHealthTick = Time.fixedTime;
            }
        }
    }
}
