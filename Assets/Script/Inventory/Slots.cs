using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    public bool engaged = false;

    public Item item;
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
                        image.color = SetColor(); 
                        break;
                    case false:
                        image.color = Color.clear;
                        break;
                }
            }

        }
    }

    Image image;

    private Color SetColor()
    {
        Color color = Color.white;

        switch (item)
        {
            case Item.seaglider:
                color = Color.red; 
                break;
            case Item.airtank:
                color = Color.blue; 
                break;
            case Item.head:
                color = Color.green;
                break;
            case Item.water:
                color = Color.white; 
                break;
        }

        return color;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }
}
