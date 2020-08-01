using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentRound;
    [SerializeField] private int currentLevel;

    [SerializeField] private LevelData[] levels;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;
        currentRound = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextLevel()
    {
        currentRound = (currentRound + 1);

        if(currentRound > levels[currentLevel].Rounds.Length)
        {
            currentRound = 0;
            currentLevel++;

            if(currentLevel > levels.Length)
            { currentLevel--; }
        }
    }

    public RoundData getRound(int pLevel = -1, int pRound = -1)
    {
        //check if we are just getting default level
        if(pLevel == -1 && pRound == -1)
        {
            return levels[currentLevel].getRound(currentRound);
        }
        else
        {
            return levels[pLevel].getRound(pRound);
        }
    }
}
