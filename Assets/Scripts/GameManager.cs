using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int knives;
    [SerializeField] private UI_Tokens knifeToken_parent;
    public int Knives { get{ return knives; } }
    [SerializeField] private float score;
    [SerializeField] private Transform knifeSpawn;
    [SerializeField] private GameObject knifePrefab;

    [SerializeField] private Transform blockSpawn;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject targetBlock;
    private Obj_Knife currentKnife = null;
    [SerializeField] private ParticleSystem sparkleParticles;
    [SerializeField] private ParticleSystem woodParticles;

    // Start is called before the first frame update
    void Start()
    {
        prepRound();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void spawnKnife()
    {
        //check if we have knives to even throw, decriment or exit
        if(( currentKnife != null || knives < 0 )) { return; }
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

    public void Win()
    {
        //play fun sparkles
        sparkleParticles.Play();

        levelManager.nextLevel();

        StartCoroutine(delayedNextRound());
    }

    IEnumerator delayedNextRound()
    {
        yield return new WaitForSeconds(6f);
        prepRound();
    }

    private void prepRound()
    {
        //delete old block
        if(targetBlock != null)
        { Destroy(targetBlock); }

        //make new block at target coords
        targetBlock = Instantiate(blockPrefab, blockSpawn.position, blockSpawn.rotation);

        //parse round data into new block, and initalize
        parseRoundData();

        //initalize block break
        targetBlock.GetComponent<Block_Break>().init(knives, woodParticles);

        //spawn inital knife
        spawnKnife();

        //init tokens
        knifeToken_parent.initTokens(knives);
    }

    private void parseRoundData()
    {
        //get current round data from level manager
        RoundData round = levelManager.getRound();

        //set knives
        knives = round.Knives;

        //set rotator settings
        Block_Rotator rotator = blockPrefab.GetComponent<Block_Rotator>();
        rotator.init(round.RotationCurve, round.RotationSpeed, round.InvertRotationCurve);
    }
}
