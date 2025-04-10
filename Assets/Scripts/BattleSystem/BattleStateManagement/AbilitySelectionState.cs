using UnityEngine;
using UnityEngine.EventSystems;

public class AbilitySelectionState : IBattleState {
    private readonly BattleSystem battleSystem;
    private const BattleState _state = BattleState.AbilitySelection;

    public AbilitySelectionState(BattleSystem system) {
        battleSystem = system;
    }

    public BattleState State {
        get {
            return _state;
        }
    }


    public void OnEnter() {
        Debug.Log("Now Entering: Ability Selection State");
        battleSystem.CurrentAction.AbilityBase = null;
        battleSystem.AbilityMenu.PopulateAbilities(battleSystem.CurrentSelectedPlayerUnit.Character.Abilities);
        battleSystem.AbilityPanel.SetActive(true);
    }

    public void OnExit() {
        Debug.Log("Now Exiting: Ability Selection State");
        battleSystem.AbilityPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public IBattleState OnBack() {
        return new ActionSelectionState(battleSystem);
    }
}
