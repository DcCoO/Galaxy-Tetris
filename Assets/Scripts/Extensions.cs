using UnityEngine;

public static class Extensions
{
    public static void DestroyAllChildren(this GameObject g)
    {
        Transform t = g.transform;
        t.DestroyAllChildren();
    }
    
    public static void DestroyAllChildren(this Transform t)
    {
        int childrenCount = t.childCount;
        for (int i = childrenCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(t.GetChild(i).gameObject);
        }
    }
}