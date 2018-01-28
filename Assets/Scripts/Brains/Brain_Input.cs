using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain_Input : Brain {

    [SerializeField]
    private float DeadZone = .02f;

    private Vector2 currentDir = Vector2.zero;
	private bool boostActivate = false;

    public string[] JoystickAxises = new string[3] { "P1_Joy_Vert", "P1_Joy_Hor", "P1_Joy_Boost" };
	public string[] KeyboardAxises = new string[3] { "P1_Key_Vert", "P1_Key_Hor", "P1_Key_Boost" };

    public override void Think()
    {
        var vertAxis = Input.GetAxis(KeyboardAxises[0]);
        if (vertAxis == 0)
            vertAxis = Input.GetAxis(JoystickAxises[0]);
        var horAxis = Input.GetAxis(KeyboardAxises[1]);
        if (horAxis == 0)
            horAxis = Input.GetAxis(JoystickAxises[1]);
		var boost = Input.GetButton(KeyboardAxises[2]);
		if (boost == false)
			boost = Input.GetButton(JoystickAxises[2]);


        if (Mathf.Abs(vertAxis) <= DeadZone)
            vertAxis = 0f;
        if (Mathf.Abs(horAxis) <= DeadZone)
            horAxis = 0f;

        currentDir = new Vector2(horAxis, vertAxis);
		boostActivate = boost;
    }

    public override void Act(Movement mv)
    {
        mv.setDirection(currentDir);
		mv.setBoost (boostActivate);
        mv.Move();
    }
}
