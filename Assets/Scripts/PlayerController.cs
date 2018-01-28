using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private int idPlayer;

	private Movement movement;

	private KeyCode Up;
	private KeyCode Down;
	private KeyCode Left;
	private KeyCode Right;
	private KeyCode Boost;


	// Use this for initialization		
	void Start () {
		this.movement = new Movement ();
        defineKeyBoard();
	}

	public void setIdPlayer(int id) {
		idPlayer = id;
		this.defineKeyBoard();
	}

	public void GetInput() {

		Vector2 direction = Vector2.zero;
		bool boostActivate = false;

		if (Input.GetKey (Up)) { direction += Vector2.up; }
		if (Input.GetKey (Left)) { direction += Vector2.left; }
		if (Input.GetKey (Down)) { direction += Vector2.down; }
		if (Input.GetKey (Right)) { direction += Vector2.right; }
		if (Input.GetKey (Boost)) { boostActivate = true; }

		this.movement.setDirection (direction);
		this.movement.setBoost (boostActivate);

	}

	void defineKeyBoard() {
		if (idPlayer == 0) {
			this.Up = KeyCode.Z;
			this.Down = KeyCode.S;
			this.Left = KeyCode.Q;
			this.Right = KeyCode.D;
			this.Boost = KeyCode.E;
		} else if (idPlayer == 1) {
			this.Up = KeyCode.UpArrow;
			this.Down = KeyCode.DownArrow;
			this.Left = KeyCode.LeftArrow;
			this.Right = KeyCode.RightArrow;
			this.Boost = KeyCode.Keypad0;
		}
	}

	public Movement getMovement() {
		return this.movement;
	}

}
