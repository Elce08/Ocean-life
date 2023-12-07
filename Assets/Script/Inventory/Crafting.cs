using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    private ItemManager inven;

    private Button[] craftButtons;

    private TextMeshProUGUI Name;
    private TextMeshProUGUI Cost;
    private Button lastCheck;

    private void Awake()
    {
        lastCheck = transform.GetChild(2).GetComponentInChildren<Button>();
        lastCheck.onClick.AddListener(LastCheck);
        lastCheck.gameObject.SetActive(false);
        inven = FindObjectOfType<ItemManager>();
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

    private void LastCheck()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(false);
    }

    private void Plastic()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void Glass()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void CopperCable()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void Water()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void CookedFish1()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void CookedFish2()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void CookedFish3()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void CookedFish4()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void AirTank()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void Mask()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void Body()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }

    private void Seaglider()
    {
        Name.text = "";
        Cost.text = "";
        lastCheck.gameObject.SetActive(true);
    }
}
