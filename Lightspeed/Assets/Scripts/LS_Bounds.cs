using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_Bounds : MonoBehaviour {

    public const int BOUNDS_PENALTY = -10;
    public int numAsteroids;
    public int numPlanets;
    public int numRocks;
    public int numSmoke;
    private GameObject prefab_asteroid;
    private GameObject prefab_planet;
    private GameObject prefab_rock;
    private GameObject prefab_Smoke;
    [SerializeField] private Transform parentOf_Obstacles;
    private BoxCollider box;

    Vector3 centerOfBounds;
    float xMax;
    float yMax;
    float zMax;

    // Use this for initialization
    void Start() {
        box = GetComponent<BoxCollider>();
        prefab_asteroid = Resources.Load("prefab_Asteroid") as GameObject;
        prefab_planet = Resources.Load("FullPlanetFinal (5)") as GameObject;
        prefab_rock = Resources.Load("prefab_Rock") as GameObject;
        prefab_Smoke = Resources.Load("prefab_Smoke") as GameObject;

        UnityEngine.Assertions.Assert.IsNotNull(prefab_asteroid);
        UnityEngine.Assertions.Assert.IsNotNull(prefab_planet);
        UnityEngine.Assertions.Assert.IsNotNull(prefab_rock);
        UnityEngine.Assertions.Assert.IsNotNull(prefab_Smoke);
        UnityEngine.Assertions.Assert.IsNotNull(parentOf_Obstacles);
        UnityEngine.Assertions.Assert.IsNotNull(box);

        centerOfBounds = box.center;
        xMax = (box.size.x - centerOfBounds.x) / 2f;
        yMax = (box.size.y - centerOfBounds.y) / 2f;
        zMax = (box.size.z - centerOfBounds.z) / 2f;

        SpawnObstacles();
    }

    void SpawnObstacles() {
        for(int i = 0; i < numPlanets; i++) {
            SpawnObject(prefab_planet, 3.3f);
        }
        for(int i = 0; i < numAsteroids; i++) {
            SpawnObject(prefab_asteroid, 5);
        }
        for(int i = 0; i < numRocks; i++) {
            SpawnObject(prefab_rock, 15);
        }
        for(int i = 0; i < numSmoke; i++) {
            SpawnObject(prefab_Smoke, 40);
        }
    }

    private void SpawnObject(GameObject prefab_obj, float maxScale = 0) {
        Vector3 offset = new Vector3(Random.Range(-xMax, xMax), Random.Range(-yMax, yMax), Random.Range(-zMax, zMax));
        GameObject clone_obj = Instantiate(prefab_obj, centerOfBounds + offset, Random.rotation, parentOf_Obstacles);

        if(maxScale != 0) {
            float scale = Random.Range(1.0f, maxScale);
            clone_obj.transform.localScale = Vector3.one * scale;
        }
    }

    private void OnTriggerExit(Collider other) {
        // Only care about player collisions
        if(!other.tag.Equals("Player"))
            return;

        Debug.Log("ExitBy:" + other.name + " " + other.tag);
        //Debug.Log("OnTriggerExit - Bounds");
        if(LS_LocalGameController.instance == null)
            Debug.LogWarning("No local game controller!");
        else
            LS_LocalGameController.instance.TransitionBounds(true);
    }

    private void OnTriggerEnter(Collider other) {
        // Only care about player collisions
        if(!other.tag.Equals("Player"))
            return;
        //Debug.Log("EnterBy:" + other.name + " " + other.tag);
        //Debug.Log("OnTriggerEnter - Bounds");
        if(LS_LocalGameController.instance == null)
            Debug.LogWarning("No local game controller!");
        else
            LS_LocalGameController.instance.TransitionBounds(false);
    }

}
