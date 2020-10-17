using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomRoundData", menuName = "ScriptableObjects/RandomRoundData", order = 3)]
public class RandomRoundData : RoundData
{
    private int imbededKnives;
    private List<float> distribution;
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
        //weighted random for number of knives
        return knives;
    }

    private GameObject prepBlock()
    {
        blockPrefab = cleanBlockPrefab;

        //decide if there are gonna be any imbeded knives
        int imbededChance = Random.Range(0, 100);

        if(imbededChance > 75) //no imbed
        {
            imbededKnives = 0;
        }
        else //imbeded, we need to define its traits
        {
            //how many, whats the spread like?
            defineImbdedKnives();
            imbedKnivesIntoBlock();
        }


        return blockPrefab;
    }

    private void defineImbdedKnives()
    {
        //most interesting play will prolly be in the 2-4 range, so weight towards that
        int chance = Random.Range(0, 100);
        if(chance > 90) // 10
        { imbededKnives = 1; }
        else if (chance > 70) //20
        { imbededKnives = 2; }
        else if (chance > 35) //35
        { imbededKnives = 3; }
        else if (chance > 15) //20
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
    { Debug.Log("<color=blue>" + distribution.Count + "</color>");
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
    }

    private void setSpeed()
    {
        //for now
        rotationSpeed = Random.Range(100, 160);

        //todo: spread speed into divisions, generally increasing as level increases
    }
}
