using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tokens : MonoBehaviour
{
    private UI_KnifeToken[] tokens;
    private bool notOne_to_one; //bool for if knives tokens are 1-1 to knives, or a percent
    private float tokenWeight = 1; //how much a token is worth compared to knives, default is 1
    private float totalTokensWeighted;
    private float usedTokensWeighted = 0f;
    [SerializeField] private bool intialized = false;

    // Start is called before the first frame update
    void Start()
    {   

    }

    public void initTokens(int knives)
    {
        //fill tokens list and make them off by default
        tokens = this.gameObject.GetComponentsInChildren<UI_KnifeToken>();

        //calc token weight if needed
        if(knives > tokens.Length)
        {
            notOne_to_one = true;
            tokenWeight = knives / tokens.Length;
        }
        else
        { tokenWeight = 1; }

        totalTokensWeighted = knives;
        usedTokensWeighted = 0f;

        //show tokens based on number of knives, overflow should be hidden
        for(int t = 0; t < tokens.Length; t++)
        {
            if(t < knives)
            { 
                tokens[t].IsUsed = false;
            }
            else
            {
                tokens[t].hide();
            }
        }

        //set as init
        intialized = true;

    }

    //calcuates what tokens should be shown as used or not, acounting for relation betwen weight of a token and visible unit
    public void useToken()
    {
        if(!intialized) { return; }

        //calc shown tokens (floored to an int) 
        int shownTokens = Mathf.FloorToInt(usedTokensWeighted);

        //take off a new token and calc what would be shown floored to and int as well
        usedTokensWeighted += tokenWeight;
        int newShownTokens = Mathf.FloorToInt(usedTokensWeighted);

        //chnage the state of the affected tokens
        for(int i = 0; i < (newShownTokens - shownTokens); i++)
        {
            tokens[shownTokens + i].IsUsed = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
