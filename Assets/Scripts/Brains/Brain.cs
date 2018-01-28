using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    public virtual void Think() { }

    public virtual void Act(Movement mv) { }
}
