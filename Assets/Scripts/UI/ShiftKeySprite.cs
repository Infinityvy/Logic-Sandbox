using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftKeySprite : MonoBehaviour
{
    public Animator animator;
    void Update()
    {
        if (Input.GetKeyDown(InputSettings.edit))
        {
            animator.SetBool("Pressed", true);
        }
        else if(Input.GetKeyUp(InputSettings.edit))
        {
            animator.SetBool("Pressed", false);
        }
    }
}
