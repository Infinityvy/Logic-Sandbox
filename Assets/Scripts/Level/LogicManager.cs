using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public static LogicManager active;

    public LevelMesh levelMesh;     

    private void Awake()
    {
        active = this;
    }

    void Update()
    {
        
    }

}
