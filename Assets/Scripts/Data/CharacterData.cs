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

    // Make sure to check if the character type is BOSS
    public Stats GetStatsAtLevel(int level) {
        if (level < 1 || level > characterStats.levelStats.Length) {
            return characterStats.levelStats[characterStats.levelStats.Length - 1];
        }

        return characterStats.levelStats[level - 1];
    }

    public Stats GetStatsForBoss() {
        return characterStats.levelStats[0];
    }

    public int GetHpAtLevel(int level) {
        return baseHP + (characterStats.HpGrowth * level - 1);
    }

    public int GetMpAtLevel(int level) {
        return baseMP + (characterStats.MpGrowth * level - 1);
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

    // [Header("Abilities")]
    // Create List of Abilities
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
