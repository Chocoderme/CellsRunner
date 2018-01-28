using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	const string TrapTag = "Trap";
    const string SpeedTag = "SpeedBoost";
	const string CoffeeTag = "Coffee";

	[SerializeField] private float speed = 5;
	[SerializeField] private float boostSpeed = 10;
	[SerializeField] private float boostQuantity = 3;
	[SerializeField] private float totalStaticTime = 2;
	[SerializeField] private float slowSpeed = 2.5f;
	[SerializeField] private float maxBoost = 3;
	[SerializeField] private Vector2 direction;
	private float staticTime;
	private bool canMove;
	private bool boostActivate;
	private bool inTrap;
    private bool inSpeedBoost = false;
	public bool isStun = false;
	private bool stunSound = true;

	private Rigidbody2D rigidbodyPlayer;

	public void Start() {
		rigidbodyPlayer = GetComponent<Rigidbody2D> ();
		canMove = true;
		boostActivate = false;
	}

	void checkMoveStatus() {
		staticTime += Time.deltaTime;
		if (this.staticTime >= this.totalStaticTime) {
			if (isStun == false) {
				canMove = true;
				stunSound = true;
				//
			} else {
				if (stunSound) {
					this.gameObject.GetComponent<SoundManager> ().onWhiteGlobuleTouched ();
					stunSound = false;
					StartCoroutine (ShakeCamera ());
				}
				canMove = false;
			}
		}
	}

	private IEnumerator ShakeCamera()
	{
			GameObject.Find ("CameraBox").GetComponent<Animator> ().SetBool ("ShouldShake", true);
			yield return new WaitForSeconds(.2f);
			GameObject.Find ("CameraBox").GetComponent<Animator> ().SetBool ("ShouldShake", false);
	}

	public void giveBoost(float boost) {
		this.gameObject.GetComponent<SoundManager> ().onCoffeeTaken ();
		this.boostQuantity += boost;
		if (this.boostQuantity > this.maxBoost) {
			this.boostQuantity = this.maxBoost;
		}
	}


	float defineSpeed() {
		if ((!boostActivate || boostQuantity <= 0) && !this.inTrap && !this.inSpeedBoost)
			return (this.speed);
		if (this.inTrap && this.boostActivate) {
			return this.speed;
		} else if (this.inTrap && !boostActivate) {
			return this.slowSpeed;
		}
        if (this.inSpeedBoost && this.boostActivate)
        {
            return this.boostSpeed + (this.boostSpeed - this.speed);
        } else if (this.inSpeedBoost && !boostActivate)
        {
            return this.boostSpeed;
        }
		this.boostQuantity -= Time.deltaTime;
        if (boostQuantity < 0) boostQuantity = 0f;
		return (this.boostSpeed);
	}

	public void Move() {

        checkMoveStatus();
        if (canMove)
        {
				
            float finalSpeed = defineSpeed();

			if (direction == Vector2.zero) {
				rigidbodyPlayer.AddForce(direction * finalSpeed * Time.fixedDeltaTime * 100);
//				rigidbodyPlayer.velocity = Vector2.zero;
//				rigidbodyPlayer.angularVelocity = 0;
			} else {
				this.gameObject.transform.Translate (direction * finalSpeed * Time.fixedDeltaTime);
			}


			//
        }
	}
		
	public void setContaminatedStatus() {
		if (rigidbodyPlayer != null) {
			rigidbodyPlayer.velocity = Vector2.zero;
			rigidbodyPlayer.angularVelocity = 0;
		}
		this.canMove = false;
		this.staticTime = 0;
	}

	public bool getCanMove() {
		return this.canMove;
	}

	public void setCanMove(bool canMove) {
		this.canMove = canMove;
	}

	public void setDirection(Vector2 direction) {
		this.direction = direction;
	}

	public void setBoost(bool boost) {
		this.boostActivate = boost;
	}

	void OnCollisionEnter2D(Collision2D collider) {
        checkOneWayWall();
	}

    private void OnCollisionExit2D(Collision2D collision)
    {
        checkOneWayWall();
    }

    public void checkOneWayWall()
    {
        const string cleanFilter = "CleanFilter";
        const string contaminatedFilter = "ContaminedFilter";

        var go = GameObject.FindGameObjectWithTag(cleanFilter);
        if (go != null)
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), go.GetComponent<Collider2D>(), !GetComponent<LifePlayer>().getContaminated());
        go = GameObject.FindGameObjectWithTag(contaminatedFilter);
        if (go != null)
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), go.GetComponent<Collider2D>(), GetComponent<LifePlayer>().getContaminated());
    }

    void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == TrapTag) {
			Debug.Log ("Wallah ! Je suis dans un piège !");
			inTrap = true;
		}
        if (collider.gameObject.tag == SpeedTag)
        {
            Debug.Log("Wallah ! Je suis sur un tapis roulant !");
            inSpeedBoost = true;
        }
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.tag == TrapTag) {
			Debug.Log ("Wallah ! Je suis plus dans le piège !");
			inTrap = false;
		}
        if (collider.gameObject.tag == SpeedTag)
        {
            Debug.Log("Wallah ! Ah bah y'a plus d'tapis roulant !");
            inSpeedBoost = false;
        }
	}

    public float getBoost() { return boostQuantity; }
    public float getMaxBoost() { return maxBoost; }

}
