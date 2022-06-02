using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public static LogicManager active;

    private static readonly byte[] inputIDs = { 3, 4, 11, 12};
    private static readonly byte[] outputIDs = { 9, 10, 13, 14};

    public LevelMesh levelMesh;     

    private void Awake()
    {
        active = this;
    }

    void Update()
    {
        
    }

    public static bool isInputID(byte id)
    {
        for (int i = 0; i < inputIDs.Length; i++)
            if (id == inputIDs[i]) return true;
        return false;
    }

    public static bool isOutputID(byte id)
    {
        for (int i = 0; i < outputIDs.Length; i++)
            if (id == outputIDs[i]) return true;
        return false;
    }
}
