using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentRound = 0;
    public int CurrentRound
    { get { return currentRound; } }

    [SerializeField] private int currentLevel = 0;
    public int CurrentLevel
    { get { return currentLevel; } }

    [SerializeField] private LevelData[] levels;

    [SerializeField] private GameObject roundNodeParent;
    [SerializeField] private UI_KnifeToken[] roundNodes;


    // Start is called before the first frame update
    void Start()
    {
        //currentLevel = 0;
        //currentRound = 0;
        getRound().reset();
        roundNodes = roundNodeParent.GetComponentsInChildren<UI_KnifeToken>();
        ResetRoundNodes();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextLevel()
    {
        //up round
        currentRound = (currentRound + 1);
        

        //check if we reacht he end of the level
        if(currentRound > levels[currentLevel].Rounds.Length - 1)
        {
            //reset round for new level, and up current level
            currentRound = 0;
            currentLevel++;

            //stay within bounds for levels
            if(currentLevel > levels.Length - 1)
            { currentLevel--; }

            //update ui
            StartCoroutine(delayedNodeUpdate(true));
        }
        else 
        { StartCoroutine(delayedNodeUpdate(false)); }


            
    }

    IEnumerator delayedNodeUpdate(bool pReset)
    {
        

        if(pReset)
        { 
            yield return new WaitForSeconds(.5f);
            roundNodes[3].IsUsed = true;

            yield return new WaitForSeconds(1f);
            ResetRoundNodes(); 
        }
        else
        { 
            yield return new WaitForSeconds(.6f);
            roundNodes[currentRound - 1].IsUsed = true; 
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

    public void ResetRoundNodes()
    {
        //round nodes use knife token ui script, as its p much just 2 colours and a boolean state system

        foreach(UI_KnifeToken node in roundNodes)
        {
            node.IsUsed = false;
        }

    //    / roundNodes[0].IsUsed = true;
    }
}
