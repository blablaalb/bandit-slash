using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugHelper;

public static class Transform2DExtensions
{
    public static float DistanceX(this Transform transform, Transform other)
    {
        return Mathf.Abs(transform.position.x - other.position.x);
    }

    public static float DistanceX(this Transform transform, Vector2 other)
    {
        return Mathf.Abs(transform.position.x - other.x);
    }


    public static float DistanceY(this Transform transform, Transform other)
    {
        return Mathf.Abs(transform.position.y - other.position.y);
    }

}
