using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slots : MonoBehaviour, IPointerClickHandler
{
    public Item item;

    private TextMeshProUGUI Name;

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
                        image.color = Color.white;
                        Name.text = $"{item}";
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

    private void Awake()
    {
        Name = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
        inven = GetComponentInParent<ItemManager>();
        itemIndex = -1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemIndex > -1)
            {
                if (inven.another != null)
                {
                    inven.ChangeInven(itemIndex);
                }
                else inven.DropItem(itemIndex);
            }
        }
    }
}
