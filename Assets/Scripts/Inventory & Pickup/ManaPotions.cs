using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Mana Potion")]
public class ManaPotions : Items
{
    public int ManaRestore;

    public void Awake()
    {
        type = items.manaPotion;


    }
}
