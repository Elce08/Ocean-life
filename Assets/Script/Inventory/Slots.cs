using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    public bool engaged = false;
    public bool Engaged
    {
        get => engaged;
        set
        {
            if(value != engaged)
            {
                engaged = value;
                switch(value)
                {
                    case true:
                        image.color = Color.white; 
                        break;
                    case false:
                        image.color = Color.clear;
                        break;
                }
            }

        }
    }

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
}
