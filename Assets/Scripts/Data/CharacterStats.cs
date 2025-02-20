using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats{
    public int Strength;
    public int Magic;
    public int Defense;
}

[CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObjects/Characters/CharacterStatsData", order = 2)]
public class CharacterStats : ScriptableObject {
    public Stats[] levelStats;
    [SerializeField] int hpGrowth;
    [SerializeField] int mpGrowth; 

    public int HpGrowth { get; }
    public int MpGrowth { get; }
}
