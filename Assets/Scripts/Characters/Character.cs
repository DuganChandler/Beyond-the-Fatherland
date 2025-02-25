using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character  {
    [Header("Character SO")]
    [SerializeField] CharacterData characterData;

    [Header("Character Level")]
    [SerializeField] int level;

    public Character(CharacterData character, int cLevel) {
        characterData = character;
        level = cLevel;

        Init();
    }

    public int EXP { get; set; }
    public int HP { get; set; }
    public int MP { get; set; }
    public Stats Stats { get; set; }
    public CharacterData CharacterData {
        get {
            return characterData;
        }
    }
    // need abilities list here

    public void Init() {
        // Set exp to level from characterSO
        Stats = characterData.GetStatsAtLevel(level);

        // This does not take into account if the party memeber is damaged
        HP = characterData.GetHpAtLevel(level);
        MP = characterData.GetMpAtLevel(level);
    }
}
