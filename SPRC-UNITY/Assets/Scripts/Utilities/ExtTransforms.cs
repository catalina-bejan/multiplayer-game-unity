using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtTransforms
{
   public static void DestroyChildern(this Transform transform, bool DestroyImmediate = false)
    {
        foreach(Transform child in transform)
        {
            if (DestroyImmediate) MonoBehaviour.DestroyImmediate(child.gameObject);
            else MonoBehaviour.Destroy(child.gameObject);
        }
    }
}
