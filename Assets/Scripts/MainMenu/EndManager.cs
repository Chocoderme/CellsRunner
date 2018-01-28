using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour {

	[SerializeField] private TMPro.TextMeshProUGUI PlayerWinner;
	[SerializeField] private GameObject winnerPanel;
	private int idWinner;

	// Use this for initialization
	void Start () {
		winnerPanel.SetActive(true);
		idWinner = PlayerPrefs.GetInt ("PlayerWinner");
		PlayerWinner.SetText ("Player " + (idWinner + 1) + " wins !");
		setRanking ();
	}

	public void GoToMenu() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/MainMenu");
	}

	public void QuitGame() {
		Application.Quit ();
	}

	public void setRanking() {

		int nbPlayer = PlayerPrefs.GetInt ("NbPlayer");
		int i;
		for (i = 0; i < nbPlayer; i++) {
			int player = PlayerPrefs.GetInt ("Ranking" + i);

			if (i == 0) {
				PlayerWinner.SetText ("Player " + (player + 1) + " wins !");
			}

			GameObject ranking = GameObject.Find ("Ranking" + (i + 1));
			ranking.GetComponentInChildren<TMPro.TextMeshProUGUI> ().SetText ("Joueur " + (player + 1));
		}
		for (i = i; i < 4; i++) {
			GameObject.Find ("Ranking" + (i + 1)).SetActive (false);
		}



	}

}
