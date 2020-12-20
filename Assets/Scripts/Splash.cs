using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Button playBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
