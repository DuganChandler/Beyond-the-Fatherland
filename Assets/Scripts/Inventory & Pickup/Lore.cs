using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Lore Object", menuName = "Inventory System/Items/Lore")]
public class Lore : Items
{
    public string lore;

    public void Awake()
    {
        type = items.healingPotion;


    }
}
