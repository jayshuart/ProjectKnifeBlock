using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Shard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float tScale = PlayerPrefs.GetFloat("world_scale", .85f);
        this.transform.localScale = new Vector3(tScale, tScale, tScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     //clean up if offscrean
    void OnBecameInvisible() 
    {
        //delet this one bc its out of bounds
        Destroy(this.gameObject);
    }
}
