using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotActionSelectionState : IBattleState {
    private readonly BattleSystem battleSystem;

    public SlotActionSelectionState(BattleSystem system) {
        battleSystem = system;
    }

    private const BattleState _state = BattleState.SlotActionSelection;

    public BattleState State { 
        get {
            return _state;
        }
    }

    public void OnEnter() {
        Debug.Log("Entering: Slot Action Selection State");
        battleSystem.SlotActionPanel.SetActive(true);
    }

    public void OnExit() {
        Debug.Log("Exiting: Slot Action Selection State");
        battleSystem.SlotActionPanel.SetActive(false);
    }

    public IBattleState OnBack() {
        Debug.Log("Slot Action Selection State: Back -> Character Selection State");
        return new CharacterSelectionState(battleSystem);
    }
}
