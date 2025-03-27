using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour {
    [Header("UI References")]
    public GameObject buttonPrefab;   // Reference to your Button prefab.
    public Transform contentPanel;    // Reference to the ScrollRect's content panel.

    [SerializeField] private BattleSystem battleSystem;

    private int lastButtonSelected = 0; 
 
    private List<Button> buttonList = new();
    public List<GameObject> buttonObjects = new();

    public event Action<ItemSlot> OnItemSelected;

    public void ClearButtons() {
        foreach (var button in buttonObjects) {
            if (button) {
                Destroy(button);
            }
        }
        buttonList.Clear();
        buttonObjects.Clear();
    }

    // Clears existing buttons and repopulates the inventory menu.
    public void PopulateInventory(Inventory playerInventory) {
        ClearButtons();

        var combatItems = playerInventory.GetSlotsByCategory((int)ItemCategory.Combat);
        if (combatItems.Count <= 0) {
            return;
        }

        // Iterate over each item slot in the player's inventory.
        int i = 0;
        foreach (ItemSlot slot in combatItems) {
            if (slot.Count > 0) {
                ++i;
                // Create a local copy to avoid lambda closure issues.
                ItemSlot currentSlot = slot;
                // Instantiate the button and set it as a child of the content panel.
                GameObject buttonObj = Instantiate(buttonPrefab, contentPanel);
                buttonObjects.Add(buttonObj);

                ItemButton itemButton = buttonObj.GetComponent<ItemButton>();

                itemButton.ItemName.text = currentSlot.Item.ItemName;
                itemButton.ItemCount.text = $"{currentSlot.Count}x";

                // Add a listener that calls OnItemButtonClicked with the current item slot.
                Button currentButton = buttonObj.GetComponent<Button>();
                currentButton.onClick.AddListener(() => OnItemButtonClicked(currentSlot, i));
                 
                buttonList.Add(currentButton);
            }
        }
        i = 0;
        SetupNavigation();
    }

    void SetupNavigation() {
        for (int i = 0; i < buttonList.Count; i++) {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;
            
            // Calculate indices for wrapping navigation.
            int upIndex = (i == 0) ? buttonList.Count - 1 : i - 1;
            int downIndex = (i == buttonList.Count - 1) ? 0 : i + 1;
            
            nav.selectOnUp = buttonList[upIndex];
            nav.selectOnDown = buttonList[downIndex];
            
            // Optionally, disable left/right navigation.
            nav.selectOnLeft = null;
            nav.selectOnRight = null;
            
            buttonList[i].navigation = nav;

            if (lastButtonSelected == i) {
                buttonList[i].Select();
            } else {
                buttonList[0].Select();
            }
        }
    } 

    // This method is called when an item button is clicked.
    void OnItemButtonClicked(ItemSlot slot, int buttonSelected) {
        lastButtonSelected = buttonSelected;
        OnItemSelected?.Invoke(slot);
    }
}
