using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundData", menuName = "ScriptableObjects/RoundData", order = 2)]
public class RoundData : ScriptableObject
{
    [SerializeField] protected AnimationCurve rotationCurve;
    public AnimationCurve RotationCurve
    { get { return rotationCurve; } }
    [SerializeField] protected float rotationSpeed = 120;
    public float RotationSpeed
    { get { return rotationSpeed; } }
    [SerializeField] protected bool invertRotationCurve;
    public bool InvertRotationCurve
    { get {return invertRotationCurve; } }
    [SerializeField] protected int knives;
    public int Knives
    { get { return knives; } }

    [SerializeField] protected GameObject blockPrefab;
    public GameObject BlockPrefab
    { get { return blockPrefab; } }

    public int difficulty = 0;

    [SerializeField] protected bool newLevel;
    public bool NewLevel
    { get { return newLevel; }}

    public virtual void init()
    {
        //do nothing in base - other like randomround will override
    }

    public virtual void cleanup()
    {
        //do nothing in base - other like randomround will override
    }

    public virtual void reset(bool hardReset = false)
    {
        difficulty = hardReset ? 0 : difficulty;
        rotationSpeed = 120;
        newLevel = true;
    }

}
