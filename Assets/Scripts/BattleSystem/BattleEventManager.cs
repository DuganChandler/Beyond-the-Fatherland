using System;
using UnityEditor.Rendering;
using UnityEngine;

public class BattleEventManager : MonoBehaviour {
    private static BattleEventManager _Instance;
    public static BattleEventManager Instance { 
        get { 
            if (!_Instance) {
                _Instance = new GameObject().AddComponent<BattleEventManager>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        } 
    }

    public event Action<ItemSlot> OnItemSelected;
    public event Action<AbilityBase> OnAbilitySelected;
    public event Action<SlotAction> OnSlotActionSelected;
    public event Action<ActionSlot, SlotAction, bool> OnSlotSelected;
    public event Action OnAnimationCompleted; 

    public void ItemSelected(ItemSlot itemSlot) {
        OnItemSelected?.Invoke(itemSlot);
    }

    public void AbilitySelected(AbilityBase abilityBase) {
        OnAbilitySelected?.Invoke(abilityBase);
    }

    public void SlotActionSelected(SlotAction slotAction) {
        OnSlotActionSelected?.Invoke(slotAction);
    }

    public void SlotSelected(ActionSlot actionSlot, SlotAction slotAction, bool swapped = false) {
        OnSlotSelected?.Invoke(actionSlot,slotAction, swapped);
    }

     public void AnimationCompleted() {
         OnAnimationCompleted?.Invoke();
     } 
}
