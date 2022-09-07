using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialKeysManager : MonoBehaviour
{
    public Image background_cover;

    public Transform shift_Key;
    public Transform[] wasd_Keys; //in order W, A, S, D

    public AudioSource audioSource;

    public AudioClip key_press_down;
    public AudioClip key_press_up;

    private Animator shift_Animator;
    private Animator w_Animator;
    private Animator a_Animator;
    private Animator s_Animator;
    private Animator d_Animator;

    void Start()
    {
        background_cover.enabled = false;
        shift_Animator = shift_Key.GetComponent<Animator>();
        w_Animator = wasd_Keys[0].GetComponent<Animator>();
        a_Animator = wasd_Keys[1].GetComponent<Animator>();
        s_Animator = wasd_Keys[2].GetComponent<Animator>();
        d_Animator = wasd_Keys[3].GetComponent<Animator>();
        wasd_KeysVisible(false);
    }

    void Update()
    {
        //Shift Key
        if (Input.GetKeyDown(InputSettings.edit))
        {
            shift_Animator.SetBool("Pressed", true);
            wasd_KeysVisible(true);
            background_cover.enabled = true;
        }
        else if (Input.GetKeyUp(InputSettings.edit))
        {
            shift_Animator.SetBool("Pressed", false);
            wasd_KeysVisible(false);
            background_cover.enabled = false;
        }

        if (Input.GetKey(InputSettings.edit))
        {
            //W Key
            if (Input.GetKeyDown(InputSettings.up))
            {
                w_Animator.SetBool("Pressed", true);
                playSound(key_press_down);
            }
            else if (Input.GetKeyUp(InputSettings.up))
            {
                w_Animator.SetBool("Pressed", false);
                playSound(key_press_up);
            }

            //A Key
            if (Input.GetKeyDown(InputSettings.left))
            {
                a_Animator.SetBool("Pressed", true);
                playSound(key_press_down);
            }
            else if (Input.GetKeyUp(InputSettings.left))
            {
                a_Animator.SetBool("Pressed", false);
                playSound(key_press_up);
            }

            //S Key
            if (Input.GetKeyDown(InputSettings.down))
            {
                s_Animator.SetBool("Pressed", true);
                playSound(key_press_down);
            }
            else if (Input.GetKeyUp(InputSettings.down))
            {
                s_Animator.SetBool("Pressed", false);
                playSound(key_press_up);
            }

            //D Key
            if (Input.GetKeyDown(InputSettings.right))
            {
                d_Animator.SetBool("Pressed", true);
                playSound(key_press_down);
            }
            else if (Input.GetKeyUp(InputSettings.right))
            {
                d_Animator.SetBool("Pressed", false);
                playSound(key_press_up);
            }
        }
    }

    private void wasd_KeysVisible(bool state)
    {
        foreach(Transform key in wasd_Keys)
        {
            key.GetComponent<Image>().enabled = state;
        }
    }

    private void playSound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
