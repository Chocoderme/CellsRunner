using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour {

    private Movement mv;

    [SerializeField]
    private Brain normalBrain;
    [SerializeField]
    private Brain infectedBrain;

    private LifePlayer life;

	public int id { get; protected set; }

	// Use this for initialization
	void Start () {
        mv = GetComponent<Movement>();
        if (mv != null)
            mv.checkOneWayWall();

        if (normalBrain == null)
            Debug.LogError("Missing normal brain");

        life = GetComponent<LifePlayer>();
        if (life == null)
            Debug.LogError("Missing component Life");
	}

    public void SetId(int _id)
    {
        id = _id;
        if (normalBrain is Brain_Input)
        {
            for (int i = 0; i < 3; i++)
            {
                (normalBrain as Brain_Input).JoystickAxises[i] = (normalBrain as Brain_Input).JoystickAxises[i].Replace("1", (id + 1).ToString());
                (normalBrain as Brain_Input).KeyboardAxises[i] = (normalBrain as Brain_Input).KeyboardAxises[i].Replace("1", (id + 1).ToString());
            }

        }
        else
            Debug.Log("Not player");
    }

    void Update()
    {
        if (life != null)
        {
            if (normalBrain != null && !life.getContaminated())
                normalBrain.Think();
            if (infectedBrain == null && life.getContaminated() && normalBrain != null)
                normalBrain.Think();
            else if (infectedBrain != null && life.getContaminated())
                infectedBrain.Think();
        }
        else if (normalBrain != null)
            normalBrain.Think();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (life != null)
        {
            if (normalBrain != null && !life.getContaminated() && mv != null)
                normalBrain.Act(mv);
            if (infectedBrain == null && life.getContaminated() && normalBrain != null && mv != null)
                normalBrain.Act(mv);
            else if (infectedBrain != null && life.getContaminated() && mv != null)
                infectedBrain.Act(mv);
        }
        else if (normalBrain != null && mv != null)
            normalBrain.Act(mv);
	}
}
