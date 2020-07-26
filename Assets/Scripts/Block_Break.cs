using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Break : MonoBehaviour
{
    [SerializeField] private Sprite[] stateImgs;
    [SerializeField] private GameObject[] shardPrefabs;
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
        ChangeState(stateIndex + newStates);

        StartCoroutine(squish());

        if(health <= 0)
        {
            Shatter();
        }
    }

    IEnumerator squish()
    {
        Vector3 originalScale = this.transform.localScale;

        //squish over time
        float scaleFactor = .9f;
        Vector3 squished = new Vector3(this.transform.localScale.x * scaleFactor, this.transform.localScale.y * scaleFactor, this.transform.localScale.z * scaleFactor);
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, squished, Time.deltaTime * 10);

        yield return new WaitForSeconds(.15f);

       this.transform.localScale = originalScale; //Vector3.Lerp(this.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 10);
    }

    public void ChangeState(int newState)
    {
        //up state index
        if(newState < 0 || newState == stateIndex) { return; } //leave if weve maxed out
        stateIndex = newState;

        //chnage sprite of block
        SpriteRenderer spriteRend = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRend.sprite = stateImgs[stateIndex]; 

    }

    private void Shatter()
    {
        //stop rotating
        this.gameObject.GetComponent<Block_Rotator>().RotationSpeed = 0;

        //hide block asset
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;

        //create all the shards at the block pos, with current rotation
        foreach(GameObject shard in shardPrefabs)
        {
            //create shard
            GameObject newShard = Instantiate(shard, this.gameObject.transform.position, this.gameObject.transform.rotation);

            //apply a force on it to make the break mroe exciting
            Vector2 bumpForce = new Vector2(Random.Range(-200, 200), Random.Range(-200, 200));
            newShard.GetComponent<Rigidbody2D>().AddForce(bumpForce);
        }

        //unlock all the knives imbded in block so they fall
        Obj_Knife[] knives = this.gameObject.GetComponentsInChildren<Obj_Knife>();
        foreach(Obj_Knife knife in knives)
        {
            knife.Release();
        }

        //find gm and run the win state
        GameObject.Find("GameManager").GetComponent<GameManager>().Win();
    }
}
