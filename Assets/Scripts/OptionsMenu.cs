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
    [SerializeField] private Slider scaleSlider;


    // Start is called before the first frame update
    void Start()
    {
        music = SoundManager.Instance.gameObject.GetComponent<AudioSource>();
        if(SceneManager.GetActiveScene().name == "Splash")
        {
            StartCoroutine(autoSelectBtn_workaround());
            StartCoroutine(close_workaround());

            float tScale = PlayerPrefs.GetFloat("world_scale", .85f);
            setGameScale(tScale);
            scaleSlider.value = tScale;
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
        setButtonHighlight();
    }

    IEnumerator close_workaround()
    { 
        yield return null;
        this.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void openOptions()
    {
        SoundManager.Instance.playClickSfx();
        this.gameObject.SetActive(true);
    }

    public void closeOptions()
    {
        SoundManager.Instance.playClickSfx();
        this.gameObject.SetActive(false);
    }

    public void OnSliderValueChanged(float value){
        setGameScale(value);
    }

    public void setGameScale(float pScale){
        PlayerPrefs.SetFloat("world_scale", pScale);
    }

    public void playIntenseMusic(){
        if(music.clip == intenseMusic) { return; }

        SoundManager.Instance.playClickSfx();
        PlayerPrefs.SetString("music", "intense");
        music.clip = intenseMusic;
        music.Play();
        setButtonHighlight();
    }

    public void playRelaxedMusic(){
        if(music.clip == relaxedMusic) { return; }

        SoundManager.Instance.playClickSfx();
        PlayerPrefs.SetString("music", "relaxed");
        music.clip = relaxedMusic;
        music.Play();
        setButtonHighlight();
    }

    public void muteMusic(){
        if(music.clip == null) { return; }
        
        SoundManager.Instance.playClickSfx();
        PlayerPrefs.SetString("music", "mute");
        music.clip = null;
        setButtonHighlight();
    }

    public void setButtonHighlight()
    {
        intenseBtn.interactable = true;
        relaxedBtn.interactable = true;
        muteBtn.interactable = true;

        switch(PlayerPrefs.GetString("music", "relaxed"))
        {
            case "intense":
                intenseBtn.Select();
                playIntenseMusic();
                intenseBtn.interactable = false;
                break;

            case "relaxed":
                relaxedBtn.Select();
                playRelaxedMusic();
                relaxedBtn.interactable = false;
                break;

            case "mute":
                muteBtn.Select();
                muteMusic();
                muteBtn.interactable = false;
                break;
        }
    }

    public void onClickHome(){ Debug.Log("go home");
        SceneManager.LoadScene("Splash");
    }
}
