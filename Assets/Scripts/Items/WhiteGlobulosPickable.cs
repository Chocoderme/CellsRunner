using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteGlobulosPickable : IPickable {

    [Header("WhiteGlobulosPickable Parameters")]
    [SerializeField]
    private float stunTime = 1f;

    public override void Pickup()
    {
        base.Pickup();

        var mv = lastTrigger.GetComponent<Movement>();
        if (mv != null)
        {
            mv.isStun = true;
            StartCoroutine(UnStun(mv));
        }
    }

    private IEnumerator UnStun(Movement mv)
    {
        yield return new WaitForSeconds(stunTime);
        mv.isStun = false;
    }
}
