using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LS_newGame_button : MonoBehaviour {
	public void RestartGame () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
