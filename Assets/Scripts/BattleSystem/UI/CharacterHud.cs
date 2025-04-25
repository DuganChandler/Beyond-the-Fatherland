using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.PlayerLoop;

public class CharacterHud : MonoBehaviour {
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI hpValueText;
    [SerializeField] TextMeshProUGUI mpValueText;
    [SerializeField] InfoBar hpBar;
    [SerializeField] InfoBar mpBar;
    [SerializeField] Image characterPortrait;

    [SerializeField] GameObject buffs;
    [SerializeField] GameObject debuffs;
    [SerializeField] TextMeshProUGUI buffText;
    [SerializeField] TextMeshProUGUI debuffText;


    private Character _character;
    
    public bool IsAlive {
        get {
            return _character.IsAlive;
        }
    }

    public void SetData(Character character) {
        if (_character != null) {
            _character.OnHpChange -= UpdateHP;
            _character.OnMpChange -= UpdateMP;
        }
        _character = character;
        
        nameText.text = character.CharacterData.name;
        levelText.text = $"Lv. {character.Level}";
        characterPortrait.sprite = character.CharacterData.CharacterPortrait;


        hpBar.SetResource((float)character.HP / character.MaxHP);
        mpBar.SetResource((float)character.MP / character.MaxMP);

        hpValueText.text = $"{character.HP}"; 
        mpValueText.text = $"{character.MP}"; 

        _character.OnHpChange += UpdateHP;
        _character.OnMpChange += UpdateMP;
    }

    public void UpdateHP() {
        hpBar.SetResource((float) _character.HP / _character.MaxHP);
        hpValueText.text = $"{_character.HP}"; 
        // StartCoroutine(UpdateHPAsync());
    }

    // IEnumerator UpdateHPAsync() {
    //     yield return hpBar.SetResourceSmooth((float) _character.HP / _character.MaxHP);
    // }

    public void UpdateMP() {
        mpBar.SetResource((float) _character.MP / _character.MaxMP);
        mpValueText.text = $"{_character.MP}"; 
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
    void Update()
    {
        if(_character != null && GameManager.Instance.GameState == GameState.Battle){
            HandleBuffs();
            HandleDebuffs();   
        }
        
    }
    private void HandleBuffs(){
        if(_character.Buffs?.Count >=1){
            Debug.Log("buff");
            buffs.SetActive(true);
            foreach(var name in _character.Buffs){
                buffText.text = name.Item1.ConditionName;
            }
        }else{
            
            buffs.SetActive(false);
        }
    }

    private void HandleDebuffs(){
        if(_character.Debuffs?.Count >=1){
            debuffs.SetActive(true);
            foreach(var name in _character.Debuffs){
                debuffText.text = name.Item1.ConditionName;
            }
        }else{
            debuffs.SetActive(false);
        }
    }
}
