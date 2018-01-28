using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteManager : MonoBehaviour {

    [SerializeField] private Sprite[] normalCell;

    [SerializeField] private Sprite[] infectedCell1;
    [SerializeField] private Sprite[] infectedCell2;
    [SerializeField] private Sprite[] infectedCell3;

    [SerializeField] private float animationTime = .5f;

    private float currentAnimationTime = 0f;
    private int currentIndex = 0;
    private int addValue = 1;

    private int currentState = 0;

    private Sprite[] currentDisplay = null;
    private Sprite[] nextDisplay = null;

    private SpriteRenderer _renderer;
    private LifePlayer life;
    // Use this for initialization
    void Start () {
        _renderer = GetComponent<SpriteRenderer>();
        life = GetComponent<LifePlayer>();
        currentDisplay = normalCell;
	}
	
    private void CheckState()
    {
        if (life != null)
        {
            var percent = (life.getLife() * 100) / life.getInitialLife();
            if (percent < 80 && currentState == 0)
            {
                nextDisplay = infectedCell1;
                currentState = 1;
            }
            else if (percent < 50 && currentState == 1)
            {
                nextDisplay = infectedCell2;
                currentState = 2;
            }
            else if (percent < 20 && currentState == 2)
            {
                nextDisplay = infectedCell3;
                currentState = 3;
            }
        }
    }

	// Update is called once per frame
	void Update () {
        CheckState();

        currentAnimationTime += Time.deltaTime;
        if (currentAnimationTime > animationTime)
        {
            currentAnimationTime -= animationTime;
            currentIndex += addValue;

            if (currentIndex >= currentDisplay.Length && nextDisplay != null)
            {
                currentIndex = 0;
                addValue = 1;
                currentDisplay = nextDisplay;
                nextDisplay = null;
            }
            else if (currentIndex >= currentDisplay.Length)
            {
                addValue = -1;
                currentIndex = currentDisplay.Length - 1;
            }
            else if (currentIndex <= 0)
            {
                addValue = 1;
                currentIndex = 0;
            }
        }

        _renderer.sprite = currentDisplay[currentIndex];
	}
}
