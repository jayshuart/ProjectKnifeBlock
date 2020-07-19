using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int knives;
    [SerializeField] private float score;
    [SerializeField] private Transform knifeSpawn;
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private GameObject targetBlock;
    private Obj_Knife currentKnife;

    // Start is called before the first frame update
    void Start()
    {
        //spawn inital knife
        spawnKnife();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void spawnKnife()
    {
        //check if we have knives to even throw, decriment or exit
        if(knives < 0) { return; }
        knives--;

        //instantiate new knife, set as current
        GameObject newKnife = Instantiate(knifePrefab, knifeSpawn);
        currentKnife = newKnife.GetComponent<Obj_Knife>();

        //set its target
        currentKnife.target = targetBlock;
    }

    public void throwKnife()
    {
        if(currentKnife == null) { return; }

        currentKnife.throwKnife();
        currentKnife = null; //clear it bc its been thrown

        //spawn new knife
        spawnKnife();
    }
}
