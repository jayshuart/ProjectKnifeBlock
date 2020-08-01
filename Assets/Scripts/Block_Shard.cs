using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Shard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
