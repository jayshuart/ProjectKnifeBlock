using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int knives;
    [SerializeField] private UI_Tokens knifeToken_parent;
    public int Knives { get{ return knives; } }
    [SerializeField] private float score;
    [SerializeField] private Transform knifeSpawn;
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private GameObject targetBlock;
    private Obj_Knife currentKnife = null;
    [SerializeField] ParticleSystem sparkleParticles;

    // Start is called before the first frame update
    void Start()
    {
        //spawn inital knife
        spawnKnife();

        //init tokens
        knifeToken_parent.initTokens(knives - 1);
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
    }
}
