using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slots : MonoBehaviour, IPointerClickHandler
{
    private Player player;

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
                        Name.text = "None";
                        itemIndex = -1;
                        break;
                }
            }
        }
    }

    Image image;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        Name = GetComponentInChildren<TextMeshProUGUI>();
        Name.text = "None";
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
                if (player.InvenState == Player.Inven.Storage)
                {
                    inven.ChangeInven(itemIndex);
                }
                else inven.DropItem(itemIndex);
            }
        }
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (itemIndex > -1)
            {
                if(player.InvenState == Player.Inven.Inventory)
                {

                }
            }
        }
    }
}
