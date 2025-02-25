using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState {
    Start,
    CharacterSelect,
    ActionSelecton,
    AbilitySelection,
    ItemSelection,
    TargetSelection,
    ActionSlotSelection,
    RunningRound,
    BattleOver
}

public class BattleSystem : MonoBehaviour {
    [Header("Battle Setup")]
    [SerializeField] private List<GameObject> partyPositions;
    [SerializeField] private List<GameObject> enemyPositions;

    // [Header("BattleUI")]
    // [SerializeField] CharacterBattleHud BattleHud;
    // Need ItemSelectionUI, AbilitySelectionUI, DialogueBox 
    BattleState state;

    private void HandleUpdate() {
        switch (state) {
            case BattleState.Start:
                Debug.Log("Start");
                break;
            case BattleState.CharacterSelect:
                Debug.Log("CharacterSelect");
                break;
            case BattleState.ActionSelecton:
                Debug.Log("ActionSelection");
                break;
            case BattleState.AbilitySelection:
                Debug.Log("AbilitySelection");
                break;
            case BattleState.ItemSelection:
                Debug.Log("ItemSelection");
                break;
            case BattleState.TargetSelection:
                Debug.Log("TargetSelection");
                break;
            case BattleState.ActionSlotSelection:
                Debug.Log("ActionSlotSelection");
                break;
            case BattleState.RunningRound:
                Debug.Log("RunningRound");
                break;
            case BattleState.BattleOver:
                Debug.Log("BattleOver");
                break;
        }
    }

    void Start() {
        
    }

    void Update() {
        
    }
}
