using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    private string text;

    private void FixedUpdate()
    {
        textMesh.text = text;
    }

    void Update()
    {
        text = ((int)(1 / Time.deltaTime)).ToString();
    }
}
