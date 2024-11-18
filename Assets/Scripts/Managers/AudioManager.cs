using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource eyeAbility;
    public AudioSource bugAbility;
    public AudioSource golemAbility;
    public AudioSource snakeAbility;

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

    public void playBugAbility()
    {
        bugAbility.Play();
    }

    public void playSnakeAbility()
    {
        snakeAbility.Play();
    }
    public void playGolemAbility()
    {
        golemAbility.Play();
    }
}
