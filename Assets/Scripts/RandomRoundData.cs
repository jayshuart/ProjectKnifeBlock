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

        //invert?
        invertRotationCurve = Random.Range(0, 2) > .5 && difficulty > 0;
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
                knifeChance.init(1f, new float[] {1f}, 100f, 0f, 100f);
                imbedChance.init(0f, new float[] {0f}, -1f, 0f, -1f);
                break;

            case 1:
                knifeChance.init(2f, new float[] {1f, 2f}, 100f, 0f, 100f);
                imbedChance.init(0f, new float[] {0f}, -1f, 0f, -1f);
                break;

            case 2:
                knifeChance.init(1f, new float[] {1, 2f}, 30f, 30f, 40f);
                imbedChance.init(1f, new float[] {1f}, 70f, 30f, 30f);
                break;

            case 3:
                knifeChance.init(2f, new float[] {1, 2f}, 30f, 30f, 40f);
                imbedChance.init(1f, new float[] {1f}, 70f, 35f, 45f);
                break;

            case 4:
                knifeChance.init(2f, new float[] {1f, 2f}, 30f, 30f, 40f);
                imbedChance.init(3f, new float[] {1f}, 50f, 45f, 50f);
                break;

            case 5:
                knifeChance.init(3f, new float[] {1f, 2f}, 70f, 30f, 80f);
                imbedChance.init(1f, new float[] {1f}, 50f, 45f, 50f);
                break;

            case 6:
                knifeChance.init(3f, new float[] {1f}, 50f, 40f, 40f);
                imbedChance.init(2f, new float[] {1f, 2f}, 60f, 40f, 60f);
                break;

            case 7:
                knifeChance.init(5f, new float[] {1f}, 80f, 20f, 80f);
                imbedChance.init(1f, new float[] {1f, 2f}, 30f, 30f, 30f);
                break;

            case 8:
                knifeChance.init(2f, new float[] {1f, 2f}, 100f, 10f, 100f);
                imbedChance.init(5f, new float[] {0f}, 0f, 0f, 0f);
                break;

            case 9:
                knifeChance.init(2f, new float[] {1f, 2f}, 100f, 10f, 100f);
                imbedChance.init(4f, new float[] {1f}, 50f, 0f, 50f);
                break;

            case 10:
                knifeChance.init(1f, new float[] {1f}, 100f, 10f, 100f);
                imbedChance.init(7f, new float[] {0f}, 0f, 0f, 0f);
                break;

            case 11:
                knifeChance.init(7f, new float[] {1f}, 100f, 10f, 100f);
                imbedChance.init(2f, new float[] {0f}, 0f, 0f, 0f);
                break;

            case 12:
                knifeChance.init(Random.Range(3, 6), new float[] {1f, 2f}, 50f, 30f, 50f);
                imbedChance.init(2f, new float[] {1f}, 30f, 30f, 30f);
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
        if(chance > 50 && difficulty > 4)
        { selectedCurve = Random.Range(1, curves.Length); }

        rotationCurve = curves[selectedCurve];
    }

    private void setSpeed()
    {
        if(!newLevel) { rotationSpeed += 10; return;}

        //new levels chose start point
        switch(difficulty)
        {
            case 0:
                rotationSpeed = 115;
                break;

            case 1:
                rotationSpeed = 120;
                break;

            case 2:
                rotationSpeed = 130;
                break;

            case 3:
                rotationSpeed = Random.Range(120, 140);
                break;

            case 4:
                rotationSpeed = Random.Range(125, 150);
                break;

            case 5:
                rotationSpeed = Random.Range(130, 160);
                break;
            
            case 6:
                rotationSpeed = Random.Range(140, 170);
                break;

            case 7:
                rotationSpeed = Random.Range(150, 180);
                break;

            case 8:
                rotationSpeed = Random.Range(160, 190);
                break;

            case 9:
                rotationSpeed = Random.Range(170, 200);
                break;

            case 10:
                rotationSpeed = Random.Range(180, 200);
                break;

            case 11:
                rotationSpeed = Random.Range(190, 220);
                break;

            case 12:
                rotationSpeed = Random.Range(190, 230);
                break;

            default:
                rotationSpeed = Random.Range(160, 250);
                break;
        }
    }
}
