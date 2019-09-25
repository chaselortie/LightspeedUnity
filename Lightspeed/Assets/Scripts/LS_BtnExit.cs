using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_BtnExit : MonoBehaviour {
    public void Exit() {
        LS_LocalGameController.instance.GoToGarage();
    }
}
