using System;
using UnityEngine;
using UnityEngine.UI;

public enum SlotAction{
    Swap,
    Remove,
    Add
}

public class SlotActionPanelManager : MonoBehaviour {
    [SerializeField] Button button;

    void OnEnable() {
        button.Select(); 
    }

    public void OnSwapSelected() {
        MusicManager.Instance.PlaySound("MenuConfirm"); 
        BattleEventManager.Instance.SlotActionSelected(SlotAction.Swap);
    }


    public void OnRemoveSelected() {
        MusicManager.Instance.PlaySound("MenuConfirm"); 
        BattleEventManager.Instance.SlotActionSelected(SlotAction.Remove);
    }
}
