using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionSlotSelectionState : IBattleState {
    private readonly BattleSystem battleSystem;
    private const BattleState _state = BattleState.ActionSlotSelection;
    private SlotAction slotAction;

    public ActionSlotSelectionState(BattleSystem system, SlotAction action = SlotAction.Add) {
        battleSystem = system;
        slotAction = action;
    }

    public BattleState State {
        get {
            return _state;
        }
    }

    public void OnEnter() {
        Debug.Log("Now Entering: Action Slot Selection State");
        // this is done since we need to run a coroutine for delay
        battleSystem.ActionBarManager.HandleActionSlotSelection(slotAction);
        if (slotAction == SlotAction.Add) {
            battleSystem.InfoPanelManager.SetText("Select an action slot to add an action", true);
            return;
        }

        if (slotAction == SlotAction.Swap) {
            battleSystem.InfoPanelManager.SetText("Select any two actions slots to swap", true);
            return;
        }

        if (slotAction == SlotAction.Remove) {
            battleSystem.InfoPanelManager.SetText("Select a party action slot to remove", true);
            return;
        }

    }

    public void OnExit(){
        Debug.Log("Now Exiting: Action Slot Selection State");
        battleSystem.ClearTargetIndicator();
        EventSystem.current.SetSelectedGameObject(null);
        battleSystem.InfoPanelManager.SetText("", false);
    }

    public IBattleState OnBack() {
        switch (battleSystem.CurrentAction.Type) {
            case ActionType.Attack:
                return new TargetSelectionState(battleSystem);
            case ActionType.Ability:
                return new TargetSelectionState(battleSystem);
            case ActionType.Item:
                return new TargetSelectionState(battleSystem);
            case ActionType.Run:
                return new ActionSelectionState(battleSystem);
            case ActionType.None:
                return new SlotActionSelectionState(battleSystem);
            default:
                Debug.LogError($"Unable to go back from: Action Slot Selection State. Type of Current Action is {battleSystem.CurrentAction.Type}");
                return null;
        }
    }
}
