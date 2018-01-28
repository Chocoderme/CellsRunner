using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePlayer : MonoBehaviour {

	[SerializeField] private float life;
    private float initialLife;
	[SerializeField] private float totalTimeCantGiveVirus;
	private bool isDead;
	private bool isContaminated;
	private bool cantGiveVirus;
	private float timeCantGiveVirus;

	// Use this for initialization
	void Start () {
		isDead = false;
		cantGiveVirus = false;
        initialLife = life;
	}

	// Update is called once per frame
	void Update () {

		checkIfCanGiveVirus ();

		if (this.isContaminated) {
			life -= Time.deltaTime;
			//Debug.Log ("Life:" + life);
		}

		if (life <= 0) {
			Debug.Log ("Player is dead !");
			this.gameObject.SetActive (false);
			isDead = true;
		}

		if (!(GetComponent<Renderer>().IsVisibleFromCamera())) {
			Debug.Log ("Player is dead !");
			this.gameObject.SetActive (false);
			isDead = true;
			life = 0f;
		}
	}

	void checkIfCanGiveVirus() {

		if (cantGiveVirus == true) {
			timeCantGiveVirus += Time.deltaTime;
			if (timeCantGiveVirus >= totalTimeCantGiveVirus) {
				cantGiveVirus = false;
			}
		}

	}

	void OnCollisionEnter2D(Collision2D	 collision) {

		if (!GetComponent<Movement> () || !GetComponent<LifePlayer> () || !collision.gameObject.GetComponent<LifePlayer>())
			return;	
		if (collision.gameObject.tag == "Player"
			&& !this.isContaminated
			&& collision.gameObject.GetComponent<LifePlayer>().getContaminated()
			&& collision.gameObject.GetComponent<LifePlayer>().canGiveVirus()) {

			Debug.Log ("AAAAAAAAAH ! YOU ARE SICK");

			//
			this.gameObject.GetComponent<SoundManager>().onVirusTransmitted();

			// SET SICKNESS ON  GAME OBJECT
			this.isContaminated = true;
			this.cantGiveVirus = true;
			this.timeCantGiveVirus = 0;
			GetComponent<Movement> ().setContaminatedStatus ();
			// CANCEL SICKNESS ON OTHER GAME OBJECT
			collision.gameObject.GetComponent<LifePlayer> ().setContaminated (false);
			collision.gameObject.GetComponent<Movement> ().setCanMove (true);

            // RESET COLLISION WALLS
            var mov = GetComponent<Movement>();
            if (mov != null)
                mov.checkOneWayWall();
            mov = collision.gameObject.GetComponent<Movement>();
            if (mov != null)
                mov.checkOneWayWall();
        }
	}

	public bool isAlive() {
		return !isDead;
	}

	public void setContaminated(bool contaminated) {
		this.isContaminated = contaminated;
	}

	public bool getContaminated() {
		return this.isContaminated;
	}

	public bool canGiveVirus() {
		return (!this.cantGiveVirus);
	}

    public void AddLife(float value) {
		life += value; if (life > initialLife) life = initialLife;
		this.gameObject.GetComponent<SoundManager> ().onHealthTaken ();
	}

    public float getLife() { return life; }
    public float getInitialLife() { return initialLife; }
}
