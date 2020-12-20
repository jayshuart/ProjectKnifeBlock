using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FailDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestFlag;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI highscore_header;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private TextMeshProUGUI score_header;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private Animator anim;
    private bool newBest = false;
    private bool unlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        replay();
    }

    IEnumerator delayUnlock()
    {
        yield return new WaitForSeconds(.25f);
        unlocked = true;
    }

    public void unlock()
    { 
        unlocked = true;
    }

    public void replay()
    {
        if(Input.GetMouseButtonDown(0) //we can use this on pc and mobile, as we dont care about any info except for if we have input
        && score.gameObject.activeSelf && unlocked)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void show()
    {
        this.gameObject.SetActive(true);
        anim.Play("failScreen");
    }

    public void setScore(int pScore)
    {
        //todo get current highscore
        int best = PlayerPrefs.GetInt("best");

        //update text
        title.text = "Try Again";
        score.text = pScore.ToString();
        highscore.text = best.ToString();

        //update for if we have a new best
        if(pScore > best)
        {
            title.text = "New Best";
            PlayerPrefs.SetInt("best", pScore);
            newBest = true;

            //hide best, show new best flag
            highscore.gameObject.SetActive(false);
            highscore_header.gameObject.SetActive(false);
            //bestFlag.gameObject.SetActive(true);

            //make text gold!
            score.color = Color.yellow;
        }
    }
}
