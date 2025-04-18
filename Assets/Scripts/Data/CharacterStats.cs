using UnityEngine;

[System.Serializable]
public class Stats{
    public int Strength;
    public int Magic;
    public int Defense;
}

[CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObjects/Characters/CharacterStatsData", order = 2)]
public class CharacterStats : ScriptableObject {
    [SerializeField] Stats[] levelStats;
    [SerializeField] int hpGrowth;
    [SerializeField] int mpGrowth; 

    public int HpGrowth { get => hpGrowth; }
    public int MpGrowth { get => mpGrowth; }
    public Stats[] LevelStats { get => levelStats; }
}
