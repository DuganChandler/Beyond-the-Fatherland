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
    SlotActionSelection,
    SlotSwap,
    SlotRemoving,
    TargetSelection,
    ActionSlotSelection,
    RunningRound,
    EndRound,
    BattleOver,
}

public enum ActionType {
    Attack,
    Ability,
    Item,
    Run,
    None
} 

public class BattleSystem : MonoBehaviour, IBattleActions {
    [Header("Battle Setup")]
    [SerializeField] private List<BattlePosition> partyPositions;
    [SerializeField] private List<BattlePosition> encounterPositions;
    [SerializeField] private int actionPoints;
    [SerializeField] private ItemUser itemUser;
    [SerializeField] private AbilityExecutor abilityExecutor;

    [Header("Battle UI")]
    [SerializeField] List<Button> playerPortraits; 
    [SerializeField] List<CharacterHud> characterHudList;
    [SerializeField] Material partyOutline;
    [SerializeField] Material enemyOutline;
    [SerializeField] TextMeshProUGUI actionPointText;
    [SerializeField] PointerManager pointerManager;
    [SerializeField] ItemMenu itemMenu;
    [SerializeField] GameObject itemPanel;
    [SerializeField] GameObject abilityPanel;
    [SerializeField] AbilityMenu abilityMenu;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] GameObject slotActionPanel;
    [SerializeField] ActionBarManager actionBarManager;
    [SerializeField] InfoPanelManager infoPanelManager;
    [SerializeField] ActionButtonManager actionButtonManager;

    [SerializeField] BattleStateManager stateManager;

    private List<BattleUnit> playerUnits;
    private List<BattleUnit> enemyUnits;

    private List<Character> playerCharacters;
    private List<Character> enemyCharacters;

    private BattleAction currentAction;
    private BattleUnit currentSelectedPlayerUnit;

    private bool isSelectingEnemy = true;
    private int currentTargetIndex = 0;
    private GameObject lastSelectedTarget = null;

    private Inventory playerInventory;

    private bool hasRoundPassed = false;
    private bool canNavigate = true;
    private bool canRunRound = false;

    private int numEscapeAttempts;

    public BattleStateManager StateManager { get => stateManager; }
    public List<Button> PlayerPortraits { get => playerPortraits; }
    public GameObject AbilityPanel { get => abilityPanel; }
    public GameObject ItemPanel{ get => itemPanel; }
    public GameObject SlotActionPanel{ get => slotActionPanel; }
    public ActionBarManager ActionBarManager { get => actionBarManager; }
    public AbilityMenu AbilityMenu { get => abilityMenu; }
    public ItemMenu ItemMenu { get => itemMenu; }
    public BattleUnit CurrentSelectedPlayerUnit { get => currentSelectedPlayerUnit; }
    public BattleAction CurrentAction { get => currentAction;  set => currentAction = value; }
    public Inventory PlayerInventory { get => playerInventory; }
    public InfoPanelManager InfoPanelManager { get => infoPanelManager; }
    public ActionButtonManager ActionButtonManager { get => actionButtonManager; } 
    public List<BattlePosition> EncounterPositions { get => encounterPositions; } 
    public List<BattleUnit> EnemyUnits { get => enemyUnits; }

    void Awake() {
        StartBattle(); 
    }

    void OnEnable() {
        BattleEventManager.Instance.OnItemSelected += HandleItemSelection;
        BattleEventManager.Instance.OnAbilitySelected += HandleAbilitySelection;
        BattleEventManager.Instance.OnSlotActionSelected += HandleSlotActionSelected;
        BattleEventManager.Instance.OnSlotSelected += HandleActionSlotSelected;
    }

    void OnDisable() {
        BattleEventManager.Instance.OnItemSelected -= HandleItemSelection;
        BattleEventManager.Instance.OnAbilitySelected -= HandleAbilitySelection;
        BattleEventManager.Instance.OnSlotActionSelected -= HandleSlotActionSelected;
        BattleEventManager.Instance.OnSlotSelected -= HandleActionSlotSelected;
    }

    public void StartBattle() {
        SetBattleData();
        StartCoroutine(SetupBattle());
    }

    private void SetBattleData() {
        playerCharacters = BattleManager.Instance.PlayerPartyList;
        enemyCharacters = BattleManager.Instance.EncounterPartyList;

        playerUnits = new();
        enemyUnits = new();
        currentAction = new(ActionType.None, null, null, null, null);

        playerInventory = BattleManager.Instance.PlayerInventory;

        actionPoints = 3;
        actionPointText.text = $"{actionPoints}";

        numEscapeAttempts = 0;
        // currentRound = 1;
    }

    public IEnumerator SetupBattle() {
        yield return new WaitForEndOfFrame(); 
        MusicManager.Instance.PlayMusic("BattleTheme");

        for (int i = 0; i < playerCharacters.Count; i++) {
            BattleUnit unit = new(playerCharacters[i], partyPositions[i], characterHudList[i]); 
            unit.Setup();
            unit.CurrentModelInstance = Instantiate(unit.Character.CharacterData.CharacterPrefab, partyPositions[i].transform);

            playerUnits.Add(unit);
        }

        for (int i = 0; i < enemyCharacters.Count; i++) {
            BattleUnit unit = new(enemyCharacters[i], encounterPositions[i]); 
            unit.Setup();
            unit.CurrentModelInstance = Instantiate(unit.Character.CharacterData.CharacterPrefab, encounterPositions[i].transform);

            enemyUnits.Add(unit);
        }

        LoadEnemyActionSlots();
        EventSystem.current.SetSelectedGameObject(null);
        StateManager.ChangeState(new ActionSelectionState(this));
    }

    void LoadEnemyActionSlots() {
        List<int> availableIndecies = new();
        for (int i = 0; i < ActionBarManager.ActionSLots.Count; i++) {
            ActionSlot actionSlot = actionBarManager.ActionSLots[i];
            if (!actionSlot.IsOccupied) {
                availableIndecies.Add(i);
            }
        }

        foreach (var enemyUnit in enemyUnits) {
            int randomNum = Random.Range(0, 2); 
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

            if (enemyUnit.Character.CharacterData.CharacerType == CharacerType.Boss) {
                int numberOfEnemies = 0;
                foreach (var positon in encounterPositions) {
                    if (positon.Occupied) {
                        numberOfEnemies++;
                    }
                }

                if (numberOfEnemies < 2) {
                    int randomListIndex = Random.Range(0, availableIndecies.Count);
                    int slotIndex = availableIndecies[randomListIndex];
                    BattleAction action = new(ActionType.Ability, enemyUnit, enemyUnit, null, enemyUnit.Character.CharacterData.Abilities[0]);
                    ActionSlot actionSlot = actionBarManager.ActionSLots[slotIndex];
                    actionSlot.CharacterPortrait.GetComponent<Image>().sprite = enemyUnit.Character.CharacterData.CharacterPortrait;
                    actionSlot.CharacterPortrait.SetActive(true);
                    actionSlot.BattleAction = action;
                    actionSlot.IsOccupied = true;

                    availableIndecies.RemoveAt(randomListIndex);
                    continue;
                }

            }
            
            if (availableIndecies.Count > 0) {
                int randomListIndex = Random.Range(0, availableIndecies.Count);
                int slotIndex = availableIndecies[randomListIndex];

                int playerCharacterIndex= Random.Range(0, validTargets.Count);

                BattleUnit playerUnit = validTargets[playerCharacterIndex];

                BattleAction action = new(ActionType.Attack, enemyUnit, playerUnit, null, null);

                ActionSlot actionSlot = actionBarManager.ActionSLots[slotIndex];

                actionSlot.CharacterPortrait.GetComponent<Image>().sprite = enemyUnit.Character.CharacterData.CharacterPortrait;
                actionSlot.CharacterPortrait.SetActive(true);
                actionSlot.BattleAction = action;
                actionSlot.IsOccupied = true;

                availableIndecies.RemoveAt(randomListIndex);
            } else {
                Debug.Log("WHERE BE THE ACITON SLOTS BRUV");
                break;
            }
        }
        canRunRound = true;
    }

    // helper function for handling target selection in TargetSelectionState class
    public void HandleTargetSelection() {
        if (currentAction.ItemSlot?.Item.ItemTarget == ItemTarget.Enemy || currentAction.Type == ActionType.Attack || (currentAction.AbilityBase != null && currentAction.AbilityBase.AbilityTarget == AbilityTarget.Enemy)) {
            isSelectingEnemy = true;
            currentTargetIndex = 0;
        } else if (currentAction.ItemSlot?.Item.ItemTarget == ItemTarget.Player || (currentAction.AbilityBase != null && currentAction.AbilityBase.AbilityTarget == AbilityTarget.Enemy)) {
            isSelectingEnemy = false;
            currentTargetIndex = 0;
        }
        UpdateTargetIndicator();
    }

    IEnumerator BattleOver(bool won) {
        StateManager.ChangeState(new BattleOverState(this));
        
        if (won) {
            foreach(var unit in playerUnits) {
                if (unit.Character.IsAlive) {
                    yield return GiveEXP(unit.Character, 100); 
                }
            }
        }
        
        foreach(var playerUnit in playerUnits) {
            Destroy(playerUnit.CurrentModelInstance);
        }

        BattleManager.Instance.EndBattle();
    }

    IEnumerator GiveEXP(Character character, int expAmount) {
        character.EXP += expAmount;     
        yield return dialogBox.TypeDialog($"{character.CharacterData.name} has gained {expAmount} EXP!");

        while (character.CheckForLevelUp()) {
            yield return dialogBox.TypeDialog($"{character.CharacterData.name} has leveled up to {character.Level}");
        }

        yield return new WaitForSeconds(1);
    }

    // Callback for player input (B key or B on C Xbox)
    public void OnBackSelected(InputAction.CallbackContext context) {
        if (!context.started) return;

        if (StateManager.CurrentState.State != BattleState.ActionSelection) {
            StateManager.Back();
        } else {
            if (actionPoints <= 0) {
                Debug.Log("No action points available!");
                return;
            }

            MusicManager.Instance.PlaySound("MenuConfirm");
            currentAction.Type = ActionType.Run;
            StateManager.ChangeState(new CharacterSelectionState(this));
        }
    }

    // Button OnClick for Character Select State

    // Button OnClick for Action Selection State

    // Player input call back for navigating Target selection
    public void OnTargetNavigate(InputAction.CallbackContext context) {
        if (StateManager.CurrentState.State != BattleState.TargetSelection) return;

        Vector2 input = context.ReadValue<Vector2>();

        if (canNavigate && (Mathf.Abs(input.x) > 0.5f || Mathf.Abs(input.y) > 0.5f)) {
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

        // if (lastSelectedTarget != null && lastSelectedTarget != currentTarget) {
        //     lastSelectedTarget.GetComponent<MeshRenderer>().materials[^1].SetFloat("_OutlineThickness", 0f);
        // }
        // currentTarget.GetComponent<MeshRenderer>().materials[^1].SetFloat("_OutlineThickness", 0.04f);

        pointerManager.TargetSingle(currentTarget.transform);

        lastSelectedTarget = currentTarget;
    }

    public void ClearTargetIndicator() {
        // if (lastSelectedTarget) {
        //     lastSelectedTarget.GetComponent<MeshRenderer>().materials[^1].SetFloat("_OutlineThickness", 0f);
        // }
        pointerManager.ClearPointers();
    }

    // Player input callback for selecting a target
    public void OnTargetSelected(InputAction.CallbackContext  context) {
        if (context.started && StateManager.CurrentState.State == BattleState.TargetSelection) {
            List<BattleUnit> currentList = isSelectingEnemy ? enemyUnits : playerUnits;

            if (currentList.Count == 0) return;
            if (currentAction.Type == ActionType.Attack && !isSelectingEnemy) return;

            MusicManager.Instance.PlaySound("MenuConfirm");

            currentAction.Target = currentList[currentTargetIndex]; 

            StateManager.ChangeState(new ActionSlotSelectionState(this));
        }
    }

    // adds player action to action slot
    private void AddToActionSlot(ActionSlot actionSlot) {
        if (actionSlot.IsOccupied) {
            Debug.Log("Choose another slot, this one is OCCUPIED");
            return;
        }

        MusicManager.Instance.PlaySound("MenuConfirm");

        actionSlot.CharacterPortrait.GetComponent<Image>().sprite = currentSelectedPlayerUnit.Character.CharacterData.CharacterPortrait;
        actionSlot.CharacterPortrait.SetActive(true);
        actionSlot.IsOccupied = true;
        actionSlot.BattleAction = new BattleAction(currentAction.Type, currentAction.User, currentAction.Target, currentAction.ItemSlot, currentAction.AbilityBase);

        canRunRound = true;
        actionPoints--;
        actionPointText.text = $"{actionPoints}";
        
        StateManager.ChangeState(new ActionSelectionState(this));
    }

    // Removes player from action slot
    public void RemoveAction(ActionSlot actionSlot) {
        if (!actionSlot.IsOccupied) {
            Debug.Log("Action slot has no action!");
            return;
        }

        if (actionSlot.BattleAction.User.Character.CharacterData.CharacerType == CharacerType.Enemy) {
            Debug.Log("Unable to remove Enemy Slot");
            return;
        }

        actionSlot.ResetData();
        actionPoints++;
        actionPointText.text = $"{actionPoints}";

        StateManager.ChangeState(new ActionSelectionState(this));
    }


    // Player input callback for running a round
    public void OnRoundRunSelected(InputAction.CallbackContext context) {
        if (context.started) {
            if (!canRunRound && StateManager.CurrentState.State == BattleState.CharacterSelect) {
                Debug.Log("no actions in the action bar");
                return;
            }

            if (StateManager.CurrentState.State != BattleState.ActionSelection) {
                Debug.Log("Currently cannot activate a round");
                return;
            }

            MusicManager.Instance.PlaySound("MenuConfirm");
            EventSystem.current.SetSelectedGameObject(null);
            StateManager.ChangeState(new RunRoundState(this));
        }
    }

    // handler for activating in the RunRoundState class
    public void HandleRunRound() {
        StartCoroutine(RunRound());
    }

    // Coroutine to run the actions of the characters
    IEnumerator RunRound() {
        foreach (var slot in actionBarManager.ActionSLots) {
            if (!slot.IsOccupied) {
                continue;
            }

            if (!slot.BattleAction.User.Character.IsAlive) {
                continue;
            }

            switch (slot.BattleAction.Type) {
                case ActionType.Attack:
                    yield return StartCoroutine(RunAttack(slot));
                    break;
                case ActionType.Run:
                    yield return StartCoroutine(TryToEscape());
                    break;
                case ActionType.Item:
                    yield return StartCoroutine(RunItem(slot));
                    break;
                case ActionType.Ability:
                    yield return StartCoroutine(RunAbility(slot));
                    break;
            }
        }

        if (StateManager.CurrentState.State != BattleState.BattleOver) {
            CleanUp();
            StateManager.ChangeState(new ActionSelectionState(this));
        }

        yield return null;
    }

    IEnumerator RunAttack(ActionSlot actionSlot) {
        BattleUnit user = actionSlot.BattleAction.User;
        BattleUnit target = actionSlot.BattleAction.Target;

        if (!target.Character.IsAlive) {
            // foreach (var unit in enemyUnits) {
            //     if (unit.Character.IsAlive) {
            //         target = unit;
            //     }
            // }
            Debug.Log("Missed Attack");
            yield return new WaitForEndOfFrame();
        }

        int totalDamage = CalculateAttackDamage(user.Character, target.Character); 
        target.Character.DecreaseHP(totalDamage);

        GameObject damageTextObject = target.CurrentModelInstance.transform.GetChild(0).gameObject;
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
            StateManager.ChangeState(new BattleOverState(this));
        }

        yield return new WaitForEndOfFrame();
    }

    // ability -> calculate damage, check for status effects, check if you can cast
    IEnumerator RunAbility(ActionSlot actionSlot){
        AbilityBase currentAbility = actionSlot.BattleAction.AbilityBase;
        BattleUnit target = actionSlot.BattleAction.Target; 

        if (actionSlot.BattleAction.AbilityBase.AbilityTarget == AbilityTarget.Battle) {
            yield return StartCoroutine(abilityExecutor.ExecuteAbility(currentAbility, actionSlot.BattleAction.User, actionSlot.BattleAction.Target, this));
        } else if (target.Character.IsAlive) {
            yield return StartCoroutine(abilityExecutor.ExecuteAbility(currentAbility, actionSlot.BattleAction.User, actionSlot.BattleAction.Target, this));
        } else {
            Debug.Log("Missed");
        }

        if (actionSlot.BattleAction.Target != null) {
            if (actionSlot.BattleAction.Target.Character.HP <= 0) {
                yield return StartCoroutine(OnCharacterDeath(actionSlot));
            }
        }

        yield return new WaitForEndOfFrame();
    }

    IEnumerator RunItem(ActionSlot actionSlot) {
        BattleUnit target = actionSlot.BattleAction.Target;
        BattleUnit user = actionSlot.BattleAction.User;
        CombatItemData currentItem = (CombatItemData)actionSlot.BattleAction.ItemSlot.Item;

        BattleUnit newTarget = null;

        if (!target.Character.IsAlive && currentItem.ItemTarget == ItemTarget.Player) {
            newTarget = user;
        } else if (!target.Character.IsAlive && currentItem.ItemTarget == ItemTarget.Enemy) {
            foreach(var enemy in enemyUnits) {
                newTarget = enemy;
            }
        }else{
            newTarget = target;
        }

        GameObject damageTextObject = newTarget.CurrentModelInstance.transform.GetChild(0).gameObject;
        yield return StartCoroutine(UseItem(currentItem, user.Character, newTarget.Character, damageTextObject));

    }

    public IEnumerator UseItem(CombatItemData item, Character user, Character target, GameObject damageTextObject) {
        Character newTarget = target;

        foreach (ItemEffectBase effect in item.effects) {
            EffectInfo effectInfo = effect.ApplyEffectToCharacter(user, newTarget);
            damageTextObject.SetActive(true);
            damageTextObject.GetComponent<DamageText>().text.text = $"{effectInfo.TextInformation}";
            damageTextObject.GetComponent<DamageText>().text.color = effectInfo.TextColor;

            yield return new WaitForSeconds(1f);

            damageTextObject.SetActive(false);
        }

        damageTextObject.GetComponent<DamageText>().text.color = Color.white;

        playerInventory.RemoveItem(item);

        yield return new WaitForEndOfFrame();
    }

    IEnumerator OnCharacterDeath(ActionSlot actionSlot) {
        // get XP;
        // death animation
        // remove model from instanceList or mark as dead and skip over;
        BattleUnit dyingUnit = actionSlot.BattleAction.Target;
        if (dyingUnit.Character.CharacterData.CharacerType == CharacerType.Enemy) {
            // dyingUnit.CurrentModelInstance.GetComponent<MeshRenderer>().materials[0].color = Color.red;

            dyingUnit.BattlePosition.Occupied = false;
            Destroy(dyingUnit.CurrentModelInstance);

            foreach(var enemyUnit in enemyUnits) {
                if(enemyUnit == dyingUnit) {
                    enemyUnits.Remove(enemyUnit);
                    break;
                }
            }
        } else {
            // dyingUnit.CurrentModelInstance.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().materials[0].color = Color.red;
            foreach(var playerUnit in playerUnits) {
                if(playerUnit == dyingUnit) {
                    // playerUnits.Remove(playerUnit);
                    playerUnit.Character.IsAlive = false;
                    break;
                }
            }
        }

        if (enemyUnits.Count <= 0) {
            yield return BattleOver(true);
        } else if (CheckIfPartyDead()){
            yield return BattleOver(false);
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
        Debug.Log(numDead);

        return numDead == 3;
    }

    void CleanUp() {
        foreach (var slot in actionBarManager.ActionSLots) {
            if (!slot.IsOccupied) {
                continue;
            }

            slot.ResetData();
        }

        canRunRound = false;

        actionPoints = 3;
        actionPointText.text = $"{actionPoints}";

        hasRoundPassed = true;
        numEscapeAttempts = 0;

        currentSelectedPlayerUnit = null;

        if (hasRoundPassed) {
            LoadEnemyActionSlots();
            hasRoundPassed = false;
        }

        StateManager.CleanStates();
    }

    // Event Handlers
    private void HandleSlotActionSelected(SlotAction slotAction) {
        StateManager.ChangeState(new ActionSlotSelectionState(this, slotAction));
    }

    private void HandleActionSlotSelected(ActionSlot actionSlot, SlotAction slotAction, bool swapped) {
        if (slotAction == SlotAction.Add) {
            AddToActionSlot(actionSlot);
        } else if (slotAction == SlotAction.Remove) {
            RemoveAction(actionSlot);
        } else if (slotAction == SlotAction.Swap) {
            if (!swapped) {
                StateManager.ChangeState(new SlotSwapState(this));
            } else if (swapped) {
                StateManager.ChangeState(new ActionSelectionState(this));
            }
        } 
    }


    void HandleAbilitySelection(AbilityBase selectedAbility){
        currentAction.AbilityBase = selectedAbility;
        StateManager.ChangeState(new TargetSelectionState(this));
    }

    void HandleItemSelection(ItemSlot selectedSlot) {
        Debug.Log("handle them items");
        currentAction.ItemSlot = selectedSlot;
        EventSystem.current.SetSelectedGameObject(null);
        itemPanel.SetActive(false);

        playerInventory.RemoveItem(selectedSlot.Item);

        StateManager.ChangeState(new TargetSelectionState(this));
    }

    public void OnAttackSelected(InputAction.CallbackContext context) {
        if (!context.started) return;
        if (StateManager.CurrentState.State != BattleState.ActionSelection) return;
        if (actionPoints <= 0) {
            Debug.Log("No action points available!");
            return;
        }

        MusicManager.Instance.PlaySound("MenuConfirm");
        currentAction.Type = ActionType.Attack;
        StateManager.ChangeState(new CharacterSelectionState(this));
    }

    public void OnAbilitiesSelected(InputAction.CallbackContext context) {
        if (!context.started) return;
        if (StateManager.CurrentState.State != BattleState.ActionSelection) return;
        if (actionPoints <= 0) {
            Debug.Log("No action points available!");
            return;
        }

        MusicManager.Instance.PlaySound("MenuConfirm");
        currentAction.Type = ActionType.Ability;
        StateManager.ChangeState(new CharacterSelectionState(this));
    }

    public void OnSlotActionsSelected(InputAction.CallbackContext context) {
        if (!context.started) return;
        if (StateManager.CurrentState.State != BattleState.ActionSelection) return;

        MusicManager.Instance.PlaySound("MenuConfirm");
        StateManager.ChangeState(new SlotActionSelectionState(this));
    }

    public void OnEscapeSelected(InputAction.CallbackContext context) {
        if (!context.started) return;
        return;
    }

    public void OnItemsSelected(InputAction.CallbackContext context) {
        if (!context.started) return;
        if (StateManager.CurrentState.State != BattleState.ActionSelection) return;
        if (actionPoints <= 0) {
            Debug.Log("No action points available!");
            return;
        }

        MusicManager.Instance.PlaySound("MenuConfirm");
        currentAction.Type = ActionType.Item;
        StateManager.ChangeState(new CharacterSelectionState(this));
    }

    public void OnCharacterSelect(int playerCharacterIndex) {
        if (StateManager.CurrentState.State != BattleState.CharacterSelect) {
            Debug.Log("Not in the character select state brother");
            return;
        }

        currentSelectedPlayerUnit = playerUnits[playerCharacterIndex];

        if (!currentSelectedPlayerUnit.Character.IsAlive) {
            Debug.Log("Charcter is not alife!");
            currentSelectedPlayerUnit = null;
            return;
        }

        currentAction.User = currentSelectedPlayerUnit;

        MusicManager.Instance.PlaySound("MenuConfirm");

        switch (currentAction.Type) {
            case ActionType.Attack:
                StateManager.ChangeState(new TargetSelectionState(this));
                break; 
            case ActionType.Run:
                StateManager.ChangeState(new ActionSlotSelectionState(this));
                break;
            case ActionType.Item:
                StateManager.ChangeState(new ItemSelectionState(this));
                break;
            case ActionType.Ability:
                StateManager.ChangeState(new AbilitySelectionState(this));
                break;
        }
    }    

    public void DelayInput() {
        StartCoroutine(Delay());
    }

    IEnumerator Delay() {
        yield return new WaitForEndOfFrame();
    }

    public bool TrySummonEnemy(Character enemy) {
        for (int i = 0; i < EncounterPositions.Count; i++) {
            BattlePosition position = EncounterPositions[i];
            if (!position.Occupied) {
                BattleUnit unit = new BattleUnit(enemy, position);
                unit.Setup();
                unit.CurrentModelInstance = Instantiate(enemy.CharacterData.CharacterPrefab, position.transform);
                EnemyUnits.Add(unit);
                return true;
            }
        }
        return false;
    }
}