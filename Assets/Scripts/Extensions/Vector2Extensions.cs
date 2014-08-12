using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public static class Vector2Extensions
    {
        public static Vector3 toVector3(this Vector2 v)
        {
            return new Vector3(v.x, 0, v.y);
        }

    }
