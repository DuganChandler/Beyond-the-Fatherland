using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager {
    private readonly Stack<IBattleState> states = new();
    public IBattleState CurrentState {get; private set; }

    public void ChangeState(IBattleState newState) {
        CurrentState?.OnExit();

        CurrentState = newState;
        CurrentState.OnEnter();

        states.Push(CurrentState);

        Debug.Log(CurrentState.State);
    }

    public void Back() {
        if (CurrentState != null) {
            IBattleState backState = CurrentState.OnBack();

            if (backState != null) {
                CurrentState.OnExit();
                CurrentState = backState;
                CurrentState.OnEnter();

                states.Pop();
            }

            MusicManager.Instance.PlaySound("MenuBack");
        }
    }

    public void CleanStates() {
        states.Clear();
    }
}
