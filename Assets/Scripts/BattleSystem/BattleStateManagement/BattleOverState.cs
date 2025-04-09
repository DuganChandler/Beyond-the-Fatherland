using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOverState : IBattleState {
    private readonly BattleSystem battleSystem;

    public BattleOverState(BattleSystem system) {
        battleSystem = system;
    }

    private const BattleState _state = BattleState.BattleOver;

    public BattleState State { 
        get {
            return _state;
        }
    }

    public void OnEnter() {
        Debug.Log("Entering: Battle Over State");
    }

    public void OnExit() {
        Debug.Log("Exiting: Battle Over State");
    }

    public IBattleState OnBack() {
        Debug.Log("You are not allowed to back out of Battle Over State");
        return null;
    }
}
