using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedEdition : ItemManager
{
    Player player;

    protected override void Awake()
    {
        player = FindObjectOfType<Player>();
        slots = GetComponentsInChildren<Slots>();
    }

    public override bool Add(Item item)
    {
        switch (item)
        {
            case Item.head:
                break;
            case Item.body:
                break;
            case Item.airtank:
                break;
        }
        return true;
    }

    private void GetItem(Item item, int slotIndex)
    {
        if (!items.Contains(item))
        {
            items[slotIndex] = item;
            slots[slotIndex].itemIndex = slotIndex;
            slots[slotIndex].engaged = true;
        }
    }
}
