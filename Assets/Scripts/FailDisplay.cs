using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FailDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestFlag;
    [SerializeField] private TextMeshProUGUI highscore_header;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private TextMeshProUGUI score_header;
    [SerializeField] private TextMeshProUGUI score;
    private bool newBest = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show()
    {
        this.gameObject.SetActive(true);
    }

    public void setScore(int pScore)
    {
        //todo get current highscore
        int best = 0;

        //update text
        score.text = pScore.ToString();
        highscore.text = best.ToString();

        //update for if we have a new best
        if(pScore > best)
        {
            newBest = true;
            
            //hide best, show new best flag
            highscore.gameObject.SetActive(false);
            highscore_header.gameObject.SetActive(false);
            bestFlag.gameObject.SetActive(true);

            //make text gold!
            score.color = Color.yellow;
        }
    }
}
