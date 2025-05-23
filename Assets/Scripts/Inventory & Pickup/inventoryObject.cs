using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Items itemTing;
    public int amount;

    public InventorySlot(Items _itemTing, int _amount)
    {
        itemTing = _itemTing;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}