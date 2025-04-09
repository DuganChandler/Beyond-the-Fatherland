using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionSlotSelectionState : IBattleState {
    private readonly BattleSystem battleSystem;
    private const BattleState _state = BattleState.ActionSlotSelection;

    public ActionSlotSelectionState(BattleSystem system) {
        battleSystem = system;
    }
    public BattleState State {
        get {
            return _state;
        }
    }

    public void OnEnter() {
        Debug.Log("Now Entering: Action Slot Selection State");
        // this is done since we need to run a coroutine for delay
        battleSystem.HandleActionSlotSelection();
    }

    public void OnExit(){
        Debug.Log("Now Exiting: Action Slot Selection State");
        battleSystem.EnableActionSlotsNav();
        battleSystem.ClearTargetIndicator();
        EventSystem.current.SetSelectedGameObject(null);
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
            default:
                Debug.LogError($"Unable to go back from: Action Slot Selection State. Type of Current Action is {battleSystem.CurrentAction.Type}");
                return null;
        }
    }
}
