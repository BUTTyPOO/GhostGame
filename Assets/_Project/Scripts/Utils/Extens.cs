using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtenMeths
{
    public static class Extens
    {
        public static Vector3 AddFloat(this Vector3 vec, float num)
        {
            vec.x += num;
            vec.y += num;
            vec.z += num;
            return vec;
        }
    }
}
