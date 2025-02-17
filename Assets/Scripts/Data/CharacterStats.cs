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
    // Party Memebers
    // public static Dictionary<int, PlayerStats> RenivarStats { get; set; } = new Dictionary<int, PlayerStats>() {
    //     {
    //         1,
    //         new PlayerStats(5, 2, 3)
    //     },
    //     {
    //         2,
    //         new PlayerStats(7, 2, 4)
    //     },
    //     {
    //         3,
    //         new PlayerStats(9, 2, 5)
    //     },
    //     {
    //         4,
    //         new PlayerStats(10, 3, 5)
    //     },
    //     {
    //         5,
    //         new PlayerStats(11, 3, 6)
    //     },
    //     {
    //         6,
    //         new PlayerStats(13, 4, 8)
    //     },
    //     {
    //         7,
    //         new PlayerStats(15, 4, 8)
    //     },
    //     {
    //         8,
    //         new PlayerStats(16, 4, 9)
    //     },
    //     {
    //         9,
    //         new PlayerStats(18, 5, 11)
    //     },
    //     {
    //         10,
    //         new PlayerStats(20, 6, 12)
    //     },
    // };

    // public static Dictionary<int, PlayerStats> LuceraStats { get; set; } = new Dictionary<int, PlayerStats>() {
    //     {
    //         1,
    //         new PlayerStats(2, 5, 3)
    //     },
    //     {
    //         2,
    //         new PlayerStats(2, 7, 4)
    //     },
    //     {
    //         3,
    //         new PlayerStats(2, 9, 5)
    //     },
    //     {
    //         4,
    //         new PlayerStats(3, 10, 5)
    //     },
    //     {
    //         5,
    //         new PlayerStats(4, 11, 7)
    //     },
    //     {
    //         6,
    //         new PlayerStats(4, 13, 8)
    //     },
    //     {
    //         7,
    //         new PlayerStats(6, 15, 8)
    //     },
    //     {
    //         8,
    //         new PlayerStats(7, 16, 10)
    //     },
    //     {
    //         9,
    //         new PlayerStats(9, 18, 12)
    //     },
    //     {
    //         10,
    //         new PlayerStats(10, 20, 12)
    //     },

    // };

    // public static Dictionary<int, PlayerStats> NorbertStats { get; set; } = new Dictionary<int, PlayerStats>() {
    //     {
    //         1,
    //         new PlayerStats(3, 2, 5)
    //     },
    //     {
    //         2,
    //         new PlayerStats(5, 3, 7)
    //     },
    //     {
    //         3,
    //         new PlayerStats(5, 3, 9)
    //     },
    //     {
    //         4,
    //         new PlayerStats(7, 4, 12)
    //     },
    //     {
    //         5,
    //         new PlayerStats(9, 5, 12)
    //     },
    //     {
    //         6,
    //         new PlayerStats(9, 7, 14)
    //     },
    //     {
    //         7,
    //         new PlayerStats(11, 7, 15)
    //     },
    //     {
    //         8,
    //         new PlayerStats(12, 9, 17)
    //     },
    //     {
    //         9,
    //         new PlayerStats(12, 10, 17)
    //     },
    //     {
    //         10,
    //         new PlayerStats(14, 12, 20)
    //     },

    // };

    // // Enemies
    // public static Dictionary<int, PlayerStats> TigermooseStats { get; set; } = new Dictionary<int, PlayerStats>() {
    //     {
    //         1,
    //         new PlayerStats(4, 1, 3)
    //     },
    //     {
    //         2,
    //         new PlayerStats(5, 1, 4)
    //     },
    //     {
    //         3,
    //         new PlayerStats(7, 2, 5)
    //     },
    //     {
    //         4,
    //         new PlayerStats(8, 2, 6)
    //     },
    //     {
    //         5,
    //         new PlayerStats(10, 2, 7)
    //     },
    //     {
    //         6,
    //         new PlayerStats(12, 2, 8)
    //     },
    //     {
    //         7,
    //         new PlayerStats(14, 3, 9)
    //     },
    //     {
    //         8,
    //         new PlayerStats(16, 3, 10)
    //     },
    //     {
    //         9,
    //         new PlayerStats(18, 3, 11)
    //     },
    //     {
    //         10,
    //         new PlayerStats(20, 3, 12)
    //     },

    // };

    // public static Dictionary<int, PlayerStats> HornetbatStats { get; set; } = new Dictionary<int, PlayerStats>() {
    //     {
    //         1,
    //         new PlayerStats(2, 4, 2)
    //     },
    //     {
    //         2,
    //         new PlayerStats(2, 5, 3)
    //     },
    //     {
    //         3,
    //         new PlayerStats(3, 6, 3)
    //     },
    //     {
    //         4,
    //         new PlayerStats(3, 8, 4)
    //     },
    //     {
    //         5,
    //         new PlayerStats(3, 10, 4)
    //     },
    //     {
    //         6,
    //         new PlayerStats(3, 12, 5)
    //     },
    //     {
    //         7,
    //         new PlayerStats(3, 14, 5)
    //     },
    //     {
    //         8,
    //         new PlayerStats(5, 16, 6)
    //     },
    //     {
    //         9,
    //         new PlayerStats(5, 18, 6)
    //     },
    //     {
    //         10,
    //         new PlayerStats(5, 20, 7)
    //     },

    // };

    // //Boss
    // public static Dictionary<int, PlayerStats> CeramicGolemStats { get; set; } = new Dictionary<int, PlayerStats>() {
    //     {
    //         8,
    //         new PlayerStats(22, 10, 18)
    //     },
    //     // {
    //     //     9,
    //     //     new PlayerStats(18, 5, 11)
    //     // },
    //     // {
    //     //     10,
    //     //     new PlayerStats(20, 6, 12)
    //     // },
    // };
}
