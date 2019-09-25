using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_RaceManager : MonoBehaviour {
    [SerializeField] TMPro.TextMeshProUGUI txtResult;
    [SerializeField] UnityEngine.UI.Button btnReturn;
    [SerializeField] LS_FinishScript destination;
    public List<Material> materials;
    GameObject AI;
    Transform parentOf_SpawnPoints;

    int numAI;
	// Use this for initialization
	void Start () {
		UnityEngine.Assertions.Assert.IsNotNull(txtResult);
        UnityEngine.Assertions.Assert.IsNotNull(btnReturn);
        UnityEngine.Assertions.Assert.IsNotNull(destination);
        txtResult.transform.parent.gameObject.SetActive(false);
        btnReturn.gameObject.SetActive(false);
        destination.manager = this;
        AI = (GameObject)Resources.Load("AircraftJetAI");
        parentOf_SpawnPoints = GameObject.Find("parentOf_SpawnPointsAI").transform;
        for (int i = 0; i < parentOf_SpawnPoints.childCount; i++)
        {
            GameObject AIShip = Instantiate(AI, parentOf_SpawnPoints.GetChild(i).position, Quaternion.Euler(0f, 90f, 0f));
            AIShip.transform.GetChild(3).gameObject.GetComponent<MeshRenderer>().material = materials[i];
        }
    }

    public void End(bool winner) {
        txtResult.transform.parent.gameObject.SetActive(true);
        txtResult.text = winner ? "Winner!!!!\n<size=60%>+100 Credits" : "Loser :(\n<size=60%>+50 Credits";
        LS_LocalGameController.instance.GetPlayer().walletSC += winner ? 100 : 50;
        LS_LocalGameController.instance.SaveData();
        btnReturn.gameObject.SetActive(true);
    }
}
