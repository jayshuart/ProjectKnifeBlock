using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundData", menuName = "ScriptableObjects/RoundData", order = 2)]
public class RoundData : ScriptableObject
{
    [SerializeField] private AnimationCurve rotationCurve;
    public AnimationCurve RotationCurve
    { get { return rotationCurve; } }
    [SerializeField] private float rotationSpeed = 120;
    public float RotationSpeed
    { get { return rotationSpeed; } }
    [SerializeField] private bool invertRotationCurve;
    public bool InvertRotationCurve
    { get {return invertRotationCurve; } }
    [SerializeField] private int knives;
    public int Knives
    { get { return knives; } }

    [SerializeField] private GameObject blockPrefab;
    public GameObject BlockPrefab
    { get { return blockPrefab; } }
}
