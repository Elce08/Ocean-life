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

    private UI playerUI;

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
        playerUI = FindObjectOfType <UI>();
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
                else if(player.invenState == Player.Inven.Inventory) inven.DropItem(itemIndex);
            }
        }
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (itemIndex > -1 && item != Item.None)
            {
                if(player.InvenState == Player.Inven.Inventory)
                {
                    if ((int)inven.items[itemIndex] > 9 && (int)inven.items[itemIndex] < 100) inven.ChangeInven(itemIndex);
                    else if ((int)inven.items[itemIndex] > 400) UseItem();
                }
            }
        }
    }

    private void UseItem()
    {
        switch (inven.items[itemIndex])
        {
            case Item.water:
                Eat(30, 0);
                break;
            case Item.fish1:
                Eat(10, 10);
                break;
            case Item.fish2:
                Eat(3, 12);
                break;
            case Item.fish3:
                Eat(4, 11);
                break;
            case Item.fish4:
                Eat(2, 15);
                break;
            case Item.cookedFish1:
                Eat(5, 20);
                break;
            case Item.cookedFish2:
                Eat(1, 24);
                break;
            case Item.cookedFish3:
                Eat(2, 22);
                break;
            case Item.cookedFish4:
                Eat(1, 30);
                break;
        }
        inven.Remove(itemIndex);
    }

    private void Eat(int Hydration, int Hunger)
    {
        playerUI.Hydration += Hydration;
        playerUI.Hunger += Hunger;
    }
}
