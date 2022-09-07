using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MySystem
{
    public static Transform getChildByName(this Transform tr, string name)
    {
        for (int i = 0; i < tr.childCount; i++)
        {
            if (tr.GetChild(i).name == name) return tr.GetChild(i);
        }

        return null;
    }

    public static bool sameContentsAs(this byte[] a1, byte[] a2)
    {
        if (a1.Length != a2.Length) return false;

        for(int i = 0; i < a1.Length; i++)
        {
            if (a1[i] != a2[i]) return false;
        }

        return true;
    }
}
