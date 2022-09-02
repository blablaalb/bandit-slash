using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugHelper;

public static class Physics2DExtensions
{
    public static Collider2D BoxCastLeft(this BoxCollider2D collider, float width = 0.01f, float truncasteHeight = 0.1f)
    {
        var layerMask = ~(1 << collider.transform.gameObject.layer);
        return BoxCastLeft(collider, layerMask, width, truncasteHeight);
    }

    public static Collider2D BoxCastLeft(this BoxCollider2D collider, int layerMask, float width = 0.01f, float truncasteHeight = 0.1f)
    {
        var size = collider.size;
        size.y -= truncasteHeight;
        size.x = width;
        var origin = collider.bounds.center;
        origin.x -= collider.size.x * 0.5f + size.x * 0.5f;
        var hit = Physics2D.OverlapBox(origin, size, 0f, layerMask);
        Drawer.Instance.DrawCube(origin, size, color: Color.red, time: Time.deltaTime);
        if (hit != null) Debug.Log($"Left hit: {hit.name}", hit);
        return hit;
    }

    public static Collider2D BoxCastRight(this BoxCollider2D collider, float width = 0.01f, float truncasteHeight = 0.1f)
    {
        var layerMask = ~(1 << collider.transform.gameObject.layer);
        return BoxCastLeft(collider, layerMask, width, truncasteHeight);
    }

    public static Collider2D BoxCastRight(this BoxCollider2D collider, int layerMask, float width = 0.01f, float truncasteHeight = 0.1f)
    {
        var size = collider.size;
        size.y -= truncasteHeight;
        size.x = width;
        var origin = collider.bounds.center;
        origin.x += collider.size.x * 0.5f + size.x * 0.5f;
        var hit = Physics2D.OverlapBox(origin, size, 0f, layerMask);
        Drawer.Instance.DrawCube(origin, size, color: Color.red, time: Time.deltaTime);
        if (hit != null) Debug.Log($"Right hit: {hit.name}", hit);
        return hit;
    }

}
