using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotSwapState : IBattleState {
    private readonly BattleSystem battleSystem;

    public SlotSwapState(BattleSystem system) {
        battleSystem = system;
    }

    private const BattleState _state = BattleState.SlotSwap;

    public BattleState State { 
        get {
            return _state;
        }
    }

    public void OnEnter() {
        battleSystem.InfoPanelManager.SetText("Select any two slots to swap", true);
        battleSystem.ActionBarManager.HandleActionSlotSelection(SlotAction.Swap);
    }

    public void OnExit() {
        Debug.Log("Exiting: Slot Swap Selection State");
        battleSystem.ClearTargetIndicator();
        EventSystem.current.SetSelectedGameObject(null);
        battleSystem.InfoPanelManager.SetText("", false);
    }

    public IBattleState OnBack() {
        Debug.Log("SLot Swap State: Back -> Slot Action State");
        return null;
    }
}
