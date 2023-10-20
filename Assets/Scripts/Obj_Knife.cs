using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Knife : MonoBehaviour
{

    public GameObject target;
    private Collider2D targetCol;
    public Rigidbody2D rb;
    private BoxCollider2D col;
    [SerializeField] private float throwSpeed;
    private bool moving;
    private bool thrown;
    public bool canHover = true;
    [SerializeField] bool startImbeded = false; //not a thrown knife, starts as a part of the block

    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite normalSprite;

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        if(startImbeded) { initImbeded(); }
        else { init(); }
        
        float tScale = PlayerPrefs.GetFloat("world_scale", .85f);
        this.transform.localScale = new Vector3(tScale, tScale, tScale);
    }

    private void initImbeded()
    {
        moving = false;
        thrown = true;

        if(target != null)
        {
            targetCol = target.GetComponent<Collider2D>();
        }

        //lock knife
        rb = this.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        //get collider and make inactive at first
        col = this.GetComponent<BoxCollider2D>();
    }

    private void init()
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

        //spawn in
        StartCoroutine(spawnEffect());

        //float while we wait to be thrown
        StartCoroutine(hover());

        //swap texture
        setSprite();

    }

    public void setSprite()
    {
        int state = PlayerPrefs.GetInt("goldKnife", 0);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = state == 1 ? goldSprite : normalSprite;
    }

    IEnumerator spawnEffect()
    {
        //get spriet rednerers colour attrbiute
        SpriteRenderer spr = this.gameObject.GetComponent<SpriteRenderer>();
        Color whiteClear = new Color(1, 1, 1, 0);
        spr.color = whiteClear;
        float duration = 0f;
        while(spr.color.a < 100)
        {
            duration += Time.deltaTime * 3;
            float progress = duration / .65f;
            spr.color = Color.Lerp(whiteClear, Color.white, progress);
            yield return null;
        }
        yield return null;
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
        rb.velocity = Vector2.zero;
        rb.drag = 0;
        
        moving = true;
        col.enabled = true;
        SoundManager.Instance.playThrowSfx();
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
                rb.AddTorque(350);
                rb.gravityScale = 2;
                moving = false;
                thrown = true;

                //trigger fail state
                gm.Fail();

                SoundManager.Instance.playMetalHitSfx();
            }
            
            return;
        }

        moving = false;
        thrown = true;

        //tell game we need a new knife, and parent this one to the block
        gm.spawnKnife();
        this.transform.SetParent(target.transform, true);

        //lock this knife
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        //update block damage state
        target.GetComponent<Block_Break>().Hit();
        SoundManager.Instance.playHitSfx();
    }

    public void Release()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale = Random.Range(2, 4);
    }

    IEnumerator hover()
    {   
        float hoverDuration = 0.0f;
        int hoverDir = 1;
        float moveDist = .5f;

        while(!moving && !thrown)
        {

            if(canHover){
                hoverDuration -= Time.deltaTime;
                float currentY = this.transform.localPosition.y;

                if(hoverDuration > 0)
                {
                    currentY += ((moveDist * Time.deltaTime) * hoverDir);
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x, currentY, this.transform.localPosition.z);
                }
                else
                {
                    hoverDuration = .5f;
                    hoverDir *= -1;
                    //yield return new WaitForSeconds(3f);
                }
            }

            yield return null;
        }
    }
    
    
    //clean up if offscrean
    void OnBecameInvisible() 
    {
        if(!startImbeded && rb.drag == 0) //not imbeded, and using drag as a flag for if its been properly thrown
        {
            //delet this one bc its out of bounds
            Destroy(this.gameObject);
            gm.Fail();
        }
    }
}
