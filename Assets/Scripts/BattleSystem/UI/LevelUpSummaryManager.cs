using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpSummaryManager : MonoBehaviour {
    [SerializeField] List<SummaryCharacterInfo> summaryCharacterInfoList;

    [Header("Summary")]
    [SerializeField] TextMeshProUGUI expEarnedText;

    public void SetSummaryUI(List<BattleUnit> units, int expEarned) {
        for (int i = 0; i < units.Count; i++) {
            Character currentCharacter = units[i].Character;
            SummaryCharacterInfo currentCharacterInfo = summaryCharacterInfoList[i];

            currentCharacterInfo.CharacterPortrait.sprite = currentCharacter.CharacterData.CharacterPortrait;
            currentCharacterInfo.CharacterName.text = currentCharacter.CharacterData.name;
            currentCharacterInfo.CharacterLevel.text = $"Lv: {currentCharacter.Level}";
            currentCharacterInfo.CharacterEXP.text = $"{currentCharacter.EXP}/100";
            if (currentCharacter.LeveledUp) {
                currentCharacterInfo.CharacterArrow.SetActive(true);
                currentCharacter.LeveledUp = false;
            }
        }
        expEarnedText.text = $"{expEarned}";
    }


    void OnDisable() {
        foreach (var characterInfo in summaryCharacterInfoList) {
            characterInfo.CharacterArrow.SetActive(false);
        } 
    }
}
