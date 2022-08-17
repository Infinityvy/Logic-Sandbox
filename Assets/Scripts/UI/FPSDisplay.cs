using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    void Update()
    {
        textMesh.text = ((int)(1 / Time.deltaTime)).ToString();
    }
}
