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
    public ItemSlot ItemSlot;
    public AbilityBase abilityBase;
}

public class BattleSystem : MonoBehaviour {
    [Header("Battle Setup")]
    [SerializeField] private List<GameObject> partyPositions;
    [SerializeField] private List<GameObject> encounterPositions;
    [SerializeField] private int actionPoints;
    [SerializeField] private ItemUser itemUser;

    [Header("Battle UI")]
    [SerializeField] List<Button> playerPortraits; 
    [SerializeField] List<Button> actionSlots; 
    [SerializeField] List<CharacterHud> characterHudList;
    [SerializeField] Material partyOutline;
    [SerializeField] Material enemyOutline;
    [SerializeField] TextMeshProUGUI actionPointText;
    [SerializeField] PointerManager pointerManager;
    [SerializeField] ItemMenu itemMenu;
    [SerializeField] GameObject ItemPanel;
    [SerializeField] GameObject abilityPanel;
    [SerializeField] AbilityMenu abilityMenu;

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

    private Inventory playerInventory;

    private bool hasRoundPassed = false;

    private bool canNavigate = true;
    private bool canRunRound = false;

    private int numEscapeAttempts;

    void Awake() {
        StartBattle(); 
    }

    void OnEnable() {
        Debug.Log(itemMenu);
        if (itemMenu != null) {
            itemMenu.OnItemSelected += HandleItemSelection;
        } 
        if (abilityMenu != null) {
            abilityMenu.OnAbilitySelected += HandleAbilitySelection;
        } 
    }

    void OnDisable() {
        if (itemMenu != null) {
            itemMenu.OnItemSelected -= HandleItemSelection;
        } 
        if (abilityMenu != null) {
            abilityMenu.OnAbilitySelected -= HandleAbilitySelection;
        }
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

        playerInventory = BattleManager.Instance.PlayerInventory;

        actionPoints = 3;
        actionPointText.text = $"{actionPoints}";

        numEscapeAttempts = 0;
    }

    public IEnumerator SetupBattle() {
        yield return new WaitForEndOfFrame(); 
        MusicManager.Instance.PlayMusic("BattleTheme", 0.25f);
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
            MusicManager.Instance.PlaySound("MenuBack");
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

        backAction = () => {
            currentSelectedPlayerUnit.Hud.ActionPanel.SetActive(false);
            ChangeState(() => CharacterSelection());
        };

        currentSelectedPlayerUnit.Hud.ActionPanel.SetActive(true);
        currentSelectedPlayerUnit.Hud.ActionPanel.transform.GetChild(1).gameObject.GetComponent<Button>().Select();
    }

    void AbilitySeletion(){
        prevState = state;
        state = BattleState.AbilitySelection;

        if(prevState == BattleState.ActionSelection){
            backAction = () => {
                currentAction.Type = ActionType.None;
                currentAction.User = null;
                abilityPanel.SetActive(false);
                ChangeState(() => ActionSelection());
            };
        }else if(prevState == BattleState.TargetSelection){
           backAction = () => {
                currentAction.Type = ActionType.None; 
                currentAction.User = null;
                abilityPanel.SetActive(false);
                ChangeState(() => ActionSelection());
            }; 
        }
        abilityMenu.PopulateAbilities(currentSelectedPlayerUnit.Character.Abilities);
        abilityPanel.SetActive(true);
    }
    void HandleAbilitySelection(AbilityBase selectedAbility){
        currentAction.abilityBase = selectedAbility;
        EventSystem.current.SetSelectedGameObject(null);
        abilityPanel.SetActive(false);
        ChangeState(() => TargetSelection());
    }

    void ItemSelection() {
        prevState = state;
        state = BattleState.ItemSelection;

        if (prevState == BattleState.ActionSelection) {
            backAction = () => {
                currentAction.Type = ActionType.None; 
                currentAction.User = null;
                ItemPanel.SetActive(false);
                ChangeState(() => ActionSelection());
            };
        } else if (prevState == BattleState.TargetSelection) {
            backAction = () => {
                currentAction.Type = ActionType.None; 
                currentAction.User = null;
                ItemPanel.SetActive(false);
                ChangeState(() => ActionSelection());
            };
        }

        itemMenu.PopulateInventory(playerInventory);
        ItemPanel.SetActive(true);
    }

    void HandleItemSelection(ItemSlot selectedSlot) {
        Debug.Log("handle them items");
        currentAction.ItemSlot = selectedSlot;
        EventSystem.current.SetSelectedGameObject(null);
        ItemPanel.SetActive(false);

        playerInventory.RemoveItem(selectedSlot.Item);

        ChangeState(() => TargetSelection());
    }

    void TargetSelection() {
        Debug.Log("we made it to target selection");
        prevState = state;
        state = BattleState.TargetSelection;
        currentAction.Target = null;

        switch (prevState) {
            case BattleState.ActionSelection:
                backAction = () => {
                    currentAction.Type = ActionType.None; 
                    currentAction.User = null;
                    ClearTargetIndicator();
                    ChangeState(() => ActionSelection());
                };
                break;
            case BattleState.ActionSlotSelection:
                switch (currentAction.Type) {
                    case ActionType.Ability:
                    backAction = () => {
                        pointerManager.ClearPointers();
                        ClearTargetIndicator();
                        ChangeState(() => AbilitySeletion());
                    };
                    break;
                    case ActionType.Item:
                        backAction = () => {
                            pointerManager.ClearPointers();
                            ClearTargetIndicator();
                            currentAction.ItemSlot.Item = null;
                            currentAction.ItemSlot.Count = 0;
                            ChangeState(() => ItemSelection());
                        };
                        break;
                    default:
                        backAction = () => {
                            currentAction.Type = ActionType.None; 
                            currentAction.User = null;
                            pointerManager.ClearPointers();
                            ClearTargetIndicator();
                            ChangeState(() => ActionSelection());
                        };
                        break;
                } 
                break;
            case BattleState.AbilitySelection:
                backAction = () => {
                    pointerManager.ClearPointers();
                    ClearTargetIndicator();
                    ChangeState(() => AbilitySeletion());
                };
                break;
            case BattleState.ItemSelection:
                backAction = () => {
                    pointerManager.ClearPointers();
                    ClearTargetIndicator();
                    playerInventory.AddItem(currentAction.ItemSlot.Item);
                    currentAction.ItemSlot.Item = null;
                    currentAction.ItemSlot.Count = 0;
                    ChangeState(() => ItemSelection());
                };
                break;
        }

        if (currentAction.ItemSlot.Item?.ItemTarget == ItemTarget.Enemy || currentAction.Type == ActionType.Attack || currentAction.abilityBase?.AbilityTarget == AbilityTarget.Enemy) {
            isSelectingEnemy = true;
            currentTargetIndex = 0;
        } else if (currentAction.ItemSlot.Item?.ItemTarget == ItemTarget.Player || currentAction.abilityBase?.AbilityTarget == AbilityTarget.Player) {
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
                    pointerManager.ClearPointers();
                    ChangeState(() => ActionSelection());
                };
                break;
            case BattleState.TargetSelection:
                backAction = () => {
                    EventSystem.current.SetSelectedGameObject(null);
                    pointerManager.ClearPointers();
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
        EventSystem.current.SetSelectedGameObject(null);
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
            case "item":
                battleAction.Type = ActionType.Item;
                battleAction.User = currentSelectedPlayerUnit;
                currentAction = battleAction;
                ChangeState(() => ItemSelection());
                break;
            case "ability":
                battleAction.Type = ActionType.Ability;
                battleAction.User = currentSelectedPlayerUnit;
                currentAction = battleAction;
                ChangeState(() => AbilitySeletion());
                break;
        }
        currentSelectedPlayerUnit.Hud.ActionPanel.SetActive(false);
    }

    public void OnTargetNavigate(InputAction.CallbackContext context) {
        if (state != BattleState.TargetSelection) return;

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

    void ClearTargetIndicator() {
        // if (lastSelectedTarget) {
        //     lastSelectedTarget.GetComponent<MeshRenderer>().materials[^1].SetFloat("_OutlineThickness", 0f);
        // }
        pointerManager.ClearPointers();
    }

    public void OnTargetSelected(InputAction.CallbackContext  context) {
        if (context.started && state == BattleState.TargetSelection) {
            ClearTargetIndicator();
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
            EventSystem.current.SetSelectedGameObject(null);
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

            switch (actionSlot.BattleAction.Type) {
                case ActionType.Attack:
                    yield return StartCoroutine(RunAttack(actionSlot));
                    break;
                case ActionType.Run:
                    yield return StartCoroutine(TryToEscape());
                    break;
                case ActionType.Item:
                    yield return StartCoroutine(RunItem(actionSlot));
                    break;
                case ActionType.Ability:
                    yield return StartCoroutine(RunAbility(actionSlot));
                    break;
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

        if (!target.Character.IsAlive) {
            foreach (var unit in enemyUnits) {
                if (unit.Character.IsAlive) {
                    target = unit;
                }
            }
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
            BattleOver(true);
        }

        yield return null;
    }
    // ability -> calculate damage, check for status effects, check if you can cast
    IEnumerator RunAbility(ActionSlot actionSlot){
        BattleUnit target = actionSlot.BattleAction.Target;
        BattleUnit user = actionSlot.BattleAction.User;
        AbilityBase currentAbility = actionSlot.BattleAction.abilityBase;

        BattleUnit newTarget = null;

        if (!target.Character.IsAlive && currentAbility.AbilityTarget == AbilityTarget.Player) {
            newTarget = user;
        } else if (!target.Character.IsAlive && currentAbility.AbilityTarget == AbilityTarget.Enemy) {
            foreach(var enemy in enemyUnits) {
                newTarget = enemy;
            }
        }else{
            newTarget = target;
        }

        GameObject damageTextObject = newTarget.CurrentModelInstance.transform.GetChild(0).gameObject;
        yield return StartCoroutine(UseAbility(currentAbility,user.Character,newTarget.Character,damageTextObject));
    }

    public IEnumerator UseAbility(AbilityBase ability, Character user, Character target, GameObject damageTextObject){
        Character newTarget = target;

        foreach (AbilityEffectBase effect in ability.Effects){
            AbilityEffectInfo effectInfo = effect.ApplyEffect(user,target);
            damageTextObject.SetActive(true);
            damageTextObject.GetComponent<DamageText>().text.text = $"{effectInfo.TextInformation}";
            damageTextObject.GetComponent<DamageText>().text.color = effectInfo.TextColor;

            yield return new WaitForSeconds(1f);

            damageTextObject.SetActive(false);
        }
        damageTextObject.GetComponent<DamageText>().text.color = Color.white;
        yield return new WaitForEndOfFrame();

    }
    // items -> calculate value, check for any status effects

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
            EffectInfo effectInfo = effect.ApplyEffect(user, newTarget);
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