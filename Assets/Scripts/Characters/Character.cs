using System;
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
    public int MaxHP {get; set; }
    public int MaxMP {get; set; }

    public Stat PrimaryStat { get; set; }
    public Stats Stats { get; set; }

    public int Level {
        get {
            return level;
        }
    }
    
    public CharacterData CharacterData {
        get {
            return characterData;
        }
    }
    public bool IsAlive { get; set; }
    // need abilities list here

    public event System.Action OnHpChange;
    public event System.Action OnMpChange;

    public void Init() {
        // Set exp to level from characterSO
        Stats = characterData.GetStatsAtLevel(level);
        PrimaryStat = characterData.PrimaryStat;

        // This does not take into account if the party memeber is damaged
        MaxHP = characterData.GetHpAtLevel(level);
        MaxMP = characterData.GetMpAtLevel(level);

        HP = MaxHP;
        MP = MaxMP;


        IsAlive = true;
    }

    public int CalculateBasicAttackDamage() {
        if (PrimaryStat == Stat.Strength) {
            return 10 * Stats.Strength;
        } else if (PrimaryStat == Stat.Magic) {
            return 10 * Stats.Magic;
        } 
        return 0;
    }

    public int CalculateDefense() {
        return (int)Math.Round(Stats.Defense * 4f);
    }

    public void DecreaseHP(int damage) {
        HP = Mathf.Clamp(HP - damage, 0 , MaxHP);
        if (HP <= 0) {
            IsAlive = false;
        }
        OnHpChange?.Invoke();
    }

    public void IncreaseHP(int amount) {
        HP = Mathf.Clamp(HP + amount, 0 , MaxHP);
        OnHpChange?.Invoke();
    }

    public void DecreaseMP(int amount) {
        MP = Mathf.Clamp(MP - amount, 0, MaxMP);
        OnMpChange?.Invoke(); 
    }

    public void IncraseMP(int amount) {
        MP = Mathf.Clamp(MP + amount, 0, MaxMP);
        OnMpChange?.Invoke(); 
    }

    public void CalculateStats() {
        int oldMaxHP = MaxHP;
        MaxHP = characterData.GetHpAtLevel(level);

        if (oldMaxHP != 0) {
            HP += MaxHP - oldMaxHP;
        }
    }

    public bool CheckForLevelUp() {
        if (EXP >= 90) {
            ++level;
            // calculate stats
            EXP -= 90;
            return true;
        }
        return false;
    }

    public List<AbilityBase> Abilities{
        get{
            return characterData.Abilities;
        }
    }
}
