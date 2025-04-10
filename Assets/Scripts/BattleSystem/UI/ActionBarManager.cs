using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class ActionBarManager : MonoBehaviour {
    [Header("Action Slot References")]
    [SerializeField] private List<ActionSlot> actionSlots;

    private Button currentSlotSlected;
    private SlotAction currentSlotAction;

    public List<ActionSlot> ActionSLots { get => actionSlots; }
    public event Action<ActionSlot, SlotAction> OnSlotSelected;
    // helper function for handling Action Slot selection in ActionSlotSelection class

    public void HandleActionSlotSelection(SlotAction slotAction) {
        currentSlotAction = slotAction;
        StartCoroutine(DelayActionSlotSelection());
    }

    // this helps with not instantly selecting an action slot when a target is selected
    private IEnumerator DelayActionSlotSelection() {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        actionSlots[0].GetComponent<Button>().Select();
    }

    public void OnActionSlotSelected(ActionSlot slot) {
        switch (currentSlotAction) {
            case SlotAction.Add:
                OnSlotSelected?.Invoke(slot, SlotAction.Add);
                break;
            case SlotAction.Remove:
                OnSlotSelected?.Invoke(slot, SlotAction.Remove);
                break;
            case SlotAction.Swap:
                OnSlotSelected?.Invoke(slot, SlotAction.Swap);
                break;
        }

        // add
        // remove
        // init swap
        // complete swap
        // remove swap 
    }

    void OnEnable() {

    }
}
