using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioClip intenseMusic;
    [SerializeField] private AudioClip relaxedMusic;
    [SerializeField] private Button intenseBtn;
    [SerializeField] private Button relaxedBtn;
    [SerializeField] private Button muteBtn;
    // Start is called before the first frame update
    void Start()
    {
        music = SoundManager.Instance.gameObject.GetComponent<AudioSource>();
        if(SceneManager.GetActiveScene().name == "Splash")
        {
            StartCoroutine(autoSelectBtn_workaround());
            StartCoroutine(close_workaround());
        }
        
    }

    void OnEnable()
    {
        StartCoroutine(autoSelectBtn_workaround());
    }

    IEnumerator autoSelectBtn_workaround()
    {
        yield return null; //yiled until end of the frame
        EventSystem.current.SetSelectedGameObject(null);
        switch(PlayerPrefs.GetString("music", "relaxed"))
        {
            case "intense":
                intenseBtn.Select();
                playIntenseMusic();
                break;

            case "relaxed":
                relaxedBtn.Select();
                playRelaxedMusic();
                break;

            case "mute":
                muteBtn.Select();
                muteMusic();
                break;
        }
    }

    IEnumerator close_workaround()
    { 
        yield return null;
        this.closeOptions();
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
        if(music.clip == intenseMusic) { return; }

        PlayerPrefs.SetString("music", "intense");
        music.clip = intenseMusic;
        music.Play();
    }

    public void playRelaxedMusic(){
        if(music.clip == relaxedMusic) { return; }

        PlayerPrefs.SetString("music", "relaxed");
        music.clip = relaxedMusic;
        music.Play();
    }

    public void muteMusic(){
        if(music.clip == null) { return; }

        PlayerPrefs.SetString("music", "mute");
        music.Stop();
        music.clip = null;
    }
}
