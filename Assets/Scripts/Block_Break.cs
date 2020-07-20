using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Break : MonoBehaviour
{
    [SerializeField] private Sprite[] stateImgs;

    private int stateIndex;

    // Start is called before the first frame update
    void Start()
    {
        stateIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextState()
    {
        //up state index
        stateIndex++;
        if(stateIndex > stateImgs.Length - 1) { return; } //leave if weve maxed out

        //chnage sprite of block
        SpriteRenderer spriteRend = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRend.sprite = stateImgs[stateIndex]; 

    }
}
