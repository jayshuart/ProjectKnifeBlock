﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip shatterSfx;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioClip metalHitSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip clickSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playMetalHitSfx()
    {
        sfx.PlayOneShot(metalHitSound, sfx.volume * .32f);
    }

    public void playHitSfx()
    {
        sfx.PlayOneShot(hitSound, sfx.volume * .6f);
    }

    public void playThrowSfx()
    {
        sfx.PlayOneShot(throwSound, sfx.volume * .8f);
    }

    public void playShatterSfx()
    {
        sfx.PlayOneShot(shatterSfx, sfx.volume * .2f);
    }

    public void playClickSfx()
    {
        sfx.PlayOneShot(clickSound, .7f);
    }

    public void playSound(AudioClip clip)
    {
        sfx.PlayOneShot(clip, sfx.volume * .7f);
    }
}
