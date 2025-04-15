using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionSelectionState : IBattleState {
    private readonly BattleSystem battleSystem;
    private const BattleState _state = BattleState.ActionSelection;

    public ActionSelectionState(BattleSystem system) {
        battleSystem = system;
    }

    public BattleState State { 
        get {
            return _state;
        }
    }

    public void OnEnter() {
        Debug.Log("Entering: Action Selection State");
        battleSystem.ActionButtonManager.SetButtonText("Slot Actions", "Attack", "Escape", "Abilities");
        battleSystem.CurrentAction.ResetBattleAction();

        battleSystem.DelayInput();

        // battleSystem.CurrentAction.Type = ActionType.None;
        // battleSystem.CurrentAction.AbilityBase = null;
        // battleSystem.CurrentAction.ItemSlot = null;

        // battleSystem.CurrentSelectedPlayerUnit.Hud.ActionPanel.SetActive(true);
        // battleSystem.CurrentSelectedPlayerUnit.Hud.ActionPanel.transform.GetChild(1).gameObject.GetComponent<Button>().Select();

        battleSystem.InfoPanelManager.SetText("Select an Action", true);
    }

    public void OnExit() {
        Debug.Log("Exiting: Action Selection State");
    }

    public IBattleState OnBack() {
        Debug.Log("You are not allowed to back out of Character Selection State");
        return null;
    }
}
