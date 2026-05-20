using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Triangle3D
{
    /// <summary>
    /// ŽOŠpŒ`‚Ì“à‘¤‚É‚¢‚é‚©‚ð”»’f
    /// </summary>
    /// <param name="p"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsPointInTriangle(
            Vector3 p,
            Vector3 a,
            Vector3 b,
            Vector3 c)
    {
        Vector3 normal =
            Vector3.Cross(b - a, c - a);

        Vector3 c0 =
            Vector3.Cross(b - a, p - a);

        Vector3 c1 =
            Vector3.Cross(c - b, p - b);

        Vector3 c2 =
            Vector3.Cross(a - c, p - c);

        return
            Vector3.Dot(normal, c0) >= 0 &&
            Vector3.Dot(normal, c1) >= 0 &&
            Vector3.Dot(normal, c2) >= 0;
    }
}
