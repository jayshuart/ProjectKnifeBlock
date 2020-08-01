using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField] private RoundData[] rounds;
    public RoundData[] Rounds{
        get {return rounds;}
    }
    public RoundData getRound(int pRound)
    {
        return rounds[pRound];
    }
    [SerializeField] private bool chooseRandomly;
    public bool ChooseRandomly
    { 
        get { return chooseRandomly; } 
    }
}
