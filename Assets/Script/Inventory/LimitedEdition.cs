using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedEdition : ItemManager
{
    UI playerUI;

    protected override void Awake()
    {
        playerUI = FindObjectOfType<UI>();
        slots = GetComponentsInChildren<Slots>();
        equip = true;
        items = new List<Item>
        {
            Item.None,
            Item.None,
            Item.None
        };
    }

    public override bool Add(Item item)
    {
        switch (item)
        {
            case Item.head:
                GetItem(item, 0);
                playerUI.safeBreathDepth = 200.0f;
                return true;
            case Item.body:
                playerUI.damageMultiple = 0.8f;
                GetItem(item, 1);
                return true;
            case Item.airtank:
                playerUI.maxBreathe = 120;
                GetItem(item, 2);
                return true;
            default:
                return false;
        }
    }

    public override void ChangeInven(int index)
    {
        if (another != null)
        {
            if (another.Add(items[index]))
            {
                items[index] = Item.None;
                slots[index].item = Item.None;
                foreach (Slots slot in slots) if (slot.item == Item.None)
                {
                    slot.itemIndex = -1;
                    slot.Engaged = false;
                }
                if (items[0] == Item.None) playerUI.safeBreathDepth = 100.0f;
                if (items[1] == Item.None) playerUI.damageMultiple = 1.0f;
                if (items[2] == Item.None) playerUI.maxBreathe = 60;
            }
        }
    }

    private void GetItem(Item item, int slotIndex)
    {
        if (!items.Contains(item))
        {
            items[slotIndex] = item;
            slots[slotIndex].item = item;
            slots[slotIndex].itemIndex = slotIndex;
            slots[slotIndex].Engaged = true;
        }
        else
        {
            Item temp = items[slotIndex];
            items[slotIndex] = item;
            slots[slotIndex].item = item;
            slots[slotIndex].itemIndex = slotIndex;
            slots[slotIndex].Engaged = true;
            another.Add(temp);
        }
    }
}
