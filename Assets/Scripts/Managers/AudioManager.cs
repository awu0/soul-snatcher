using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource eyeAbility;
    public AudioSource bugAbility;
    public AudioSource golemAbility;
    public AudioSource snakeAbility;
    public AudioSource eyeDeath;
    public AudioSource bugDeath;
    public AudioSource golemDeath;
    public AudioSource snakeDeath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playEyeAbility()
    {
        eyeAbility.Play();
    }

    public void playEyeDeath()
    {
        eyeDeath.Play();
    }

    public void playBugAbility()
    {
        bugAbility.Play();
    }
    public void playBugDeath()
    {
        bugDeath.Play();
    }

    public void playSnakeAbility()
    {
        snakeAbility.Play();
    }

    public void playSnakeDeath()
    {
        snakeDeath.Play();
    }


    public void playGolemAbility()
    {
        golemAbility.Play();
    }

    public void playGolemDeath()
    {
        golemDeath.Play();
    }
}
