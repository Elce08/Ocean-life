using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public Item sort;
    public ItemManager inven;

    public void Interaction()
    {
        inven.Add(sort);
        Destroy(this.gameObject);
    }
}
