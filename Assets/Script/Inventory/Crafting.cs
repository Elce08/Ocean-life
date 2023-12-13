using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    private ItemManager inven;

    private Button[] craftButtons;

    private TextMeshProUGUI Name;
    private TextMeshProUGUI Cost;
    public Button lastCheck;

    private List<Item> removeList = new();
    private Item addItem = Item.None;

    private void Awake()
    {
        lastCheck = transform.GetChild(2).GetComponentInChildren<Button>();
        lastCheck.onClick.AddListener(LastCheck);
        lastCheck.gameObject.SetActive(false);
        inven = FindObjectOfType<Canvas>().transform.GetChild(2).GetChild(0).GetComponent<ItemManager>();
        craftButtons = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Button>();
        Name = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        Cost = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        SetButtons();
    }

    private void SetButtons()
    {
        craftButtons[0].onClick.AddListener(Plastic);
        craftButtons[1].onClick.AddListener(Glass);
        craftButtons[2].onClick.AddListener(CopperCable);
        craftButtons[3].onClick.AddListener(Water);
        craftButtons[4].onClick.AddListener(CookedFish1);
        craftButtons[5].onClick.AddListener(CookedFish2);
        craftButtons[6].onClick.AddListener(CookedFish3);
        craftButtons[7].onClick.AddListener(CookedFish4);
        craftButtons[8].onClick.AddListener(AirTank);
        craftButtons[9].onClick.AddListener(Mask);
        craftButtons[10].onClick.AddListener(Body);
        craftButtons[11].onClick.AddListener(Seaglider);
    }

    private int[] ItemCount = new int[11];

    public void InvenCheck()
    {
        ClearItemCount();
        foreach (Button button in craftButtons) button.interactable = false;
        foreach (Item item in inven.items)
        {
            if(item == Item.titanium) ItemCount[0]++;
            else if(item == Item.copper) ItemCount[1]++;
            else if(item == Item.coal) ItemCount[2]++;
            else if(item == Item.quartz) ItemCount[3]++;
            else if(item == Item.plastic) ItemCount[4]++;
            else if(item == Item.glass) ItemCount[5]++;
            else if(item == Item.coppercable) ItemCount[6]++;
            else if(item == Item.fish1) ItemCount[7]++;
            else if(item == Item.fish2) ItemCount[8]++;
            else if(item == Item.fish3) ItemCount[9]++;
            else if(item == Item.fish4) ItemCount[10]++;
        }
        if (ItemCount[2] > 1) craftButtons[0].interactable = true;
        if (ItemCount[3] > 1) craftButtons[1].interactable = true;
        if (ItemCount[1] > 1) craftButtons[2].interactable = true;
        if (ItemCount[7] > 0) craftButtons[3].interactable = true;
        if (ItemCount[7] > 0) craftButtons[4].interactable = true;
        if (ItemCount[8] > 0) craftButtons[5].interactable = true;
        if (ItemCount[9] > 0) craftButtons[6].interactable = true;
        if (ItemCount[10] > 0) craftButtons[7].interactable = true;
        if (ItemCount[0] > 3) craftButtons[8].interactable = true;
        if (ItemCount[0] > 0 && ItemCount[4] > 0 && ItemCount[5] > 0) craftButtons[9].interactable = true;
        if (ItemCount[4] > 2) craftButtons[10].interactable = true;
        if (ItemCount[0] > 2 && ItemCount[4] > 1) craftButtons[11].interactable = true;
    }

    private void ClearItemCount()
    {
        for(int i = 0; i<ItemCount.Length; i++)
        {
            ItemCount[i] = 0;
        }
    }

    private void LastCheck()
    {
        if(addItem != Item.None)
        {
            if (inven.Add(addItem))
            {
                Name.text = "";
                Cost.text = "";
                if (removeList != null) foreach (Item removeItem in removeList) inven.Remove(removeItem);
                lastCheck.gameObject.SetActive(false);
                InvenCheck();
                removeList.Clear();
                addItem = Item.None;
            }
            else
            {
                Name.text = "Inventory";
                Cost.text = "full";
                inven.Remove(addItem);
                if (removeList != null) foreach (Item removeItem in removeList) inven.Add(removeItem); 
                lastCheck.gameObject.SetActive(false);
                removeList.Clear() ;
                addItem = Item.None;
            }
        }
    }

    private void Plastic()
    {
        removeList.Clear();
        Name.text = "Coal";
        Cost.text = "X2";
        addItem = Item.plastic;
        removeList = new() { Item.coal, Item.coal };
        lastCheck.gameObject.SetActive(true);
    }

    private void Glass()
    {
        removeList.Clear();
        Name.text = "Quartz";
        Cost.text = "X2";
        addItem = Item.glass;
        removeList = new() { Item.quartz, Item.quartz };
        lastCheck.gameObject.SetActive(true);
    }

    private void CopperCable()
    {
        removeList.Clear();
        Name.text = "Copper";
        Cost.text = "X2";
        addItem = Item.coppercable;
        removeList = new() { Item.copper, Item.copper };
        lastCheck.gameObject.SetActive(true);
    }

    private void Water()
    {
        removeList.Clear();
        Name.text = "Fish1";
        Cost.text = "X1";
        addItem = Item.water;
        removeList = new() { Item.fish1 };
        lastCheck.gameObject.SetActive(true);
    }

    private void CookedFish1()
    {
        removeList.Clear();
        Name.text = "Fish1";
        Cost.text = "X1";
        addItem = Item.cookedFish1;
        removeList = new() { Item.fish1 };
        lastCheck.gameObject.SetActive(true);
    }

    private void CookedFish2()
    {
        removeList.Clear();
        Name.text = "Fish2";
        Cost.text = "X1";
        addItem = Item.cookedFish2;
        removeList = new() { Item.fish2 };
        lastCheck.gameObject.SetActive(true);
    }

    private void CookedFish3()
    {
        removeList.Clear();
        Name.text = "Fish3";
        Cost.text = "X1";
        addItem = Item.cookedFish3;
        removeList = new() { Item.fish3 };
        lastCheck.gameObject.SetActive(true);
    }

    private void CookedFish4()
    {
        removeList.Clear();
        Name.text = "Fish4";
        Cost.text = "X1";
        addItem = Item.cookedFish4;
        removeList = new() { Item.fish4 };
        lastCheck.gameObject.SetActive(true);
    }

    private void AirTank()
    {
        removeList.Clear();
        Name.text = "Titanium";
        Cost.text = "X4";
        addItem = Item.airtank;
        removeList = new() { Item. titanium, Item.titanium, Item.titanium, Item.titanium };
        lastCheck.gameObject.SetActive(true);
    }

    private void Mask()
    {
        removeList.Clear();
        Name.text = "Titanium\nPlastic\nGlass";
        Cost.text = "X1\nX1\nX1";
        addItem = Item.head;
        removeList = new() { Item.titanium, Item.plastic, Item.glass };
        lastCheck.gameObject.SetActive(true);
    }

    private void Body()
    {
        removeList.Clear();
        Name.text = "Plastic";
        Cost.text = "X3";
        addItem = Item.body;
        removeList = new() { Item.plastic, Item.plastic, Item.plastic };
        lastCheck.gameObject.SetActive(true);
    }

    private void Seaglider()
    {
        removeList.Clear();
        Name.text = "Titanium\nPlastic";
        Cost.text = "X3\nX2";
        addItem = Item.seaglider;
        removeList = new() { Item.titanium, Item.titanium, Item.titanium, Item.plastic, Item.plastic };
        lastCheck.gameObject.SetActive(true);
    }
}
