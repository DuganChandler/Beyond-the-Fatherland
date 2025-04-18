using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour {
    [Header("UI References")]
    public GameObject buttonPrefab;   // Reference to your Button prefab.
    public Transform contentPanel;    // Reference to the ScrollRect's content panel.
    public TextMeshProUGUI itemDescription; 

    [SerializeField] ItemCategory itemCategory;

    private int lastButtonSelected = 0; 
 
    private List<(Button, ItemSlot)> buttonList = new();
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

    public void Update() {
        SelectItemDescription();
    }

    void SelectItemDescription() {
        if (!EventSystem.current.currentSelectedGameObject.TryGetComponent<Button>(out var currentSelectedButton)) {
            return;
        }
        
        foreach (var button in buttonList) {
            if (currentSelectedButton == button.Item1) {
                if (itemDescription != null) {
                    itemDescription.text = button.Item2.Item.ItemDescription;
                }
            }
        }
    }

    // Clears existing buttons and repopulates the inventory menu.
    public void PopulateInventory(Inventory playerInventory, ItemCategory category) {
        ClearButtons();

        var items = playerInventory.GetSlotsByCategory((int)category);
        if (items.Count <= 0) {
            return;
        }

        // Iterate over each item slot in the player's inventory.
        int i = 0;
        foreach (ItemSlot slot in items) {
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
                 
                buttonList.Add((currentButton, currentSlot));
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
            
            nav.selectOnUp = buttonList[upIndex].Item1;
            nav.selectOnDown = buttonList[downIndex].Item1;
            
            // Optionally, disable left/right navigation.
            nav.selectOnLeft = null;
            nav.selectOnRight = null;
            
            buttonList[i].Item1.navigation = nav;

            if (lastButtonSelected == i) {
                buttonList[i].Item1.Select();
            } else {
                buttonList[0].Item1.Select();
            }
        }
    } 

    // This method is called when an item button is clicked.
    void OnItemButtonClicked(ItemSlot slot, int buttonSelected) {
        MusicManager.Instance.PlaySound("MenuConfirm");
        lastButtonSelected = buttonSelected;
        OnItemSelected?.Invoke(slot);
        BattleEventManager.Instance.ItemSelected(slot);
    }
}
