using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionState : IBattleState {
    private readonly BattleSystem battleSystem;

    public CharacterSelectionState(BattleSystem system) {
        battleSystem = system;
    }

    private const BattleState _state = BattleState.CharacterSelect;

    public BattleState State { 
        get {
            return _state;
        }
    }

    public void OnEnter() {
        Debug.Log("Entering: Character Selection State");
        battleSystem.PlayerPortraits[0].Select();
        battleSystem.CurrentAction.ResetBattleAction();
        battleSystem.InfoPanelManager.SetText("Select a Character", true);
    }

    public void OnExit() {
        Debug.Log("Exiting: Character Selection State");
    }

    public IBattleState OnBack() {
        Debug.Log("You are not allowed to back out of Character Selection State");
        return null;
    }
}
