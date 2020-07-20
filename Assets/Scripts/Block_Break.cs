using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Break : MonoBehaviour
{
    [SerializeField] private Sprite[] stateImgs;
    [SerializeField] private ParticleSystem woodParticles;

    [SerializeField] private float health = 100; //default 100, can chnage in editor
    private int stateIndex;
    private float damageStep;  //how much health is cut per hit
    private float breakStep;  //how much health is needed to change break state
    

    // Start is called before the first frame update
    void Start()
    {
        stateIndex = stateImgs.Length - 1;
        damageStep = health / GameObject.Find("GameManager").GetComponent<GameManager>().Knives;
        breakStep = health / stateImgs.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        //run particle effect
        woodParticles.Play();

        //inflict damage
        health -= damageStep;

        //check if enough damage ahs been taken to goto next state
        float dmgTaken = damageStep * stateIndex;
        dmgTaken = health - dmgTaken;

        int newStates = Mathf.FloorToInt(dmgTaken / damageStep);
        Debug.Log(newStates);
        ChangeState(stateIndex + newStates);
    }

    public void ChangeState(int newState)
    {
        //up state index
        if(newState > stateImgs.Length - 1 || newState == stateIndex) { return; } //leave if weve maxed out
        stateIndex = newState;

        //chnage sprite of block
        SpriteRenderer spriteRend = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRend.sprite = stateImgs[stateIndex]; 

    }
}
