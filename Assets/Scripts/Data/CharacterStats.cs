using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStat{
    public int Strength;
    public int Magic;
    public int Defense;
}

[CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObjects/Characters/CharacterStatsSO", order = 2)]
public class CharacterStats : ScriptableObject {
    public CharacterStat[] levelStats;
    [SerializeField] int mpGrowth; 
    [SerializeField] int hpGrowth;
}
