using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setScore(int pScore)
    {
        score.text = pScore.ToString();

        if(pScore > PlayerPrefs.GetInt("best"))
        {
            score.color = Color.yellow;
        }
    }
}
