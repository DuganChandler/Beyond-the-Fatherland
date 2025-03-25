using UnityEngine;

public enum ItemCategory {
    Usable,
    Story,
}

public abstract class ItemBase : ScriptableObject {
    [Header("Item Details")]
    [SerializeField] private  string itemName;
    [SerializeField] private ItemCategory itemCategory;
    [SerializeField] private Sprite itemSprite;

    [TextArea]
    [SerializeField] private string itemDescription;


    public virtual ItemCategory ItemCategory { 
        get {
            return itemCategory;
        } 
    }

    public virtual string ItemDescription { 
        get {
            return itemDescription;
        }
    }

    public virtual string ItemName { 
        get {
            return itemName;
        }
    }

    public virtual Sprite ItemSprite { 
        get {
            return itemSprite;
        } 
    }
}
