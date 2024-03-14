using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TestInventory : TestBase
{
    public ItemManager inven;
    public ObjectManager objManager;
    public Item testItem = Item.None;
    Crafting craft;
    Player player;
    Handling handle;

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
        handle = FindObjectOfType<Handling>();
        craft = FindObjectOfType<Crafting>();
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        inven.Add(Item.titanium);
        inven.Add(Item.copper);
        inven.Add(Item.coal);
        inven.Add(Item.quartz);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        //craft.gameObject.SetActive(true);
        //craft.InvenCheck();
        //Cursor.lockState = CursorLockMode.None;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        inven.Add(testItem);
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        objManager.able = false;
        player.setWork = true;
        player.setStorage = false;
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        objManager.able = false;
        player.setWork = false;
        player.setStorage = true;
    } 

    protected override void TestRClick(InputAction.CallbackContext _)
    {
        player.setWork = false;
        player.setStorage = false;
        objManager.ShowIndicator();
    }

}
