using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

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
    public ActionType Type;
    public BattleUnit User;
    public BattleUnit Target;
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
    [SerializeField] TextMeshProUGUI actionPointText;

    private System.Action backAction;

    private List<BattleUnit> playerUnits;
    private List<BattleUnit> enemyUnits;

    private List<Character> playerCharacters;
    private List<Character> enemyCharacters;

    private BattleState state;
    private BattleState prevState;

    private BattleAction currentAction;
    private BattleUnit currentSelectedPlayerUnit;

    private bool isSelectingEnemy = true;
    private int currentTargetIndex = 0;
    private GameObject lastSelectedTarget = null;

    private bool hasRoundPassed = false;

    private bool canNavigate = true;
    private bool canRunRound = false;

    private int numEscapeAttempts;

    void OnEnable() {
        StartBattle(); 
    }

    public void StartBattle() {
        SetBattleData();

        CallAfterDelay.Create(1.0f, () => {
            StartCoroutine(SetupBattle());
        });
    }

    private void SetBattleData() {
        playerCharacters = BattleManager.Instance.PlayerPartyList;
        enemyCharacters = BattleManager.Instance.EncounterPartyList;

        playerUnits = new();
        enemyUnits = new();

        actionPoints = 3;
        actionPointText.text = $"{actionPoints}";

        numEscapeAttempts = 0;
    }

    public IEnumerator SetupBattle() {
        // initalize party and enemy prefabs in given positions
        // set hud data
        for (int i = 0; i < playerCharacters.Count; i++) {
            BattleUnit unit = new(playerCharacters[i], characterHudList[i]); 
            unit.Setup();
            unit.CurrentModelInstance = Instantiate(unit.Character.CharacterData.CharacterPrefab, partyPositions[i].transform);
            playerUnits.Add(unit);
        }

        for (int i = 0; i < enemyCharacters.Count; i++) {
            enemyCharacters[i].Init();
            BattleUnit unit = new(enemyCharacters[i]); 
            unit.Setup();
            unit.CurrentModelInstance = Instantiate(unit.Character.CharacterData.CharacterPrefab, encounterPositions[i].transform);
            enemyUnits.Add(unit);
        }
        LoadEnemyActionSlots();
        ChangeState(() => CharacterSelection());
        yield return null;
    }

    void Update() {
        if (GameManager.Instance.GameState == GameState.Battle) {
            HandleUpdate();
        } 
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
    
    public void OnBackSelected(InputAction.CallbackContext context) {
        if (!context.started) {
            return;
        }

        Debug.Log(backAction);
        switch (state) {
            case BattleState.Start:
                Debug.Log($"Cannot go back in {state}");
                return;
            case BattleState.CharacterSelect:
                Debug.Log($"Cannot go back in {state}");
                return;
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
                Debug.Log($"Cannot go back in {state}");
                return;
            case BattleState.BattleOver:
                Debug.Log("BattleOver");
                break;
        }

        if (backAction != null) {
            Debug.Log("not null");
            backAction();
        }
    }

    private void ChangeState(System.Action stateChangeFunc) {
        backAction = null;
        stateChangeFunc();
    }

    void LoadEnemyActionSlots() {
        List<int> availableIndecies = new();
        for (int i = 0; i < actionSlots.Count; i++) {
            ActionSlot actionSlot = actionSlots[i].GetComponent<ActionSlot>();
            if (!actionSlot.IsOccupied) {
                availableIndecies.Add(i);
            }
        }

        foreach (var enemyUnit in enemyUnits) {
            List<BattleUnit> validTargets = new();
            foreach (var playerUnit in playerUnits) {
                if (playerUnit.Character.IsAlive) {
                    validTargets.Add(playerUnit);
                }
            }

            if (validTargets.Count <= 0) {
                Debug.Log("WHERE THEM VALID BOYS AT");
                break;
            }

            if (availableIndecies.Count > 0) {
                int randomListIndex = Random.Range(0, availableIndecies.Count);
                int slotIndex = availableIndecies[randomListIndex];

                int playerCharacterIndex= Random.Range(0, validTargets.Count);

                BattleUnit playerUnit = validTargets[playerCharacterIndex];

                BattleAction action = new() {
                    Type = ActionType.Attack,
                    User = enemyUnit,
                    Target = playerUnit,
                };

                ActionSlot actionSlot = actionSlots[slotIndex].GetComponent<ActionSlot>();

                actionSlot.CharacterPortrait.GetComponent<Image>().sprite = enemyUnit.Character.CharacterData.CharacterPortrait;
                actionSlot.CharacterPortrait.SetActive(true);
                actionSlot.BattleAction = action;
                actionSlot.IsOccupied = true;
                // actionSlot.TargetBattleUnit = playerUnit;

                availableIndecies.RemoveAt(randomListIndex);
            } else {
                Debug.Log("WHERE BE THE ACITON SLOTS BRUV");
                break;
            }
        }
        canRunRound = true;
    }

    // State Functions
    void CharacterSelection() {
        prevState = state;
        state = BattleState.CharacterSelect;
        playerPortraits[0].Select();

        foreach (var slot in actionSlots) {
            slot.GetComponent<ActionSlot>().EnableLeftRightNav();
        }

        if (hasRoundPassed) {
            LoadEnemyActionSlots();
            hasRoundPassed = false;
        }
    }

    void ActionSelection() {
        prevState = state;
        state = BattleState.ActionSelection;

        Debug.Log("aciton seelction selected");
        backAction = () => {
            currentSelectedPlayerUnit.Hud.ActionPanel.SetActive(false);
            backAction = null;
            ChangeState(() => CharacterSelection());
        };

        currentSelectedPlayerUnit.Hud.ActionPanel.SetActive(true);
        currentSelectedPlayerUnit.Hud.ActionPanel.transform.GetChild(1).gameObject.GetComponent<Button>().Select();
    }

    void TargetSelection() {
        prevState = state;
        state = BattleState.TargetSelection;
        currentAction.Target = null;

        switch (prevState) {
            case BattleState.ActionSelection:
                backAction = () => {
                   currentAction.Type = ActionType.None; 
                   currentAction.User = null;
                   backAction = null;
                   ChangeState(() => ActionSelection());
                };
                break;
            case BattleState.ActionSlotSelection:
                backAction = () => {
                   currentAction.Type = ActionType.None; 
                   currentAction.User = null;
                   backAction = null;
                   ChangeState(() => ActionSelection());
                };
                break;
            case BattleState.AbilitySelection:
                backAction = () => {

                };
                break;
            case BattleState.ItemSelection:
                backAction = () => {
                    return;
                };
                break;
        }

        if (enemyUnits.Count > 0) {
            isSelectingEnemy = true;
            currentTargetIndex = 0;
        } else if (playerUnits.Count > 0) {
            isSelectingEnemy = false;
            currentTargetIndex = 0;
        }
        UpdateTargetIndicator();
    }

    void ActionSlotSelection() {
        prevState = state;
        state = BattleState.ActionSlotSelection;

        switch (prevState) {
            case BattleState.ActionSelection:
                backAction = () => {
                    currentAction.Type = ActionType.None; 
                    currentAction.User = null;
                    EventSystem.current.SetSelectedGameObject(null);
                    ChangeState(() => ActionSelection());
                };
                break;
            case BattleState.TargetSelection:
                backAction = () => {
                    EventSystem.current.SetSelectedGameObject(null);
                    ChangeState(() => TargetSelection());
                };
                break;
        }

        StartCoroutine(DelayActionSlotSelection());

        foreach (var slot in actionSlots) {
            slot.GetComponent<ActionSlot>().DisableLeftRightNav();
        }
    }

    void BattleOver(bool won) {
        prevState = state;
        state = BattleState.BattleOver;

        foreach(var playerUnit in playerUnits) {
            Destroy(playerUnit.CurrentModelInstance);
        }
        GameManager.Instance.GameState = GameState.FreeRoam;
        BattleManager.Instance.EndBattle();
    }


    // Button Input Functions
    public void OnCharacterSelect(int playerCharacterIndex) {
        currentSelectedPlayerUnit = playerUnits[playerCharacterIndex];
        if (actionPoints <= 0) {
            currentSelectedPlayerUnit = null;
            Debug.Log(actionPoints);
            return;
        }

        if (!currentSelectedPlayerUnit.Character.IsAlive) {
            Debug.Log("bro is dead...");
            return;
        }

        if (state != BattleState.CharacterSelect) {
            Debug.Log("Not in the character select state brother");
            return;
        }

        MusicManager.Instance.PlaySound("MenuConfirm");
        switch (state) {
            case BattleState.CharacterSelect:
                ChangeState(() => ActionSelection());
                break;
            case BattleState.TargetSelection:
                // TargetSelection();
                break;
            default:
                currentSelectedPlayerUnit = null;
                Debug.Log("Default, no state match");
                break;
        }
    }    

    public void OnActionSelection(string actionType) {
        BattleAction battleAction = new();
        MusicManager.Instance.PlaySound("MenuConfirm");
        switch (actionType) {
            case "attack":
                battleAction.Type = ActionType.Attack;
                battleAction.User = currentSelectedPlayerUnit;
                currentAction = battleAction;
                ChangeState(() => TargetSelection());
                break; 
            case "run":
                battleAction.Type = ActionType.Run;
                battleAction.User = currentSelectedPlayerUnit;
                currentAction = battleAction;
                ChangeState(() => ActionSlotSelection());
                break;
        }
        currentSelectedPlayerUnit.Hud.ActionPanel.SetActive(false);
    }

    public void OnTargetNavigate(InputAction.CallbackContext context) {
        if (state != BattleState.TargetSelection) return;

        Vector2 input = context.ReadValue<Vector2>();

        if (canNavigate && (Mathf.Abs(input.x) > 0.5f || Mathf.Abs(input.y) > 0.5f)) {
            // if (input.y > 0.5f) {
            //     if (!isSelectingEnemy && encounterIntances.Count > 0) {
            //         isSelectingEnemy = true;
            //         currentTargetIndex = Mathf.Min(currentTargetIndex, encounterIntances.Count - 1);
            //         UpdateTargetIndicator();
            //     }
            // } else if (input.y < -0.5f) {
            //     if (isSelectingEnemy && partyInstances.Count > 0) {
            //         isSelectingEnemy = false;
            //         currentTargetIndex = Mathf.Min(currentTargetIndex, partyInstances.Count - 1);
            //         UpdateTargetIndicator();
            //     }
            // }

            if (input.x > 0.5f) {
                List<BattleUnit> currentList = isSelectingEnemy ? enemyUnits : playerUnits;
                if (currentList.Count > 0) {
                    currentTargetIndex = (currentTargetIndex + 1) % currentList.Count;
                    UpdateTargetIndicator();
                }
            } else if (input.x < -0.5f) {
                List<BattleUnit> currentList = isSelectingEnemy ? enemyUnits : playerUnits;
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
        List<BattleUnit> currentTargetList = isSelectingEnemy ? enemyUnits: playerUnits;
        if (currentTargetList.Count < 0) return;

        GameObject currentTarget = currentTargetList[currentTargetIndex].CurrentModelInstance;

        if (lastSelectedTarget != null && lastSelectedTarget != currentTarget) {
            lastSelectedTarget.GetComponent<MeshRenderer>().materials[^1].SetFloat("_OutlineThickness", 0f);
        }
        currentTarget.GetComponent<MeshRenderer>().materials[^1].SetFloat("_OutlineThickness", 0.02f);
        lastSelectedTarget = currentTarget;
    }

    public void OnTargetSelected(InputAction.CallbackContext  context) {
        if (context.started && state == BattleState.TargetSelection) {
            List<BattleUnit> currentList = isSelectingEnemy ? enemyUnits : playerUnits;

            if (currentList.Count == 0) return;
            if (currentAction.Type == ActionType.Attack && !isSelectingEnemy) return;

            MusicManager.Instance.PlaySound("MenuConfirm");

            currentAction.Target = currentList[currentTargetIndex]; 

            ChangeState(() => ActionSlotSelection());
        }
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

        MusicManager.Instance.PlaySound("MenuConfirm");
        // Move this code into the ActionSlot class
        actionSlot.CharacterPortrait.GetComponent<Image>().sprite = currentSelectedPlayerUnit.Character.CharacterData.CharacterPortrait;
        actionSlot.CharacterPortrait.SetActive(true);
        actionSlot.IsOccupied = true;

        actionSlot.BattleAction = currentAction;
        // actionSlot.TargetBattleUnit = currentAction.Target;

        canRunRound = true;
        actionPoints--;
        actionPointText.text = $"{actionPoints}";
        ChangeState(() => CharacterSelection());
    }

    IEnumerator DelayActionSlotSelection() {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        actionSlots[0].Select();
    }

    IEnumerator DelayInput() {
        yield return new WaitForEndOfFrame();
    }

    public void OnRoundRunSelected(InputAction.CallbackContext context) {
        if (context.started) {
            if (!canRunRound && state == BattleState.CharacterSelect) {
                Debug.Log("no actions in the action bar");
                return;
            }

            if (state != BattleState.CharacterSelect) {
                Debug.Log("Currently cannot activate a round");
                return;
            }
            MusicManager.Instance.PlaySound("MenuConfirm");
            StartCoroutine(RunRound());
        }

    }

    // Running actions/battle
    IEnumerator RunRound() {
        prevState = state;
        state = BattleState.RunningRound;

        foreach (var slot in actionSlots) {
            ActionSlot actionSlot = slot.GetComponent<ActionSlot>();
            if (!actionSlot.IsOccupied) {
                continue;
            }

            if (!actionSlot.BattleAction.User.Character.IsAlive) {
                continue;
            }

            if (actionSlot.BattleAction.Type == ActionType.Attack) {
                //attack
                yield return StartCoroutine(RunAttack(actionSlot));
            } else if (actionSlot.BattleAction.Type == ActionType.Run) {
                yield return StartCoroutine(TryToEscape());
            }
        }

        if (state != BattleState.BattleOver) {
            foreach (var slot in actionSlots) {
                ActionSlot actionSlot = slot.GetComponent<ActionSlot>();
                if (!actionSlot.IsOccupied) {
                    continue;
                }
                actionSlot.ResetData();
            }
            canRunRound = false;
            actionPoints = 3;
            actionPointText.text = $"{actionPoints}";
            hasRoundPassed = true;
            numEscapeAttempts = 0;
            ChangeState(() => CharacterSelection());
        }
        yield return null;
    }

    IEnumerator RunAttack(ActionSlot actionSlot) {
        BattleUnit user = actionSlot.BattleAction.User;
        BattleUnit target = actionSlot.BattleAction.Target;
        int totalDamage = CalculateAttackDamage(user.Character, target.Character); 
        target.Character.DecreaseHP(totalDamage);

        GameObject damageTextObject = actionSlot.BattleAction.Target.CurrentModelInstance.transform.GetChild(0).gameObject;
        damageTextObject.SetActive(true);
        damageTextObject.GetComponent<DamageText>().text.text = $"{totalDamage}";

        yield return new WaitForSeconds(1f);

        damageTextObject.SetActive(false);

        if (target.Character.HP <= 0) {
            yield return StartCoroutine(OnCharacterDeath(actionSlot));
        }

        yield return new WaitForEndOfFrame();
    }

    int CalculateAttackDamage(Character user, Character target) {
        int userDamage = user.CalculateBasicAttackDamage();
        int targetDefense = target.CalculateDefense();
        Debug.Log($"User Damage: {userDamage} Target Defense: {targetDefense}");
        return userDamage - targetDefense;
    }

    IEnumerator TryToEscape() {
        numEscapeAttempts++;
        float f = 5 * 128 / 7 + 30 * numEscapeAttempts;
        f %= 256;

        if (Random.Range(0, 256) < f) {
            BattleOver(true);
        }

        yield return null;
    }
    // ability -> calculate damage, check for status effects, check if you can cast
    // items -> calculate value, check for any status effects

    IEnumerator OnCharacterDeath(ActionSlot actionSlot) {
        // get XP;
        // death animation
        // remove model from instanceList or mark as dead and skip over;
        BattleUnit dyingUnit = actionSlot.BattleAction.Target;
        if (dyingUnit.Character.CharacterData.CharacerType == CharacerType.Enemy) {
            dyingUnit.CurrentModelInstance.GetComponent<MeshRenderer>().materials[0].color = Color.red;
            foreach(var enemyUnit in enemyUnits) {
                if(enemyUnit == dyingUnit) {
                    enemyUnits.Remove(enemyUnit);
                    break;
                }
            }
        } else {
            dyingUnit.CurrentModelInstance.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().materials[0].color = Color.red;
            foreach(var playerUnit in playerUnits) {
                if(playerUnit == dyingUnit) {
                    // playerUnits.Remove(playerUnit);
                    playerUnit.Character.IsAlive = false;
                    break;
                }
            }
        }

        if (enemyUnits.Count <= 0 || CheckIfPartyDead()) {
            BattleOver(true);
        }

        yield return new WaitForEndOfFrame();
    }

    bool CheckIfPartyDead() {
        int numDead = 0;
        foreach(var playerUnit in playerUnits) {
            if (!playerUnit.Character.IsAlive) {
                numDead++;
            }
        }

        return numDead == 3;
    }

}