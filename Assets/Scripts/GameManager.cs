using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField] private GameObject voiture_qui_claque_wallah;
    [SerializeField] private GameObject voiture_qui_claque_wallah_ROBOT;
    private List<GameObject> players;
	private bool withIA;
	private bool end;
	private GameObject winner;


	// Use this for initialization
	void Start () {

		players = new List<GameObject> ();
		end = false;
		initGame ();

	}

	// Update is called once per frame
	void Update () {

		//checkIfVirusIsTransmitted ();
		checkIfGameIsFinished ();

	}

	void initGame() {

		Debug.Log ("Initialiazation of the game");
		definePlayer ();
		defineWhoIsContaminated ();
		definePositionOfPlayer ();

	}
		
	void definePlayer() {

        //CODE TEMPORAIRE
        int playerCount = PlayerPrefs.GetInt("PlayerCount", 2);
		for (int i = 0; i < playerCount; i++) {
			GameObject player = Instantiate (voiture_qui_claque_wallah) as GameObject;
            player.GetComponent<Player>().SetId(i);
			//player.GetComponent<PlayerController> ().setIdPlayer (i);
			players.Add (player);
		}
		/*GameObject IA = Instantiate(voiture_qui_claque_wallah_ROBOT) as GameObject;
		players.Add(IA);*/
		/*GameObject IA2 = Instantiate(voiture_qui_claque_wallah_ROBOT) as GameObject;
		players.Add(IA2);*/
	}

	void definePositionOfPlayer() {


		const string ContaminatedSpawnPoint = "ContaminatedSpawnPoint";
		const string CleanSpawnPoint = "CleanSpawnPoint";
		GameObject listSpawnPoint = GameObject.Find ("SpawnPoints");
		int nbCleanSpawnPointUsed = 1;

		foreach (GameObject player in players) {
			if (player.GetComponent<LifePlayer> ().getContaminated()) {
				GameObject ContaminatedSpawnPointObject = GameObject.Find (ContaminatedSpawnPoint);
				player.transform.position = ContaminatedSpawnPointObject.transform.position;
			} else {
				GameObject CleanSpawnPointObject = GameObject.Find (CleanSpawnPoint + nbCleanSpawnPointUsed);
				nbCleanSpawnPointUsed += 1;
				player.transform.position = CleanSpawnPointObject.transform.position;
			}
		}

	}

	void defineWhoIsContaminated() {

		int nbPlayer = players.Count;
		System.Random rnd = new System.Random ();
		int randomNumber = rnd.Next (0, nbPlayer - 1);
		players[randomNumber].GetComponent<LifePlayer> ().setContaminated (true);

	}

	void startEndOfGame(int idPlayerWinner) {
		PlayerPrefs.SetInt("PlayerWinner", idPlayerWinner);
		PlayerPrefs.SetInt ("NbPlayer", players.Count);
		int ranking = 0;
		players.Sort((b, a) => {
			return (a.GetComponent<LifePlayer>().getLife().CompareTo(b.GetComponent<LifePlayer>().getLife()));
		});

		foreach (GameObject player in players) {

			PlayerPrefs.SetInt ("Ranking" + ranking, player.GetComponent<Player>().id);
			ranking++;

		}
		UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/EndGame");
	}

	void checkIfGameIsFinished() {
		int playerId = 0;
		foreach (GameObject player in players) {
			if (!(player.GetComponent<LifePlayer> ().isAlive ())) {
				end = true;
				playerId = player.GetComponent<Player>().id;
				break;
			}
		}
		if (end == true)
			startEndOfGame (playerId);

	}
}
