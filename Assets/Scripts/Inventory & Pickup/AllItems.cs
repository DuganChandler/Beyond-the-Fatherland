using UnityEngine;
[CreateAssetMenu(fileName = "New Defult Object", menuName = "Inventory System/Items/Defult")]
public class AllItems : Items
{
    public void Awake()
    {
        type = items.defult;
    }
}
