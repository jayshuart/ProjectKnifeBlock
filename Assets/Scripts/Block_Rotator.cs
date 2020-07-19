using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Rotator : MonoBehaviour
{
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool invertRoationCurve;

    private float animTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get rot change from curve
        animTime += Time.deltaTime;
        float dr = rotationCurve.Evaluate(animTime); //chnage in rotation (delta rotation)

        dr *= rotationSpeed;
        dr = invertRoationCurve ? dr : dr * -1;

        this.transform.Rotate(0, 0, dr);

        //course correct for pos being changed by adding children
        // ------------------------------------------------------------- *** LOOK INTO THIS FOR A PROPER SOLUTION YOU DINGUS ***
       // this.transform.position = new Vector3(0, 3, -1);
    }
}
