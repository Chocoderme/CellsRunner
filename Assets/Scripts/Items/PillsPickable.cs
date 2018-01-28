using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillsPickable : IPickable {

    [Header("PillsPickable Parameters")]
    [SerializeField]
    private float healthGiven = 10f;

    public override void Pickup()
    {
        base.Pickup();

        var life = lastTrigger.GetComponent<LifePlayer>();
        if (life != null)
        {
            life.AddLife(healthGiven);
        }
    }
}
