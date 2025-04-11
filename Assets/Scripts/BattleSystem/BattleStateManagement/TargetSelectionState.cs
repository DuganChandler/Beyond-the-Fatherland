using UnityEngine;

public class TargetSelectionState : IBattleState {
    private readonly BattleSystem battleSystem;
    private const BattleState _state = BattleState.TargetSelection;

    public TargetSelectionState(BattleSystem system) {
        battleSystem = system;
    }

    public BattleState State {
        get {
            return _state;
        }
    }

    public void OnEnter() {
        Debug.Log("Entering: Target Selection State");
        battleSystem.CurrentAction.Target = null;
        battleSystem.HandleTargetSelection();

        battleSystem.InfoPanelManager.SetText("Select a Target", true);
    }

    public void OnExit() {
        Debug.Log("Exiting: Target Selection State");
        battleSystem.ClearTargetIndicator();
    }

    public IBattleState OnBack() {
        switch (battleSystem.CurrentAction.Type) {
            case ActionType.Attack:
                return new ActionSelectionState(battleSystem);
            case ActionType.Ability:
                return new AbilitySelectionState(battleSystem);
            case ActionType.Item:
                return new ItemSelectionState(battleSystem);
            default:
                Debug.LogError($"Unable to go back from: Target Selection State. Type of Current Action is {battleSystem.CurrentAction.Type}");
                return null;
        }
    }
}
