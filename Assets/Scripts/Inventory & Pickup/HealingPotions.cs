using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Healing Potion")]

public class HealingPotions : Items
{
    public int healingAmount;
    
    public void Awake()
    {
        type = items.healingPotion;
        

    }
}
