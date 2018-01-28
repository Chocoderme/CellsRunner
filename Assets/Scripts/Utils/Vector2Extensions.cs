using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions {

    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        Vector2 r = Quaternion.Euler(0, 0, degrees) * v;
        return r;
    }
}
