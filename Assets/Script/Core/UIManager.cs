using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ItemManager targetInven; // UI가 연결된 게임 오브젝트

    public void SetTargetSlot(ItemManager inven)
    {
        targetInven = inven;
    }
}
