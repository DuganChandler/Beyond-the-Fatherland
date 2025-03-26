using System.Collections.Generic;
using UnityEngine;

public enum CombatItemTargetVal { 
    Health, 
    Mana, 
    Both 
}

[CreateAssetMenu(fileName = "New Combat Item", menuName = "Items/Combat Item")]
public class CombatItemData : ItemBase {
    public bool canTargetDead;
    public List<ItemEffectBase> effects;
}
