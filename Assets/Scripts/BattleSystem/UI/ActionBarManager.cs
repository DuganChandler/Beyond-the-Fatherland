using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionBarManager : MonoBehaviour {
    [Header("Action Slot References")]
    [SerializeField] private List<ActionSlot> actionSlots;

    private SlotAction currentSlotAction;

    private static ActionSlot slotToSwap;

    public List<ActionSlot> ActionSLots { get => actionSlots; }
    public Button previouslySelectedSlot { get; set; }

    public void HandleActionSlotSelection(SlotAction slotAction) {
        currentSlotAction = slotAction;
        StartCoroutine(DelayActionSlotSelection(slotAction));
    }

    private IEnumerator DelayActionSlotSelection(SlotAction slotAction) {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;

        if (slotAction == SlotAction.Add) {
            foreach (ActionSlot slot in actionSlots) {
                if (!slot.IsOccupied) {
                    slot.GetComponent<Button>().Select();
                    break;
                }
            } 
        } else if (slotAction == SlotAction.Remove || slotAction == SlotAction.Swap) {
            foreach (ActionSlot slot in actionSlots) {
                if (slot.IsOccupied && slot.BattleAction.User.Character.CharacterData.CharacerType == CharacerType.PartyMember) {
                    slot.GetComponent<Button>().Select();
                    break;
                }
            } 
        } else if (previouslySelectedSlot != null) {
            previouslySelectedSlot.Select();
        } else {
            actionSlots[0].GetComponent<Button>().Select();
        }
    }

    public void OnActionSlotSelected(ActionSlot slot) {
        previouslySelectedSlot = slot.GetComponent<Button>();
        switch (currentSlotAction) {
            case SlotAction.Add:
                BattleEventManager.Instance.SlotSelected(slot, SlotAction.Add);
                break;
            case SlotAction.Remove:
                BattleEventManager.Instance.SlotSelected(slot, SlotAction.Remove);
                break;
            case SlotAction.Swap:
                HandleSwap(slot);
                break;
        }
    }

    private void HandleSwap(ActionSlot slot) {
        if (slotToSwap == null) {
            slotToSwap = slot; 
            slotToSwap.IsSwapping = true;
            slotToSwap.GetComponent<Image>().color = Color.blue;
            BattleEventManager.Instance.SlotSelected(slot, SlotAction.Swap, false);
        } else if (slotToSwap == slot) {
            slot.IsSwapping = false;
            slotToSwap.GetComponent<Image>().color = Color.white;
            slotToSwap = null;
        } else {
            SwapActions(slotToSwap, slot);
            slotToSwap.IsSwapping = false;
            slotToSwap.GetComponent<Image>().color = Color.white;
            slotToSwap = null;

            BattleEventManager.Instance.SlotSelected(slot, SlotAction.Swap, true);
        }
    }

    private void SwapActions(ActionSlot slotA, ActionSlot slotB) {
        // Swap the BattleAction objects.
        BattleAction tempAction = slotA.BattleAction;
        slotA.BattleAction = slotB.BattleAction;
        slotB.BattleAction = tempAction;

        // Swap the occupancy flags.
        bool tempOccupied = slotA.IsOccupied;
        slotA.IsOccupied = slotB.IsOccupied;
        slotB.IsOccupied = tempOccupied;

        // Get the image components from the character portrait GameObjects.
        Image imageA = slotA.CharacterPortrait.GetComponent<Image>();
        Image imageB = slotB.CharacterPortrait.GetComponent<Image>();

        // Swap the portrait sprites.
        Sprite tempSprite = imageA.sprite;
        imageA.sprite = imageB.sprite;
        imageB.sprite = tempSprite;

        // Update the active state of the character portrait based on occupancy.
        slotA.CharacterPortrait.SetActive(slotA.IsOccupied);
        slotB.CharacterPortrait.SetActive(slotB.IsOccupied);
    }
}
