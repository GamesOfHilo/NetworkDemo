using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Vector3toVector2
{
    public static float getPlainMagnitude(this Vector3 v)
    {
        return v.toPlainVector2().magnitude;
    }
    public static Vector2 getPlainNormelized(this Vector3 v)
    {
        return v.toPlainVector2().normalized;
    }

    public static Vector2 toPlainVector2(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
}