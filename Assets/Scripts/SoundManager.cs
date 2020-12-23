using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip shatterSfx;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioClip metalHitSound;
    [SerializeField] private AudioClip hitSound;

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
        sfx.PlayOneShot(metalHitSound, sfx.volume * .5f);
    }

    public void playHitSfx()
    {
        sfx.PlayOneShot(hitSound);
    }

    public void playThrowSfx()
    {
        sfx.PlayOneShot(throwSound);
    }

    public void playShatterSfx()
    {
        sfx.PlayOneShot(shatterSfx, sfx.volume * .7f);
    }
}
