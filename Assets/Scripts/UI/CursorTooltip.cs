using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorTooltip : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 cursorPosition = Input.mousePosition;
        transform.position = cursorPosition;


    }
}
