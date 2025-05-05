using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.UI;

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
    public float AttackMod {get; set;} = 0;
    public float DefenseMod {get; set;} = 0;

    

    public bool LeveledUp { get; set; } = false;
    private List<(Condition, int duration)> buffs = new();
    private List<(Condition, int duration)> debuffs = new();

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
        if(buffs == null) buffs = new List<(Condition, int duration)>();
        if(debuffs == null) debuffs = new List<(Condition, int duration)>();
    }

    public int CalculateBasicAttackDamage(Character target) {
        float primaryStat = PrimaryStat == Stat.Strength ? Stats.Strength : Stats.Magic;
        float levelMod = Mathf.Max((Level - target.Level) * 1.1f, 1f);
        float targetDefense = target.Stats.Defense;
        float weaponPower = CharacterData.WeaponPower;
        float baseDamage = Mathf.Max(Mathf.Sqrt(primaryStat/targetDefense * weaponPower), 1f);
        float randMod = UnityEngine.Random.Range(0.95f,1.05f);
        float damageMod = 1 + AttackMod - target.DefenseMod;

        // attaack dmg mod and def reduction mod

        return (int)Mathf.Clamp(5 * baseDamage * levelMod * randMod * damageMod, 0, 10000); 
    }

    public int CalculateAbilityPower(int power, Character target){
        float primaryStat = PrimaryStat == Stat.Strength ? Stats.Strength : Stats.Magic;
        float levelMod = Mathf.Max((Level - target.Level) * 1.1f, 1f);
        float targetDefense = target.Stats.Defense;
        float baseDamage = Mathf.Max(Mathf.Sqrt(primaryStat/targetDefense * power), 1f);
        float randMod = UnityEngine.Random.Range(0.95f,1.05f);
        float damageMod = 1 + AttackMod - target.DefenseMod;

        // attaack dmg mod and def reduction mod

        return (int)Mathf.Clamp(5 * baseDamage * levelMod * randMod * damageMod , 0, 10000); 
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

    public List<(Condition, int duration)> Buffs{
        get{
            return buffs;
        }
    }
    public List<(Condition, int duration)> Debuffs{
        get{
            return debuffs;
        }
    }
    public void AddCondition(Condition newCondition, Character target){
        if(newCondition.Type == ConditionType.Buff){
            if(buffs?.Count > 0){
                for(int i = 0; i < buffs.Count; i ++){
                    var condition = buffs[i];
                    if(newCondition.ConditionName.Equals(condition.Item1.ConditionName)){
                        condition.duration = newCondition.Duration;
                    }else{
                        buffs.Add((newCondition, newCondition.Duration));
                        newCondition.ApplyToCharacter(target);
                    }
                }   
            }else{
                buffs?.Add((newCondition, newCondition.Duration));
                newCondition.ApplyToCharacter(target);
            }
        }
            
        if(newCondition.Type == ConditionType.Debuff){
            if(debuffs?.Count > 0){
                for(int i = 0; i < debuffs.Count; i ++){
                    var condition = debuffs[i];
                    if(newCondition.ConditionName.Equals(condition.Item1.ConditionName)){
                        condition.duration = newCondition.Duration;
                    }else{
                        debuffs.Add((newCondition, newCondition.Duration));
                        newCondition.ApplyToCharacter(target);
                    }
                }   
            }else{
                debuffs.Add((newCondition, newCondition.Duration));
                newCondition.ApplyToCharacter(target);
            }
        }
    }

    public void EndOfRound( Character target){
        for( int i = 0; i< buffs.Count; i ++){
            var condition = buffs[i];
            condition.duration --;
            if(condition.duration <= 0){
                Debug.Log("ooooo");
                condition.Item1.RemoveFromCharacter(target);
                buffs.RemoveAt(i);
                Debug.Log(buffs.Count + "eee");
            }
        }
        for( int i = 0; i< debuffs.Count; i ++){
            var condition = debuffs[i];
            condition.duration --;
            if(condition.duration <= 0){
                condition.Item1.RemoveFromCharacter(target);
                debuffs.Remove(condition);
            }
        }
        
    }
    public Condition CheckHasCondition(Condition newCondition){
        for(int i = 0; i < buffs.Count; i ++){
            var condition = buffs[i];
           if(newCondition.ConditionName.Equals(condition.Item1.ConditionName)){
               return newCondition;
            }
        }
        for(int i = 0; i < debuffs.Count; i ++){
            var condition = debuffs[i];
           if(newCondition.ConditionName.Equals(condition.Item1.ConditionName)){
               return newCondition;
            }
        }
        return null;
    }
}
