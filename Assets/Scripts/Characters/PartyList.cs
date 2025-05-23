using System.Collections.Generic;
using UnityEngine;

public class PartyList : MonoBehaviour
{
    [Header("Party Members")]
    [SerializeField] List<Character> characterList;

    public List<Character> CharacterList {
        get {
            return characterList;
        }
    }

    private void Awake() {
        foreach (var character in characterList) {
            character.Init();
        }

        BattleManager.Instance.PlayerPartyList = characterList;
    }

    public int CalculateAveragePartyLevel() {
        int totalLevels = 0;
        foreach(Character character in characterList) {
            totalLevels += character.Level;
        }

        return totalLevels / characterList.Count;
    }
}
