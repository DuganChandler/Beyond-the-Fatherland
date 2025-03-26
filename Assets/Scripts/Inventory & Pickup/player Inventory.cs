using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public List<InventorySlot> inventory = new List<InventorySlot>();  // Direct inventory list

    private item currentItem; // Stores the detected item

    private void OnTriggerEnter(Collider other)
    {
        var itemly = other.GetComponent<item>();
        if (itemly)
        {
            currentItem = itemly; // Store the item reference
            Debug.Log("Item in range: " + itemly.itemly.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var itemly = other.GetComponent<item>();
        if (itemly && itemly == currentItem)
        {
            currentItem = null; // Remove reference when out of range
            Debug.Log("Item out of range");
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && currentItem != null)  // Only if there is an item in range
        {
            PickUpItem(currentItem);
        }
    }

    private void PickUpItem(item itemToPick)
    {
        AddItem(itemToPick.itemly, 1);  // Use new AddItem logic
        Destroy(itemToPick.gameObject);
        Debug.Log("Item picked up: " + itemToPick.itemly.name);
        currentItem = null; // Clear reference
    }

    private void AddItem(Items _itemTing, int _amount)
    {
        bool hasItem = false;
        foreach (var slot in inventory)
        {
            if (slot.itemTing == _itemTing)
            {
                slot.AddAmount(_amount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            inventory.Add(new InventorySlot(_itemTing, _amount));
        }
    }

   
}

