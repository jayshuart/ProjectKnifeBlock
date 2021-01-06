using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum GAME_STATE {
        UNSET,
        PLAY,
        WIN,
        FAIL
    }

    private GAME_STATE gameState = GAME_STATE.UNSET;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int knives;
    [SerializeField] private UI_Tokens knifeToken_parent;
    public int Knives { get{ return knives; } }
    [SerializeField] private int score;
    [SerializeField] private Transform knifeSpawn;
    [SerializeField] private GameObject knifePrefab;

    [SerializeField] private Transform blockSpawn;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject targetBlock;
    private Obj_Knife currentKnife = null;
    [SerializeField] private ParticleSystem sparkleParticles;
    [SerializeField] private ParticleSystem woodParticles;
    [SerializeField] private Animator winAnim;
    [SerializeField] private FailDisplay failScreen;
    [SerializeField] private WinDisplay winDisplay;

    private bool unlockDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        unlockDifficulty = false;
        knives = -1;
        prepRound();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void spawnKnife()
    {
        //check if we have knives to even throw, decriment or exit
        if(( currentKnife != null || knives <= 0 ) || gameState == GAME_STATE.FAIL) { return; }
        knives--;

        //update ui
        knifeToken_parent.useToken();

        //instantiate new knife, set as current
        GameObject newKnife = Instantiate(knifePrefab, knifeSpawn);
        currentKnife = newKnife.GetComponent<Obj_Knife>();

        //set its target and gm
        currentKnife.target = targetBlock;
        currentKnife.gm = this;
    }

    public void throwKnife()
    {
        if(currentKnife == null) { return; }

        currentKnife.throwKnife();
        currentKnife = null; //clear it bc its been thrown
    }

    public void Fail()
    {
        //update game state so this is only triggered once
       if(gameState != GAME_STATE.PLAY) { return; }
       gameState = GAME_STATE.FAIL;

        //todo - save to highscores

        //todo - do fail effect
        failScreen.setScore(score);
        failScreen.show();

        //todo - trigger restarting the scene as to restart the game fresh
        
    }

    public void Win()
    {
        if(gameState != GAME_STATE.PLAY) { return; }
        gameState = GAME_STATE.WIN;

        //do sfx
        SoundManager.Instance.playShatterSfx();

        //do win effects
        StartCoroutine(payoff());

        //update level data and scoring
        score += 10; //todo - 10 per round, 100 on level complete
        winDisplay.setScore(score);
        levelManager.nextLevel();

        //trigger next round
        StartCoroutine(delayedNextRound());
    }

    IEnumerator payoff()
    {
        //play fun sparkles and win banner
        sparkleParticles.Play();

        //wait a second
        yield return new WaitForSeconds(.15f);

        //show banner
        winAnim.gameObject.SetActive(true);
        winAnim.Play("winsBanner");
    }

    IEnumerator delayedNextRound()
    {
        //wait
        yield return new WaitForSeconds(1f);

        //clear win banner
        winAnim.Play("winsBanner_exit");
        yield return new WaitForSeconds(1f);
        winAnim.gameObject.SetActive(false);

        prepRound();
    }

    private void prepRound()
    {
        //delete old block
        if(targetBlock != null)
        { Destroy(targetBlock); }

        //get current round data from level manager
        RoundData round = levelManager.getRound();

        //special acse reset for our first random level
        if(levelManager.CurrentRound == 0)
        { 
            round.difficulty = Mathf.Clamp((round.difficulty + 1), 0, 10);
            round.reset(levelManager.CurrentLevel == 0); 
        }

        //initalize round os we can grab data
        int oldKnives = knives;
        do{ round.init(); }
        while(oldKnives == round.Knives);
        knives = round.Knives;

        //make new block
        GameObject blockStyle = round.BlockPrefab;
        if(blockStyle == null)
        {
            targetBlock = Instantiate(blockPrefab, blockSpawn.position, blockSpawn.rotation);
        }
        else
        {
            targetBlock = Instantiate(blockStyle, blockSpawn.position, blockSpawn.rotation);
        }

        targetBlock.GetComponent<Block_Break>().init(knives, woodParticles);
        targetBlock.GetComponent<Block_Rotator>().init(round.RotationCurve, round.RotationSpeed, round.InvertRotationCurve);

        //init tokens
        knifeToken_parent.initTokens(knives);

        //spawn inital knife
        spawnKnife();
       
       gameState = GAME_STATE.PLAY;
       round.cleanup();
    }
}
