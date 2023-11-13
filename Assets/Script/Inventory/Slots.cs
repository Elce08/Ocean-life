using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slots : MonoBehaviour, IPointerClickHandler
{
    public Item item;

    public int itemIndex = -1;

    public bool engaged = false;

    ItemManager inven;

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
                        itemIndex = -1;
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
            case Item.body:
                color = Color.yellow;
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
        inven = GetComponentInParent<ItemManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("a");
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("b");
            if (itemIndex > -1)
            {
                Debug.Log("c");
                if (inven.another != null) inven.ChangeInven(itemIndex);
                else inven.DropItem(itemIndex);
            }
        }
    }
}
