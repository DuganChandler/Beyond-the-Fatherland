using System.Collections.Generic;
using UnityEngine;

public enum EncounterLevel {
    Low,
    Med,
    High
}

public class EncounterMapArea : MonoBehaviour {
    [SerializeField] List<EncounterList> lowEncounters;
    [SerializeField] List<EncounterList> medEncounters;
    [SerializeField] List<EncounterList> highEncounters;


    public List<Character> GetRandomEncounter(int averageLevel) {
        List<Character> encounter = new();
        EncounterLevel encounterLevel = new();
        if (averageLevel <= 3) {
            encounterLevel = EncounterLevel.Low;
        } else if (averageLevel > 3 && averageLevel <= 6) {
            encounterLevel = EncounterLevel.Med;
        } else {
            encounterLevel = EncounterLevel.High;
        }

        Debug.Log(encounterLevel);

        switch (encounterLevel) {
            case EncounterLevel.Low:
                encounter = lowEncounters[Random.Range(0, lowEncounters.Count)].CharacterList;
                break;
            case EncounterLevel.Med:
                encounter = medEncounters[Random.Range(0, medEncounters.Count)].CharacterList;
                break;
            case EncounterLevel.High:
                encounter = highEncounters[Random.Range(0, highEncounters.Count)].CharacterList;
                break;
            default:
                break;
        }

        return new List<Character>(encounter);
    }
}

[System.Serializable]
public class EncounterList {
    [SerializeField] List<Character> characterList; 
    public List<Character> CharacterList { 
        get {
            return characterList;
        }
    }
}
