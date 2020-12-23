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

    public override void init()
    {
        prepBlock();
        setKnives();
        selectCurve();
        setSpeed();
    }

    public override void cleanup()
    {
        GameObject.Destroy(tempBlock);
    }

    private int setKnives()
    {
        int throwingKnives = 0;

        //weighted random for number of knives
        int chance = Random.Range(0, 100);

        //calc adjustments (like level or imbeded knives)
        if(difficulty == 0)
        { throwingKnives = Random.Range(2, 5); }
        else if(difficulty == 1)
        { throwingKnives = Random.Range(2, 7); }
        else if(difficulty == 2)
        { throwingKnives = Random.Range(3, 8); }
        else if(difficulty == 3)
        { throwingKnives = Random.Range(3, 12); }
        else if(difficulty == 4)
        { throwingKnives = Random.Range(4, 15); }
        else if(difficulty == 5)
        { throwingKnives = Random.Range(5, 20); }

        //special handling for high imbed cases
        if(imbededKnives >= 19)
        {
            throwingKnives = 1;
        }
        else if(imbededKnives >= 18)
        {
            throwingKnives = 2;
        }
        else if(imbededKnives >= 11)
        {
            if(throwingKnives > 10)
            { throwingKnives = Random.Range(difficulty, 10); }
        }

        knives = throwingKnives;
        return knives;
    }

    private GameObject prepBlock()
    {
        blockPrefab = cleanBlockPrefab;

        //decide if there are gonna be any imbeded knives
        int imbededChance = Random.Range(0 + (difficulty * 3), 100);

        if(imbededChance > 40 && difficulty >= 1) //imbeded, we need to define its traits
        {
            //how many, whats the spread like?
            defineImbdedKnives();
            imbedKnivesIntoBlock();
        }
        else//no imbed
        {
            imbededKnives = 0;
        }


        return blockPrefab;
    }

    private void defineImbdedKnives()
    {
        //most interesting play will prolly be in the 2-4 range, so weight towards that
        int min = difficulty <= 2 ? 73 : 0;
        min = difficulty <= 3 ? 38 : 0;

        int chance = Random.Range(min, 100);
        if(chance > 93) // 10
        { imbededKnives = 1; }
        else if (chance > 72) //20
        { imbededKnives = 2; }
        else if (chance > 37) //35
        { imbededKnives = 3; }
        else if (chance > 18) //20
        { imbededKnives = 4; }
        else if (chance > 5) //10
        { imbededKnives = 5; }
        else // 5
        { imbededKnives = 6; }

        //figure out distribution, different num of knives can be done differently
        distribution = new List<float>(); //holds where each nife is disributed
        chance = Random.Range(0, 100);
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
        invertRotationCurve = Random.Range(0, 1) > .5 && difficulty > 0;
    }
}
