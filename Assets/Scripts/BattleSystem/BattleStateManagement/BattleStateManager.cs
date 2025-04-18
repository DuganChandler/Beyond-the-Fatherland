using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager : MonoBehaviour {
    private readonly Stack<IBattleState> states = new();
    public IBattleState CurrentState {get; private set; }

    public void ChangeState(IBattleState newState) {
        StartCoroutine(DelayStateChange(newState));
    }

    IEnumerator DelayStateChange(IBattleState newState) {
        yield return new WaitForEndOfFrame();
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
