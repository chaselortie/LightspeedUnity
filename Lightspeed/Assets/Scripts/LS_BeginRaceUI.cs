using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_BeginRaceUI : MonoBehaviour {
    [SerializeField] TMPro.TextMeshProUGUI txtCountdown;
    [SerializeField] TMPro.TextMeshProUGUI txtSecondsElapsed;
    [SerializeField] GameObject controls;
    raceState state;
    private enum raceState { init, ready, set, go, racing };
    int secondsElapsed = 0;
    bool raceHasBegun = false;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.H))
            ToggleControlDisplay();
    }

    void ToggleControlDisplay() {
        controls.SetActive(!controls.activeInHierarchy);
    }

    // START AFTER 4 SECONDS...
    // Use this for initialization
    void Start() {
        UnityEngine.Assertions.Assert.IsNotNull(txtCountdown);
        UnityEngine.Assertions.Assert.IsNotNull(txtSecondsElapsed);
        UnityEngine.Assertions.Assert.IsNotNull(controls);

        controls.SetActive(false);
        state = raceState.init;
        InvokeRepeating("IncrementState", 0f, 1f);
    }

    void IncrementState() {
        switch(state) {
            case raceState.init: {
                    txtSecondsElapsed.text = "";
                    txtCountdown.text = "3\n<size=60%>Ready...";
                    state = raceState.ready;
                    break;
                }
            case raceState.ready: {
                    txtCountdown.text = "2\n<size=60%>Ready...";
                    state = raceState.set;
                    break;
                }
            case raceState.set: {
                    txtCountdown.text = "1\n<size=60%>Set...";
                    state = raceState.go;
                    break;
                }
            case raceState.go: {

                    txtCountdown.text = "Race!";
                    state = raceState.racing;
                    break;
                }
            case raceState.racing: {
                    if(!raceHasBegun) {
                        controls.SetActive(true);
                        raceHasBegun = true;
                        LS_LocalGameController.instance.RaceHasBegun = true;
                    }
                    txtCountdown.text = "";
                    txtSecondsElapsed.text = "" + secondsElapsed;
                    secondsElapsed++;
                    break;
                }
        }
    }

    // Ends timer and returns time elapsed
    public int StopTimer() {
        CancelInvoke();
        return secondsElapsed;
    }
}
