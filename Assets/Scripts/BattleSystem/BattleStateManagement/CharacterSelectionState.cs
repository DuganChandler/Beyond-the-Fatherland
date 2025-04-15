using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        battleSystem.ActionButtonManager.SetButtonText("", "Select", "Back", "");
        battleSystem.PlayerPortraits[0].Select();

        battleSystem.CurrentAction.User = null;
        battleSystem.CurrentAction.AbilityBase = null;
        battleSystem.CurrentAction.ItemSlot = null;
        battleSystem.CurrentAction.Target = null;

        battleSystem.InfoPanelManager.SetText($"{battleSystem.CurrentAction.Type}: Select a Character", true);
    }

    public void OnExit() {
        Debug.Log("Exiting: Character Selection State");
        EventSystem.current.SetSelectedGameObject(null);
    }

    public IBattleState OnBack() {
        Debug.Log("Character Selection -> Action Selection State");
        return new ActionSelectionState(battleSystem);
    }
}
