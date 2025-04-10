using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunRoundState : IBattleState {
    private readonly BattleSystem battleSystem;

    public RunRoundState(BattleSystem system) {
        battleSystem = system;
    }

    private const BattleState _state = BattleState.RunningRound;

    public BattleState State { 
        get {
            return _state;
        }
    }

    public void OnEnter() {
        Debug.Log("Entering: Run Round State");
        battleSystem.HandleRunRound();
    }

    public void OnExit() {
        Debug.Log("Exiting: Run Round State");
    }

    public IBattleState OnBack() {
        Debug.Log("You are not allowed to back out of Run Round State");
        return null;
    }
}
