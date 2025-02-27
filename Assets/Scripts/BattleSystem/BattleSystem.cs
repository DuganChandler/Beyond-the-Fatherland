using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public enum BattleState {
    Start,
    CharacterSelect,
    ActionSelection,
    AbilitySelection,
    ItemSelection,
    TargetSelection,
    ActionSlotSelection,
    RunningRound,
    BattleOver
}

public enum ActionType {
    Attack,
    Ability,
    Item,
    Run,
    None
} 

public struct BattleAction {
    // public BattleAction(ActionType type, Character user, Character enemy) {
    //     Type = type;
    //     User = user;
    //     Enemy = enemy;
    // }

    public ActionType Type;
    public Character User;
    public Character Target;

    // ability
}

public class BattleSystem : MonoBehaviour {
    [Header("Battle Setup")]
    [SerializeField] private List<GameObject> partyPositions;
    [SerializeField] private List<GameObject> encounterPositions;
    [SerializeField] private int actionPoints;

    [Header("Battle UI")]
    [SerializeField] List<Button> playerPortraits; 
    [SerializeField] List<Button> actionSlots; 
    [SerializeField] List<CharacterHud> characterHudList;
    [SerializeField] Material partyOutline;
    [SerializeField] Material enemyOutline;

    private List<Character> playerParty;
    private List<Character> encounterParty; 

    private List<GameObject> encounterIntances;
    private List<GameObject> partyInstances;

    private BattleState state;
    private BattleState prevState;

    private BattleAction currentAction;
    private GameObject currentSelectedCharacter;


    private bool isSelectingEnemy = true;
    private int currentTargetIndex = 0;
    private GameObject lastSelectedTarget = null;

    private bool canNavigate = true;

    void OnEnable() {
        StartBattle(); 
    }

    public void StartBattle() {
        // initalize party and enemies stats
        playerParty = BattleManager.Instance.PlayerPartyList; 
        encounterParty = BattleManager.Instance.EncounterPartyList;
        actionPoints = 3;

        encounterIntances = new();
        partyInstances = new();
        // Play battle music;
        CallAfterDelay.Create(1.0f, () => {
            StartCoroutine(SetupBattle());
        });
    }

    public IEnumerator SetupBattle() {
        // initalize party and enemy prefabs in given positions
        // set hud data
        for (int i = 0; i < playerParty.Count; i++) {
            GameObject playerCharacterPrefab = playerParty[i].CharacterData.CharacterPrefab;
            Transform playerCharacterPosition = partyPositions[i].transform;

            characterHudList[i].SetData(playerParty[i]);

            var partyInstance = Instantiate(playerCharacterPrefab, playerCharacterPosition);
            partyInstances.Add(partyInstance);
        }

        for (int i = 0; i < encounterParty.Count; i++) {
            GameObject encounterCharacterPrefab = encounterParty[i].CharacterData.CharacterPrefab;
            Transform encounterCharacterPosition = encounterPositions[i].transform;

            var encounterInstance = Instantiate(encounterCharacterPrefab, encounterCharacterPosition);
            encounterIntances.Add(encounterInstance);
            encounterParty[i].Init();
        }
        CharacterSelection();
        yield return null;
    }

    void Update() {
        if (GameManager.Instance.GameState == GameState.Battle) {
            HandleUpdate();
        } 

        // if (Input.GetKeyDown(KeyCode.K)) {
        //     playerParty[1].DecreaseHP(20);
        // }
    }

    private void HandleUpdate() {
        switch (state) {
            case BattleState.Start:
                Debug.Log("Start");
                break;
            case BattleState.CharacterSelect:
                Debug.Log("CharacterSelect");
                break;
            case BattleState.ActionSelection:
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

    void CharacterSelection() {
        prevState = state;
        state = BattleState.CharacterSelect;
        playerPortraits[0].Select();
    }

    public void OnCharacterSelect(GameObject characterHud) {
        currentSelectedCharacter = characterHud;
        if (actionPoints <= 1) {
            currentSelectedCharacter = null;
            return;
        }

        switch (state) {
            case BattleState.CharacterSelect:
                ActionSelection();                
                break;
            case BattleState.TargetSelection:
                // TargetSelection();
                break;
            default:
                currentSelectedCharacter = null;
                Debug.Log("Default, no state match");
                break;
        }
    }    

    void ActionSelection() {
        prevState = state;
        state = BattleState.ActionSelection;
        GameObject actionPanel = currentSelectedCharacter.transform.GetChild(4).gameObject;
        actionPanel.SetActive(true);
        actionPanel.transform.GetChild(1).gameObject.GetComponent<Button>().Select();
    }

    void TargetSelection(BattleAction battleAction) {
        prevState = state;
        state = BattleState.TargetSelection;
        currentAction = battleAction;

        if (encounterIntances.Count > 0) {
            isSelectingEnemy = true;
            currentTargetIndex = 0;
        } else if (partyInstances.Count > 0) {
            isSelectingEnemy = false;
            currentTargetIndex = 0;
        }
        UpdateTargetIndicator();
    }

    public void OnTargetNavigate(InputAction.CallbackContext context) {
        if (state != BattleState.TargetSelection) return;

        Vector2 input = context.ReadValue<Vector2>();

        if (canNavigate && (Mathf.Abs(input.x) > 0.5f || Mathf.Abs(input.y) > 0.5f)) {
            if (input.y > 0.5f) {
                if (!isSelectingEnemy && encounterIntances.Count > 0) {
                    isSelectingEnemy = true;
                    currentTargetIndex = Mathf.Min(currentTargetIndex, encounterIntances.Count - 1);
                    UpdateTargetIndicator();
                }
            } else if (input.y < -0.5f) {
                if (isSelectingEnemy && partyInstances.Count > 0) {
                    isSelectingEnemy = false;
                    currentTargetIndex = Mathf.Min(currentTargetIndex, partyInstances.Count - 1);
                    UpdateTargetIndicator();
                }
            }

            if (input.x > 0.5f) {
                List<GameObject> currentList = isSelectingEnemy ? encounterIntances : partyInstances;
                if (currentList.Count > 0) {
                    currentTargetIndex = (currentTargetIndex + 1) % currentList.Count;
                    UpdateTargetIndicator();
                }
            } else if (input.x < -0.5f) {
                List<GameObject> currentList = isSelectingEnemy ? encounterIntances : partyInstances;
                if (currentList.Count > 0) {
                    currentTargetIndex = (currentTargetIndex - 1 + currentList.Count) % currentList.Count;
                    UpdateTargetIndicator();
                }
            }

            canNavigate = false;
        }

        // Reset the navigation flag when the stick returns to neutral.
        if (Mathf.Abs(input.x) < 0.5f && Mathf.Abs(input.y) < 0.5f) {
            canNavigate = true;
        }
    } 

    void UpdateTargetIndicator() {
        List<GameObject> currentTargetList = isSelectingEnemy ? encounterIntances : partyInstances;
        if (currentTargetList.Count < 0) return;

        GameObject currentTarget = currentTargetList[currentTargetIndex];

        if (lastSelectedTarget != null && lastSelectedTarget != currentTarget) {
            lastSelectedTarget.GetComponent<MeshRenderer>().materials[^1].SetFloat("_OutlineThickness", 0f);
        }
        currentTarget.GetComponent<MeshRenderer>().materials[^1].SetFloat("_OutlineThickness", 0.02f);
        lastSelectedTarget = currentTarget;
    }

    public void OnTargetSelected(InputAction.CallbackContext  context) {
        if (context.started && state == BattleState.TargetSelection) {
            List<Character> currentList = isSelectingEnemy ? encounterParty : playerParty;

            if (currentList.Count == 0) return;
            if (currentAction.Type == ActionType.Attack && !isSelectingEnemy) return;

            Character selectedCharacter = currentList[currentTargetIndex];
            currentAction.Target = selectedCharacter;
            ActionSlotSelection(currentAction);
        }
    }

    public void OnActionSelection(string actionType) {
        BattleAction battleAction = new();
        switch (actionType) {
            case "attack":
                battleAction.Type = ActionType.Attack;
                battleAction.User = currentSelectedCharacter.GetComponent<CharacterHud>().Character;
                TargetSelection(battleAction);
                break; 
            case "run":
                battleAction.Type = ActionType.Run;
                battleAction.User = currentSelectedCharacter.GetComponent<CharacterHud>().Character;
                ActionSlotSelection(battleAction);
                break;
        }
        GameObject actionPanel = currentSelectedCharacter.transform.GetChild(4).gameObject;
        actionPanel.SetActive(false);
    }

    void ActionSlotSelection(BattleAction battleAction) {
        prevState = state;
        state = BattleState.ActionSlotSelection;
        currentAction = battleAction;

        StartCoroutine(DelayActionSlotSelection());

        foreach (var slot in actionSlots) {
            slot.GetComponent<ActionSlot>().DisableLeftRightNav();
        }
    }

    IEnumerator DelayActionSlotSelection() {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        actionSlots[0].Select();
    }

    public void OnActionSlotSelection(ActionSlot actionSlot) {
        if (state != BattleState.ActionSlotSelection) {
            Debug.Log("Not in the ActionSlotSelection state");
            return;
        }

        if (actionSlot.IsOccupied) {
            Debug.Log("Choose another slot");
            return;
        }

        // Move this code into the ActionSlot class
        actionSlot.CharacterPortrait.GetComponent<Image>().sprite = currentSelectedCharacter.GetComponent<CharacterHud>().Character.CharacterData.CharacterPortrait;
        actionSlot.CharacterPortrait.SetActive(true);
        actionSlot.BattleAction = currentAction;
        actionSlot.IsOccupied = true;

        foreach (var slot in actionSlots) {
            slot.GetComponent<ActionSlot>().EnableLeftRightNav();
        }

        CharacterSelection();
    }
}