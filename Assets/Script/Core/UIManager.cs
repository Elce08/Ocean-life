using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ItemManager targetInven; // UI�� ����� ���� ������Ʈ

    public void SetTargetSlot(ItemManager inven)
    {
        targetInven = inven;
    }
}
