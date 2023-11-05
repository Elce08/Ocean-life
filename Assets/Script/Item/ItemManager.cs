using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public enum Item
{
    None= 0,
    seaglider,
    head,
    body,
    airtank,
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
    public static int width = 10;
    public static int height = 12;

    public Sprite[] itemSprites;

    public static Vector2Int ItemData(Item item)
    {
        Vector2Int result = new();
        switch ((int)item)
        {
            case 3:
            case 2:
                result.x = 2;
                result.y = 2;
                break;
            case 4:
                result.x = 2;
                result.y = 3;
                break;
            case 1:
                result.x = 3;
                result.y = 3;
                break;
            default:
                result.x = 1;
                result.y = 1;
                break;
        }
        Debug.Log($"{result.x},{result.y}");
        return result;
    }

    static Slots[] slots;

    public static List<(Item,List<Slots>)> itemsSlots = new();
    public static List<Item> items = new();

    private void Awake()
    {
        slots = GetComponentsInChildren<Slots>();
    }

    public static void Add(Item item)
    {
        items.Add(item);
        RefreshSlots();
    }

    public static void Remove(Item item)
    {
        items.Remove(item);
        RefreshSlots();
    }

    public static void RefreshSlots()
    {
        // 칸 넘치면 아랫줄로 넘어가게 수정하기
        items.Sort();
        foreach(Slots slot in slots)
        {
            slot.Engaged = false;
        }
        foreach(Item item in items)
        {
            int x = ItemData(item).x;
            int y = ItemData(item).y;
            int start = 0;
            List<Slots> Slots = new();
            foreach(Slots startSlot in slots)
            {
                if (start % width + x > width) start++;
                else 
                { 
                    if (!startSlot.Engaged)
                    {
                        break;
                    }
                    else start++;
                }
            }
            if((start + x + y * width) > ((width+1) * (height+1)))
            {
                Debug.Log("Fail to add");
                items.Remove(item);
                break;
            }

            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    Slots.Add(slots[(start + i + (j * width))]);
                    slots[(start + i + (j * width))].item = item;
                    slots[(start + i + (j * width))].Engaged = true;
                }
            }
            itemsSlots.Add(new(item, Slots));
        }
    }
}
