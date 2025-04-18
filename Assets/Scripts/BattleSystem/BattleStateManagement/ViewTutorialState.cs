using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTutorialState : IBattleState {
    private readonly BattleSystem battleSystem;

    public ViewTutorialState(BattleSystem system) {
        battleSystem = system;
    }

    private const BattleState _state = BattleState.ViewTutorial;

    public BattleState State { 
        get {
            return _state;
        }
    }

    public void OnEnter() {
        Debug.Log("Entering: View Tutorial State");
        battleSystem.ActionButtonManager.SetButtonText("", "", "Back", "", false);
        battleSystem.InfoPanelManager.SetText("", false);
        battleSystem.TutorialPanel.SetActive(true);
    }

    public void OnExit() {
        Debug.Log("Exiting: View Tutorial State");
        battleSystem.TutorialPanel.SetActive(false);
    }

    public IBattleState OnBack() {
        Debug.Log("View Tutorial: Back -> Action Selection State");
        return new ActionSelectionState(battleSystem);
    }
}
