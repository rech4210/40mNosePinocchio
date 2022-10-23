using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinocchioPlayer : MonoBehaviour
{
    private PinocchioEnemy pinocchioEnemy;
    
    private int getPower;
    public int GetPower { set; get; }
    public AudioClip Space;
    public AudioClip Space2;
    
    public ParticleSystem particleSystem;

    private void Start()
    {
        pinocchioEnemy = FindObjectOfType<PinocchioEnemy>();
    }
    
    private void Update()
    {
        //if (PinocchioGame.Instance.possibleClick)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                pinocchioEnemy.AddForcePower(100);
                particleSystem.Play();

                float value = UnityEngine.Random.Range(0f, 1f);
                var output = value > 0.5f ? Space : Space2;
                SoundManager.PlaySFX(output);
            }
        }
    }
}
