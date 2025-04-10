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

    public void HandleActionSlotSelection(SlotAction slotAction) {
        currentSlotAction = slotAction;
        StartCoroutine(DelayActionSlotSelection());
    }

    private IEnumerator DelayActionSlotSelection() {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        actionSlots[0].GetComponent<Button>().Select();
    }

    public void OnActionSlotSelected(ActionSlot slot) {
        switch (currentSlotAction) {
            case SlotAction.Add:
                BattleEventManager.Instance.SlotSelected(slot, SlotAction.Add);
                break;
            case SlotAction.Remove:
                BattleEventManager.Instance.SlotSelected(slot, SlotAction.Remove);
                break;
            case SlotAction.Swap:
                BattleEventManager.Instance.SlotSelected(slot, SlotAction.Swap);
                break;
        }

        // init swap
        // complete swap
        // remove swap 
    }
}
