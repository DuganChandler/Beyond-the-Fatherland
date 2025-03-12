using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterHud : MonoBehaviour {
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] InfoBar hpBar;
    [SerializeField] InfoBar mpBar;
    [SerializeField] GameObject actionPanel;

    private Character _character;
    
    public bool IsAlive {
        get {
            return _character.IsAlive;
        }
    }

    public GameObject ActionPanel {
        get {
            return actionPanel;
        }
    }

    public void SetData(Character character) {
        if (_character != null) {
            _character.OnHpChange -= UpdateHP;
            _character.OnMpChange -= UpdateMP;
        }
        _character = character;
        
        nameText.text = character.CharacterData.name;

        hpBar.SetResource((float)character.HP / character.MaxHP);
        mpBar.SetResource((float)character.MP / character.MaxMP);

        _character.OnHpChange += UpdateHP;
        _character.OnMpChange += UpdateMP;
    }

    public void UpdateHP() {
        hpBar.SetResource((float) _character.HP / _character.MaxHP);
        // StartCoroutine(UpdateHPAsync());
    }

    // IEnumerator UpdateHPAsync() {
    //     yield return hpBar.SetResourceSmooth((float) _character.HP / _character.MaxHP);
    // }

    public void UpdateMP() {
        mpBar.SetResource((float) _character.MP / _character.MaxMP);
        // StartCoroutine(UpdateMPAsync());
    }

    // IEnumerator UpdateMPAsync() {
    //     yield return mpBar.SetResourceSmooth((float) _character.MP / _character.MaxMP);
    // }

    public void ClearData() {
        if (_character != null) {
            _character.OnHpChange -= UpdateHP;
            _character.OnMpChange -= UpdateMP;
        }
    }
}
