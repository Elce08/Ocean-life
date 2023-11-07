using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Item
{
    None= 0,
    seaglider,
    airtank = 10,
    head = 20,
    body,
    titanium = 101,
    copper,
    coal,
    quartz,
    plastic = 201,
    glass,
    coppercable,
    water = 401,
    fish1,
    fish2,
    fish3,
    fish4,
    cookedFish1,
    cookedFish2,
    cookedFish3,
    cookedFish4,
}

public class ItemManager : MonoBehaviour
{
    GridLayoutGroup group;
    RectTransform rectTransform;

    public int width;
    public int height;

    public Sprite[] itemSprites;

    public Vector2Int ItemData(Item item)
    {
        Vector2Int result = new();
        switch (item)
        {
            case Item.seaglider:
                result.x = 3;
                result.y = 3;
                break;
            case Item.airtank:
                result.x = 2;
                result.y = 3;
                break;
            case Item.head:
            case Item.body:
                result.x = 2;
                result.y = 2;
                break;
            default:
                result.x = 1;
                result.y = 1;
                break;
        }
        return result;
    }

    private Slots[] slots;

    public List<(Item,List<Slots>)> itemsSlots = new();
    public List<Item> items = new();

    private void Awake()
    {
        group = GetComponent<GridLayoutGroup>();
        slots = GetComponentsInChildren<Slots>();
        rectTransform = GetComponent<RectTransform>();
        width = (int)(rectTransform.sizeDelta.x / group.cellSize.x);
        height = (int)(rectTransform.sizeDelta.y / group.cellSize.y);
    }

    static Item lastItem;

    public void Add(Item item)
    {
        items.Add(item);
        lastItem = item;
        RefreshSlots();
    }

    public void Remove(int index)
    {
        items.Remove(items[index]);
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        items.Sort();
        foreach(Slots slot in slots)
        {
            slot.Engaged = false;
        }
        int index = 0;
        foreach(Item item in items)
        {
            int x = ItemData(item).x;
            int y = ItemData(item).y;
            int start = 0;
            List<Slots> Slots = new();
            foreach(Slots startSlot in slots)
            {
                if (start % width + x > width) start++;
                else if (slots[start+x-1].Engaged) start++;
                else 
                { 
                    if (!startSlot.Engaged)
                    {
                        break;
                    }
                    else start++;
                }
            }
            if(start >= (width * height))
            {
                items.Remove(lastItem);
                Debug.Log("Fail");
                break;
            }
            if((start + (x-1) + (y-1) * width) > (width * height))
            {
                items.Remove(lastItem);
                Debug.Log("Fail");
                RefreshSlots();
                break;
            }

            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    Slots.Add(slots[(start + i + (j * width))]);
                    slots[(start + i + (j * width))].item = item;
                    slots[(start + i + (j * width))].itemIndex = index;
                    slots[(start + i + (j * width))].Engaged = true;
                }
            }
            itemsSlots.Add(new(item, Slots));
            index++;
        }
    }
}
