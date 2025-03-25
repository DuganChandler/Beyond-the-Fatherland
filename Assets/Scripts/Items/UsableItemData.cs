using UnityEngine;

public enum UsableItemTargetVal { 
    Health, 
    Mana, 
    Both 
}

[CreateAssetMenu(fileName = "New Usable Item", menuName = "Items/UsableItem")]
public class UsableItem : ItemBase {
    [Header("Usable Item Values")]
    public UsableItemTargetVal targetVal;
    public int HP;               
    public int MP;              
}
