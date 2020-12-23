using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioClip intenseMusic;
    [SerializeField] private AudioClip relaxedMusic;
    // Start is called before the first frame update
    void Start()
    {
        music = SoundManager.Instance.gameObject.GetComponent<AudioSource>();
        switch(PlayerPrefs.GetString("music"))
        {
            case "intense":
                playIntenseMusic();
                break;

            case "relaxed":
                playRelaxedMusic();
                break;

            case "mute":
                muteMusic();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openOptions()
    {
        this.gameObject.SetActive(true);
    }

    public void closeOptions()
    {
        this.gameObject.SetActive(false);
    }

    public void playIntenseMusic(){
        PlayerPrefs.SetString("music", "intense");
        music.clip = intenseMusic;
        music.Play();
    }

    public void playRelaxedMusic(){
        PlayerPrefs.SetString("music", "relaxed");
        music.clip = relaxedMusic;
        music.Play();
    }

    public void muteMusic(){
        PlayerPrefs.SetString("music", "mute");
        music.Stop();
        music.clip = null;
    }
}
