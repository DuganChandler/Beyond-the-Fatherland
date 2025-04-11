using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSelectionState : IBattleState {
    private readonly BattleSystem battleSystem;
    private const BattleState _state = BattleState.ItemSelection;

    public ItemSelectionState(BattleSystem system) {
        battleSystem = system;
    }

    public BattleState State {
        get {
            return _state;
        }
    }

    public void OnEnter() {
        Debug.Log("Entering: Item Selection State");
        battleSystem.CurrentAction.ItemSlot = null;
        battleSystem.ItemMenu.PopulateInventory(battleSystem.PlayerInventory, ItemCategory.Combat);
        battleSystem.ItemPanel.SetActive(true);
    }

    public void OnExit() {
        Debug.Log("Exiting: Item Selection State");
        battleSystem.ItemPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public IBattleState OnBack() {
        return new ActionSelectionState(battleSystem);
    }
}
