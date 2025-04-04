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

    private Button actionSlotButton;
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

    public void DisableLeftRightNav() {
        Navigation nav = actionSlotButton.navigation;
        nav.selectOnLeft = null;
        actionSlotButton.navigation = nav;
    }

    public void EnableLeftRightNav() {
        Navigation nav = actionSlotButton.navigation;
        nav.selectOnLeft = defaultLeftNavButton;
        actionSlotButton.navigation = nav;
    }

    void Start() {
        actionSlotButton = GetComponent<Button>();
    }
}
