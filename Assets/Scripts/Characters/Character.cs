using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
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
    public float strengthBuffs {get; set;}
    public float magicBuffs {get; set;}
    public float defenseBuffs {get; set;}
    public float strengthDebuffs {get; set;}
    public float magicDebuffs {get; set;}
    public float defenseDebuffs {get; set;}

    public bool LeveledUp { get; set; } = false;

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

    public int CalculateBasicAttackDamage(Character target) {
        float primaryStat = PrimaryStat == Stat.Strength ? Stats.Strength : Stats.Magic;
        float levelMod = Mathf.Max((Level - target.Level) * 1.1f, 1f);
        float targetDefense = target.Stats.Defense;
        float weaponPower = CharacterData.WeaponPower;
        float baseDamage = Mathf.Max(Mathf.Sqrt(primaryStat/targetDefense * weaponPower), 1f);
        float randMod = UnityEngine.Random.Range(0.95f,1.05f);

        // attaack dmg mod and def reduction mod

        return (int)Mathf.Clamp(5 * baseDamage * levelMod * randMod, 0, 10000); 
    }

    public int CalculateAbilityPower(int power, Character target){
        float primaryStat = PrimaryStat == Stat.Strength ? Stats.Strength : Stats.Magic;
        float levelMod = Mathf.Max((Level - target.Level) * 1.1f, 1f);
        float targetDefense = target.Stats.Defense;
        float baseDamage = Mathf.Max(Mathf.Sqrt(primaryStat/targetDefense * power), 1f);
        float randMod = UnityEngine.Random.Range(0.95f,1.05f);
        // attaack dmg mod and def reduction mod

        return (int)Mathf.Clamp(5 * baseDamage * levelMod * randMod, 0, 10000); 
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
        int oldMaxMP = MaxMP;
        Stats = characterData.GetStatsAtLevel(level);

        MaxHP = characterData.GetHpAtLevel(level);
        MaxMP = characterData.GetMpAtLevel(level);        
        //Debug.Log(MaxHP);

        if (oldMaxHP != 0) {
            HP += MaxHP - oldMaxHP;
            MP += MaxMP - oldMaxMP;
        }

        OnHpChange?.Invoke();
        OnMpChange?.Invoke(); 
    }

    public void CheckForLevelUp() {
        if (EXP >= 75) {
            ++level;
            EXP -= 75;
            LeveledUp = true;
            CalculateStats();
            return;
        }
        LeveledUp = false;
    }

    public List<AbilityBase> Abilities{
        get{
            return characterData.Abilities;
        }
    }

    public List<Condition> Conditions{
        get{
            return characterData.Conditions;
        }
    }

    /*public void CheckConditions(int round, BattleSystem battleSystem){
        foreach(Condition condition in Conditions){
            if(condition.initilaRound - )
        }
    }*/
}
