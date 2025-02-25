using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] private bool isPlayerUnit;
    // [SerializeField] private CharacterBattleHud hud;

    [SerializeField] Transform playerBattleSpot;
    [SerializeField] Transform enemyBattleSpot;

    public bool IsPlayerUnit {
        get { return isPlayerUnit; }
    }

    // public CharacterBattleHud Hud {
    //     get { return hud; }
    // }

    public Character Character { get; set; }

    GameObject characterModel;

    public GameObject CurrentPlayerModel { get; set; }
    public GameObject CurrentEnemyModel { get; set; }

    public void Setup(Character character) {
        Character = character;
        characterModel = Character.CharacterData.CharacterPrefab;

        if (IsPlayerUnit) {
            CurrentPlayerModel = Instantiate(characterModel, playerBattleSpot);
        } else {
            CurrentEnemyModel = Instantiate(characterModel, enemyBattleSpot);

        }
        // hud.gameObject.SetActive(true);
        // set hud data
    }
}
