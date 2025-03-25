using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemSlot {
    public ItemBase Item;
    public int Count;
} 

public class Inventory : MonoBehaviour {
    [SerializeField] List<ItemSlot> usableItems;
    [SerializeField] List<ItemSlot> storyItems;

    List<List<ItemSlot>> allItems;

    void Awake() {
        allItems = new() { usableItems, storyItems };
    }

    public List<ItemSlot> GetSlotsByCategory(int slotIndex) {
        return allItems[slotIndex];
    }

    public void AddItem(ItemBase item, int count = 1) {
        int itemCategory = (int)item.ItemCategory;
        List<ItemSlot> currentSlotCategory = GetSlotsByCategory(itemCategory);

        if (currentSlotCategory.Count == 0) {
            currentSlotCategory.Add(new ItemSlot {
                Item = item,
                Count = count
            });
            return;
        }

        for (int i = 0; i < currentSlotCategory.Count; ++i) {
            ItemSlot currentSlot = currentSlotCategory[i]; 
            if (currentSlot.Item.ItemName == item.ItemName) {
                currentSlot.Count += count;
                currentSlotCategory[i] = currentSlot;
            } else {
                Debug.Log("Added");
                currentSlotCategory.Add(new ItemSlot {
                    Item = item,
                    Count = count
                });
            }
        }
    }

    public void RemoveItem(ItemBase item, int count = 1) {
        int itemCategory = (int)item.ItemCategory;
        List<ItemSlot> currentSlotCategory = GetSlotsByCategory(itemCategory);

        for (int i = 0; i < currentSlotCategory.Count; ++i) {
            ItemSlot currentSlot = currentSlotCategory[i]; 
            if (currentSlot.Item.ItemName == item.ItemName) {
                currentSlot.Count -= count;
                if (currentSlot.Count < 1) {
                    currentSlotCategory.RemoveAt(i);
                    continue;
                }
                currentSlotCategory[i] = currentSlot;
            }
        }
    }
}
