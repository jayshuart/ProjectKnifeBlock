using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Knife : MonoBehaviour
{

    public GameObject target;
    private Collider2D targetCol;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    [SerializeField] private float throwSpeed;
    private bool moving;
    private bool thrown;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        thrown = false;

        if(target != null)
        {
            targetCol = target.GetComponent<Collider2D>();
        }

        rb = this.GetComponent<Rigidbody2D>();

        //get collider and make inactive at first - will bre reenabled when thrown
        col = this.GetComponent<BoxCollider2D>();
        col.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        //make sure its always pointing at center of screen (where the block is)
        //transform.up = target.transform.position - transform.position;

        move();
    }

    public void throwKnife()
    {
        moving = true;
        col.enabled = true;
    }

    private void move()
    {
        //check if moving
        if(!moving) { return; }

        //apply speed to pos
        float tY = this.transform.position.y + (throwSpeed * Time.deltaTime);
        this.transform.position = new Vector3(this.transform.position.x, tY, this.transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(thrown) { return; }


        //check we are colliding with correct type of obj
        if(col.collider != targetCol) 
        {
            Obj_Knife other = col.gameObject.GetComponent<Obj_Knife>();
            if(other != null && other.thrown)
            {
                rb.gravityScale = 2;
                moving = false;
            }
            
            return;
        }

        moving = false;
        thrown = true;

        //this.transform.parent = target.transform;
        this.transform.SetParent(target.transform, true);

        //lock this knife
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        //update block damage state
        target.GetComponent<Block_Break>().Hit();
    }

    void OnBecameInvisible() 
    {
        Destroy(this.gameObject);
    }
}
