using System.Collections.Generic;
using UnityEngine;

public class EncounterMapArea : MonoBehaviour {
    [SerializeField] List<EncounterList> encounterList;

    public List<Character> GetRandomEncounter() {
        List<Character> encounter = encounterList[Random.Range(0, encounterList.Count)].CharacterList;
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
