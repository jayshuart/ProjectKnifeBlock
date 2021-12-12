using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Rotator : MonoBehaviour
{
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private float rotationSpeed;
    public float RotationSpeed 
    { 
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }
    [SerializeField] private bool invertRotation;

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

        dr *= rotationSpeed * Time.deltaTime;
        dr = invertRotation ? dr : dr * -1;

        this.transform.Rotate(0, 0, dr);

        //course correct for pos being changed by adding children
        // ------------------------------------------------------------- *** LOOK INTO THIS FOR A PROPER SOLUTION YOU DINGUS ***
       // this.transform.position = new Vector3(0, 3, -1);
    }

    public void init(AnimationCurve pRotationCurve, float pRotationSpeed, bool pInvertRotation)
    {
        //set passed values
        rotationCurve = pRotationCurve;
        rotationSpeed = pRotationSpeed;
        invertRotation = pInvertRotation;

        //set own values
        animTime = 0;

        //set scale
        float tScale = PlayerPrefs.GetFloat("world_scale", .85f);
        this.transform.localScale = new Vector3(tScale, tScale, tScale);

        if(!this.isActiveAndEnabled) { this.enabled = true; }
    }
}
