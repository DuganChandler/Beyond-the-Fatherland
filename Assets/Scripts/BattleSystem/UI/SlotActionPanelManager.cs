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

    public event Action<SlotAction> OnSlotActionSelected;

    void OnEnable() {
        button.Select(); 
    }

    public void OnSwapSelected() {
        MusicManager.Instance.PlaySound("MenuConfirm"); 
        OnSlotActionSelected?.Invoke(SlotAction.Swap);
    }


    public void OnRemoveSelected() {
        MusicManager.Instance.PlaySound("MenuConfirm"); 
        OnSlotActionSelected?.Invoke(SlotAction.Remove);
    }
}
