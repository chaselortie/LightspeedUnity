using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles the local side of the player (such as manipulating movement and animation)
/// If this script is not owned by the local client it will be disabled.
/// </summary>
public class LS_PlayerController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private GameObject shipCanvas;
    [SerializeField] private GameObject spaceship;
    [SerializeField] private TMPro.TextMeshProUGUI txtHealth_Shield;
    [SerializeField] private TMPro.TextMeshProUGUI txtHealth_Engine;
    [SerializeField] private TMPro.TextMeshProUGUI txtHealth_Weapon;
    [SerializeField] private TMPro.TextMeshProUGUI txtHealth_Thruster;
    private float autoFadeTime = -1; // If less than 0, it means nothing. Else its when we auto hide the health.
    private const float HEALTH_DISPLAY_LENGTH = 5;
    // Use this for initialization
    void Start() {
        Init();
        LS_LocalGameController.instance.SetCurrentPlayer(this);
        UnityEngine.Assertions.Assert.IsNotNull(shipCanvas);
        UnityEngine.Assertions.Assert.IsNotNull(spaceship);
        // Override for orientation TODO: loop through game controller
        spaceship = gameObject;
        var sun = GameObject.Find("Sun");
        if(sun == null)
            Debug.LogError("404: Sun not found.");

        // Aim towards Sun!
        Vector3 targetDir = sun.transform.position - transform.position;
        float step = speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 100, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void Init() {
        shipCanvas.SetActive(false);
    }

    public void UpdateHealth() {
        txtHealth_Shield.text =     "Armor\n" + LS_LocalGameController.instance.GetPlayer().health_armor + "%";
        txtHealth_Engine.text =     "Hull\n" + LS_LocalGameController.instance.GetPlayer().health_hull + "%";
        txtHealth_Thruster.text =   "Thruster\n" + LS_LocalGameController.instance.GetPlayer().health_thruster + "%";
        txtHealth_Weapon.text =     "Gun\n" + LS_LocalGameController.instance.GetPlayer().health_weapon + "%";
        shipCanvas.SetActive(true);
        autoFadeTime = Time.fixedTime + HEALTH_DISPLAY_LENGTH;
    }

    // Update is called once per frame
    void Update() {
        bool show = Input.GetButtonDown("ShowStatus");  // Spacebar on desktop, little ship icon on mobile
        bool hide = Input.GetButtonUp("ShowStatus");    // Spacebar on desktop, little ship icon on mobile

        // Auto hide health display if needed
        if(autoFadeTime > 0 && Time.fixedTime > autoFadeTime) {
            hide = true;
        }

        if(show) {
            shipCanvas.SetActive(true);
            autoFadeTime = -1;
        }
        else if(hide) {
            shipCanvas.SetActive(false);
            autoFadeTime = -1;
        }
        //transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            Vector3 oldScale = spaceship.transform.localScale;
            spaceship.transform.localScale = (oldScale * 1.1f);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            Vector3 oldScale = spaceship.transform.localScale;
            spaceship.transform.localScale = (oldScale / 1.1f);
        }
    }
}
