using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Button playBtn;
    [SerializeField] private Image playKnife;
    [SerializeField] private Sprite goldenKnife;
    [SerializeField] private Sprite normalKnife;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("goldKnife", 0);

        //handle if we are on an olde rversion and need to update some data
        if(!PlayerPrefs.HasKey("v1"))
        { Debug.Log("missing v1");
            PlayerPrefs.SetInt("v1", 1);
            clearHighscore();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clearHighscore()
    {
        PlayerPrefs.SetInt("best", 0);
    }

    public void holdToggleSecret()
    {
        int state = PlayerPrefs.GetInt("goldKnife", 0);
        state = (state + 1) % 2;

        playKnife.sprite = state == 1 ? goldenKnife : normalKnife;

        PlayerPrefs.SetInt("goldKnife", state);

    }

    public void onClickPlay()
    {
        playBtn.interactable = false;
        anim.Play("Selected");
        StartCoroutine(gotoGame());
    }

    IEnumerator gotoGame()
    {
        yield return new WaitForSeconds(.45f);
        SceneManager.LoadScene("MainGame");
    }
}
