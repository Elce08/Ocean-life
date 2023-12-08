using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TestInventory : TestBase
{
    public ItemManager inven;
    public ObjectManager objManager;
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
        craft.gameObject.SetActive(true);
        craft.InvenCheck();
        Cursor.lockState = CursorLockMode.None;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        player.Die();
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        // inven.Add(Item.water);
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        objManager.able = false;
        player.setStorage = true;
    } 

    protected override void TestRClick(InputAction.CallbackContext _)
    {
    }

}
