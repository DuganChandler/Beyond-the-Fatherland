using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState {
    Start,
    CharacterSelect,
    ActionSelecton,
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
    [SerializeField] List<Button> actionSlots; 
    [SerializeField] List<CharacterHud> characterHudList;

    private List<Character> playerParty;
    private List<Character> encounterParty; 

    private BattleState state;
    private BattleState prevState;

    private void HandleUpdate() {
        switch (state) {
            case BattleState.Start:
                Debug.Log("Start");
                break;
            case BattleState.CharacterSelect:
                Debug.Log("CharacterSelect");
                break;
            case BattleState.ActionSelecton:
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

            Instantiate(encounterCharacterPrefab, encounterCharacterPosition);
            encounterParty[i].Init();
        }



        yield return null;
    }

    void Update() {
        if (GameManager.Instance.GameState == GameState.Battle) {
            HandleUpdate();
        } 

        if (Input.GetKeyDown(KeyCode.K)) {
            playerParty[1].DecreaseHP(20);
        }
    }
}
