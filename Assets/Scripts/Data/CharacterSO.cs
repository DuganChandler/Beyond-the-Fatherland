using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Characters/CharacterSO", order = 1)]
public class CharacterSO : ScriptableObject {
    [Header("Character Name")]
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [Header("Character Model")]
    [SerializeField] GameObject characterPrefab;

    [Header("Stats")]
    [SerializeField] int currentLevel;
    [SerializeField] int baseHP;
    [SerializeField] int baseMP;
    [SerializeField] CharacterStats characterStats;

    public CharacterStat GetStatsAtLevel(int level) {
        if (level < 1 || level > characterStats.levelStats.Length) {
            return characterStats.levelStats[characterStats.levelStats.Length - 1];
        }

        return characterStats.levelStats[level - 1];
    }


    // [Header("Abilities")]
    // Create List of Abilities
}

public enum Stat {
    Strength,
    Magic,
    Defense,
}
