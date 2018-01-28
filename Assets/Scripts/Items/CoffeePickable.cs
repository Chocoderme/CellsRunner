using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeePickable : IPickable {

    [Header("CoffeePickable Parameters")]
    [SerializeField] private float boostGiven = 3f;

    public override void Pickup()
    {
        base.Pickup();

        var mv = lastTrigger.GetComponent<Movement>();
        if (mv != null)
        {
            mv.giveBoost(boostGiven);
        }
    }
}
