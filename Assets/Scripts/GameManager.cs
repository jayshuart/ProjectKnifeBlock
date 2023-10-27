using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] private GameObject optionsScreen;

    private bool allowKnifeInput = false;
    private bool clickHeld = false;
    private bool clickUp = false;
    private Vector3 position;
    private float maxYMove;
    private float width;
    private float height;

    private bool unlockDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        unlockDifficulty = false;
        knives = -1;
        prepRound();

        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);

        float tScreenHeight = (float)Screen.height;
        maxYMove = Camera.main.ScreenToWorldPoint(new Vector2(0, tScreenHeight * .4f)).y;
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    void checkInput(){
        
        if(Input.GetMouseButtonDown(0)){
            allowKnifeInput = !EventSystem.current.IsPointerOverGameObject(0);
        }
        
        bool hasInput = Input.GetMouseButton(0);
        if (allowKnifeInput && hasInput){ //input on this frame
            Vector2 pos = Input.mousePosition;
            knifeToFinger(pos);
        }
        else if(allowKnifeInput && Input.GetMouseButtonUp(0)){ //no input this frame, btu last one did
            position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            throwKnife();
            allowKnifeInput = false;
        }
        else{
            knifeToHome();
            allowKnifeInput = false;
        }
    }

    void knifeToFinger(Vector2 pos){
        if(currentKnife == null || currentKnife.rb == null) { return; }

        currentKnife.canHover = false;

        float speed = 15f;

        position = Camera.main.ScreenToWorldPoint(pos);
        position.z = 0;
        position.x = 0;

        //cap y movement
        Vector3 posAdjust = new Vector3(0, currentKnife.transform.position.y + (speed * Time.deltaTime), 0);
        if(posAdjust.y > position.y) { posAdjust.y = position.y; }
        if(posAdjust.y > maxYMove) { posAdjust.y = maxYMove; }
        currentKnife.transform.position = posAdjust;
    }

    void knifeToHome(){
        if(currentKnife == null || currentKnife.canHover) { return; }

        float tMinPos = .3f;
        float tDirModifier = currentKnife.transform.localPosition.y < tMinPos ? 1 : -1;
        Vector3 tPosAdjust = new Vector3(0, currentKnife.transform.localPosition.y + ((20f * tDirModifier) * Time.deltaTime), 0);
        
        if((tDirModifier == -1 && tPosAdjust.y < tMinPos)
        || tDirModifier == 1 && tPosAdjust.y > tMinPos) { 
            tPosAdjust.y = tMinPos; 
            currentKnife.canHover = true;
        }

        currentKnife.transform.localPosition = tPosAdjust;
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

        //validate we have enough speed
        if(currentKnife.transform.position.y - position.y >= 0.0f) { return; }

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
        int attempts = 0; //will use to check if we need to cleanup and saftey catch for the while loop
        do{ 
            if(attempts > 0) { round.cleanup(); }
            round.init(); 
            attempts++;
        }
        while(attempts < 20 && (oldKnives == round.Knives || round.Knives <= 1));
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
