﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_KnifeToken : MonoBehaviour
{
    [SerializeField] private Color availableState;
    [SerializeField] private Color isUsedState;
    private Image img;
    private bool isUsed = true; //isUsed by default
    public bool IsUsed
    {
        get { return isUsed; }
        set {
            isUsed = value;
            setState(isUsed);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        img = this.GetComponent<Image>();
        setState(isUsed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setState(bool isUsed)
    {
        if(!isUsed)
        {
            img.color = availableState;
        }
        else
        {
            img.color = isUsedState;
        }
    }

    public void hide()
    {
        img.color = Color.clear;
    }
}
