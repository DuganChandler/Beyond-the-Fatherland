using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum items
{
    healingPotion,
    manaPotion,
    lore,
    defult
}
public class Items : ScriptableObject
{
    
    public items type;
    [ TextArea(15,20)]
    public string descirption;

    
}
