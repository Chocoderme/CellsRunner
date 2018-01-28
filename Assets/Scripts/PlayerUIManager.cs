using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour {

    private LifePlayer life;
    private Movement mv;
    private Player player;

    [SerializeField] private GameObject hpBar;
    [SerializeField] private Color initialColor;
    [SerializeField] private Color endColor;

    [SerializeField] private GameObject boostBar;
    [SerializeField] private TMPro.TMP_Text nameText;
    [SerializeField] private Color[] nameColors;

    // Use this for initialization
    void Start () {
        life = GetComponent<LifePlayer>();
        mv = GetComponent<Movement>();
        player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if (life != null && hpBar != null)
        {
            var percent = life.getLife() / life.getInitialLife();
            hpBar.transform.localScale = new Vector3(percent, hpBar.transform.localScale.y, hpBar.transform.localScale.z);
            hpBar.GetComponent<SpriteRenderer>().color = Color.Lerp(initialColor, endColor, 1 - percent);
        }

        if (mv != null && boostBar != null)
        {
            var percent = mv.getBoost() / mv.getMaxBoost();
            boostBar.transform.localScale = new Vector3(percent, boostBar.transform.localScale.y, boostBar.transform.localScale.z);
        }

        if (player != null && nameText != null)
        {
            nameText.SetText("J" + (player.id + 1).ToString());
            if (nameColors.Length > 0)
                nameText.color = nameColors[player.id % nameColors.Length];
        }

	}
}
