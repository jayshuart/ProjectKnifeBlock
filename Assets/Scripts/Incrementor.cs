using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Incrementor", menuName = "ScriptableObjects/Incrementor", order = 3)]
public class Incrementor : ScriptableObject
{
    [SerializeField] private float chance = 0;
    public float Chance
    { 
        get { return chance; } 
        set { chance = value;}
    }

    [SerializeField] private float resetChance = 0;
    public float ResetChance
    { get { return resetChance; } }

    [SerializeField] private float chanceUp = 0;
    public float ChanceUp
    { get { return chanceUp; } }

    [SerializeField] private float value = 0;
    public float Value
    { 
        get { return value; } 
        set { this.value = value;}
    }
    [SerializeField] private float[] valueUp = new float[0];
    public float ValueUp
    { 
        get 
        { 
            int index = Random.Range(0, valueUp.Length);
            return valueUp[index];
        } 
    }


    public void init(float pVal, float[] pValUp, float pChance, float pChanceUp, float pResetChance)
    {
        value = pVal;
        valueUp = pValUp;

        chance = pChance;
        chanceUp = pChance;
        resetChance = pResetChance;
    }

    public bool checkChance()
    {
        int rand = Random.Range(0, 101);
        bool check = rand < chance;
        chance = check ? resetChance : increment();

        return check;
    }

    public float increment()
    { return chance += chanceUp; }

}
