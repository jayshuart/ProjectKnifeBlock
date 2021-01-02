using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomRoundData", menuName = "ScriptableObjects/RandomRoundData", order = 3)]
public class RandomRoundData : RoundData
{
    private int imbededKnives;
    private List<float> distribution; //imbeded knife distribution
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private AnimationCurve[] curves;
    [SerializeField] private GameObject cleanBlockPrefab;
    private GameObject tempBlock;

    private Incrementor knifeChance;
    private Incrementor imbedChance;


    public override void init()
    {
        //init knives and imbed chance
        if(imbedChance == null)
        { imbedChance = new Incrementor(); }
        if(knifeChance == null) 
        { knifeChance = new Incrementor(); }

        newLevelInit();

        //set everything else
        setKnives(); //knives
        prepBlock(); //block w/ imbed
        
        selectCurve();
        setSpeed();

        //newLevel = false;
    }

    public override void cleanup()
    {
        newLevel = false;
        GameObject.Destroy(tempBlock);
    }

    private void newLevelInit()
    {
        if(!newLevel) { return; }

        switch(difficulty)
        {
            case 0:
                knifeChance.init(0f, new float[] {1f}, 100f, 0f, 100f);
                imbedChance.init(0f, new float[] {0f}, -1f, 0f, -1f);
                break;

            case 1:
                knifeChance.init(1f, new float[] {1f, 2f}, 100f, 0f, 100f);
                imbedChance.init(0f, new float[] {0f}, -1f, 0f, -1f);
                break;

            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                knifeChance.init(1f, new float[] {1, 2f}, 30f, 30f, 40f);
                imbedChance.init(1f, new float[] {1f}, 70f, 30f, 30f);
                break;

            default:
                knifeChance.init(1f, new float[] {1f}, 100f, 0f, 100f);
                imbedChance.init(0f, new float[] {0f}, -1f, 0f, -1f);
                break;
        }
    }

    private int setKnives()
    {
        int throwingKnives = (int) knifeChance.Value;

        if(knifeChance.checkChance())
        {
            throwingKnives += (int) knifeChance.ValueUp;

            //lower imbed chance if we add a knife
            imbedChance.Chance -= (imbedChance.ChanceUp / 2);
        }

        knives = throwingKnives;
        knifeChance.Value = knives;
        return knives;
    }

    private GameObject prepBlock()
    {
        blockPrefab = cleanBlockPrefab;

        //decide if there are gonna be any imbeded knives
        imbededKnives = (int) imbedChance.Value;

        if(imbedChance.checkChance()) //imbeded, we need to define its traits
        {
            //most interesting play will prolly be in the 2-4 range, so weight towards that
            imbededKnives = (int) (imbedChance.Value + imbedChance.ValueUp); //(Mathf.Clamp((imbedChance.Value + imbedChance.ValueUp), 0f, 7f));
            imbedChance.Value = imbededKnives;
        }

        //how many, whats the spread like?
        defineImbdedKnives();
        imbedKnivesIntoBlock();

        return blockPrefab;
    }

    private void defineImbdedKnives()
    {
        if(imbededKnives <= 0) {return; }

        //figure out distribution, different num of knives can be done differently
        distribution = new List<float>(); //holds where each nife is disributed
        float chance = Random.Range(0, 100);
        if(chance > 60)
        {
            distribution.Add(360.0f / imbededKnives);
        }
        else
        {
            float degreesLeft = 360f; //each knife takes some out of the full circle
            float buffer = 5f; //min space between knives
            float extraBuffer = 10f;
            
            //loop through each knife and spread it around
            for(int i = 0; i < imbededKnives - 1; i++)
            {
                float distrib = Random.Range(
                    360 - degreesLeft, //mind
                    degreesLeft - ((imbededKnives - i) * (buffer + extraBuffer))); //can be whatever is left
                        //extra buffer section to try to keep things more towards the min so we dont eatt he space for the later knives

                distribution.Add(distrib);
            }
        }
    }

    private void imbedKnivesIntoBlock()
    {
        if(imbededKnives <= 0) { return; }
        
        //get copy of block prefab
        tempBlock = blockPrefab;
        tempBlock = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity);

        //loop through knives and imbed them at the defined positions
        Vector3 spawn = new Vector3(0, -2.7f, 1);
        for(int i = 0; i < distribution.Count; i++)
        { 
            //create new knife
            GameObject knife = Instantiate(knifePrefab, spawn, Quaternion.identity, tempBlock.transform);

            //reset from old rotation and rotate block again
            tempBlock.transform.Rotate(new Vector3(0, 0, 1), distribution[i]);
        }

        //update prefab
        blockPrefab = tempBlock;
    }

    private void selectCurve()
    {
        //todo: first curve is basic straight curve, rest of choices have  alow chance of being chosen
        int chance = Random.Range(0, 100);
        int selectedCurve = 0;
        if(chance <= 50)
        { selectedCurve = 0; }
        else if(chance > 62)
        { selectedCurve = 1; }
        else if(chance > 74)
        { selectedCurve = 2; }
        else if(chance > 86)
        { selectedCurve = 3; }

        rotationCurve = curves[selectedCurve];
    }

    private void setSpeed()
    {
        //for now
        rotationSpeed = Random.Range(100, 120 + (difficulty * 11));
        rotationSpeed += (10 * difficulty);

        //invert?
        invertRotationCurve = Random.Range(0, 2) > .5 && difficulty > 0;
    }
}
