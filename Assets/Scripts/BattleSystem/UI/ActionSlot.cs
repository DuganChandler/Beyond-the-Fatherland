using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ActionSlot : MonoBehaviour {
    [SerializeField] private GameObject characterPortrait;
    [SerializeField] private Button defaultLeftNavButton;

    private Animator animator;

    public bool IsOccupied { get; set; }
    public bool IsSwapping { get; set; } 

    public GameObject CharacterPortrait {
        get {
            return characterPortrait;
        } set {
            characterPortrait = value;
        }
    }

    private bool _highlight = false;
    public bool Highlight {
        get {
            return _highlight;
        } set {
            _highlight = value;
            animator.SetBool("Target", value);
        }
    }

    private BattleAction battleAction = null;
    public BattleAction BattleAction {
        get {
            return battleAction;
        } set {
            battleAction = value;
        }
    }

    void Start() {
        animator = GetComponent<Animator>();
    } 

    public void SetToNormal() {
        animator.SetTrigger("Normal");
    }

    public void ResetData() {
        battleAction.User = null;
        battleAction.Target = null;
        battleAction.Type = ActionType.None;
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
