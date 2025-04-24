using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Characters/CharacterData", order = 1)]
public class CharacterData : ScriptableObject {
    [Header("Character Name")]
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [Header("Character Model")]
    [SerializeField] GameObject characterPrefab;

    [Header("Character Portrait")]
    [SerializeField] Sprite characterPortrait;

    [Header("Character Type")]
    [SerializeField] CharacerType characerType;

    [Header("Stats")]
    [SerializeField] int currentLevel;
    [SerializeField] int baseHP;
    [SerializeField] int baseMP;
    [SerializeField] CharacterStats characterStats;
    [SerializeField] Stat primaryStat;
    [SerializeField] float weaponPower;


    [Header("Abilities")]
    [SerializeField] List<AbilityBase> abilities;

    List<Condition> conditions;

    // Make sure to check if the character type is BOSS
    public Stats GetStatsAtLevel(int level) {
        if (level < 1 || level > characterStats.LevelStats.Length) {
            return characterStats.LevelStats[^1];
        }

        Debug.Log($"Strength: {characterStats.LevelStats[level - 1].Strength}");
        return characterStats.LevelStats[level - 1];
    }

    public Stats GetStatsForBoss() {
        return characterStats.LevelStats[0];
    }

    public int GetHpAtLevel(int level) {
        return baseHP + (characterStats.HpGrowth * (level - 1));
    }

    public int GetMpAtLevel(int level) {
        return baseMP + (characterStats.MpGrowth * (level - 1));
    }

    public GameObject CharacterPrefab {
        get {
            return characterPrefab;
        }
    }

    public Sprite CharacterPortrait {
        get {
            return characterPortrait;
        }
    }

    public Stat PrimaryStat {
        get {
            return primaryStat;
        }
    }

    public CharacerType CharacerType {
        get {
            return characerType;
        }
    } 

    public List<AbilityBase> Abilities{
        get{
            return abilities;
        }
    }
    // Create List of Abilities

    public List<Condition> Conditions{
        get{
            return conditions;
        }
    }

    public float WeaponPower => weaponPower;
}

public enum CharacerType {
    PartyMember,
    Enemy,
    Boss
}

public enum Stat {
    Strength,
    Magic,
    Defense,
}
