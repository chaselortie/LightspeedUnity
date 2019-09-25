using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays text representing options for a selected component. Returns the selected index.
/// </summary>
public class LS_Garage_ComponentOptions : MonoBehaviour {
    public delegate void OptionSelectedHandler(int index);
    public event OptionSelectedHandler OptionSelected = delegate { };
    Color UNLOCKED = LS_Helper.HexToColor("000000A7");
    Color LOCKED = LS_Helper.HexToColor("5C00009F");

    // These are choices for a certain component type such as bluehull, redhull, greenhull
    List<GameObject> componentOptionChoices;
    const int MAX_OPTIONS = 4;
    void Start() {
        componentOptionChoices = new List<GameObject>();
        // All children of this GO should be componentOptionChoices
        for(int i = 0; i < transform.childCount; i++) {
            int optionIndex = i;
            var currOption = transform.GetChild(i).gameObject;
            transform.GetChild(i).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { OnOptionSelected(optionIndex); });
            currOption.SetActive(false);
            componentOptionChoices.Add(currOption);
        }
        UnityEngine.Assertions.Assert.AreNotEqual(0, componentOptionChoices.Count, "Drag and drop the GOs for ComponentOptionChoices into their slots.");
    }

    public void DisplayComponentOptions(List<ShipComponent> options) {
        gameObject.SetActive(true);
        UnityEngine.Assertions.Assert.IsTrue(options.Count <= MAX_OPTIONS, "The max number of component options" + MAX_OPTIONS + " was exceeded!");
        // Populate buttons with component purchase information
        for(int i = 0; i < options.Count; i++) {
            var currOptionDisplay = transform.GetChild(i).gameObject;
            var fullName = options[i].name.Substring(options[i].name.IndexOf("_")+1);
            bool owned = LS_LocalGameController.instance.GetPlayer().unlockedComponents.Contains(options[i].referenceID);
            currOptionDisplay.GetComponentInChildren<UnityEngine.UI.Image>().color = owned ? UNLOCKED : LOCKED;
            currOptionDisplay.GetComponentInChildren<UnityEngine.UI.Text>().text =  owned ? fullName : fullName  + " - " + options[i].cost + " credits";
            currOptionDisplay.SetActive(true);
        }
        // Disable unused buttons
        for(int i = options.Count; i < transform.childCount; i++) {
            var currOption = transform.GetChild(i).gameObject;
            currOption.SetActive(false);
        }
    }

    public void OnOptionSelected(int index) {
        OptionSelected(index);
        //// Close all buttons
        //for(int i = 0; i < transform.childCount; i++) {
        //    var currOption = transform.GetChild(i).gameObject;
        //    currOption.SetActive(false);
        //}
    }
}
