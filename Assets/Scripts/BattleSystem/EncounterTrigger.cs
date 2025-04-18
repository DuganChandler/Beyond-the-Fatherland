using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterTrigger : MonoBehaviour {
    [SerializeField] private List<EncounterList> encounterLists;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (GameManager.Instance.GameState != GameState.FreeRoam) {
                Debug.Log("Need to be in Free Roam to enter Battle");
                return;
            }

            Debug.Log("Collided With Player");
            BattleManager.Instance.EncounterPartyList = encounterLists[0].CharacterList;
            BattleManager.Instance.PlayerPartyList = other.GetComponent<PartyList>().CharacterList;
            BattleManager.Instance.PlayerInventory = other.GetComponent<Inventory>();

            BattleManager.Instance.BattleType = BattleType.Boss;

            StartCoroutine(BattleManager.Instance.StartBattle());
        } 
    }
}
