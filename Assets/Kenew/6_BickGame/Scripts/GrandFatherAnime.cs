using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandFatherAnime : MonoBehaviour
{
    public Animator animator;
    private bool isDance = false;
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            isDance = true;
        }
        else
        {
            isDance = false;
        }
        
        animator.SetBool("Dance", isDance);
    }
}
