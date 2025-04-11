using UnityEngine;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour {
    [SerializeField] private GameObject characterPortrait;
    public GameObject CharacterPortrait {
        get {
            return characterPortrait;
        } set {
            characterPortrait = value;
        }
    }

    [SerializeField] int slotNumber; 
    public int SlotNumber { get => slotNumber; set { slotNumber =  value; } }

    public bool IsSwapping  { get; set; } 

    [SerializeField] private Button defaultLeftNavButton;

    private BattleAction battleAction;
    public BattleAction BattleAction {
        get {
            return battleAction;
        } set {
            battleAction = value;
        }
    }

    public bool IsOccupied { get; set; }

    public void ResetData() {
        battleAction.User = null;
        battleAction.Target = null;
        battleAction.Type = ActionType.None;
        // _targetBattleUnit = null;
        IsOccupied = false;

        characterPortrait.SetActive(false);
    }

    public void SetData(BattleAction action, Sprite portrait, bool isOccupied) {
        battleAction = action;
        characterPortrait.GetComponent<Image>().sprite = portrait;
        IsOccupied = isOccupied;
        characterPortrait.SetActive(isOccupied);
    }
}
