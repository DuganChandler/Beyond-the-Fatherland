using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

public class BattleSystem : MonoBehaviour {
    [Header("Battle Setup")]
    [SerializeField] private List<GameObject> partyPositions;
    [SerializeField] private List<GameObject> encounterPositions;

    [Header("Battle UI")]
    [SerializeField] List<Button> playerPortraits; 
    // make an action slot script attached to each slot
    // create an action script or struct, to hold action information
    [SerializeField] List<Button> actionSlots; 
    [SerializeField] List<CharacterHud> characterHudList;

    private List<Character> playerParty;
    private List<Character> encounterParty; 
    private List<GameObject> encounterIntances;

    private BattleState state;
    private BattleState prevState;


    void OnEnable() {
        StartBattle(); 
    }

    public void StartBattle() {
        // initalize party and enemies stats
        playerParty = BattleManager.Instance.PlayerPartyList; 
        encounterParty = BattleManager.Instance.EncounterPartyList;
        Debug.Log(playerParty);
        Debug.Log(encounterParty);

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

            Instantiate(playerCharacterPrefab, playerCharacterPosition);
        }

        for (int i = 0; i < encounterParty.Count; i++) {
            GameObject encounterCharacterPrefab = encounterParty[i].CharacterData.CharacterPrefab;
            Transform encounterCharacterPosition = encounterPositions[i].transform;

            GameObject encounterInstance = Instantiate(encounterCharacterPrefab, encounterCharacterPosition);
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
        state = BattleState.CharacterSelect;
        prevState = state;
        playerPortraits[0].Select();
    }

    void OnCharacterSelect(GameObject characterHud= null) {
        switch (state) {
            case BattleState.CharacterSelect:
                ActionSelection(characterHud);                
                break;
            case BattleState.TargetSelection:
                TargetSelection();
                break;
            default:
                Debug.Log("Default, no state match");
                break;
        }
    }    

    void ActionSelection(GameObject characterHud) {
        prevState = state;
        state = BattleState.ActionSelection;
        characterHud.SetActive(true);
        characterHud.transform.GetChild(1).gameObject.GetComponent<Button>().Select();
    }

    void TargetSelection() {

    }

    void OnActionSelection(String action) {
        switch (action) {
            case "attack":
                TargetSelection();
                break; 
            case "run":

                break;
        }
    }

    void ActionBarSelection() {

    }
}
