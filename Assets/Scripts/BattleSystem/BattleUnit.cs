using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleUnit {
    public BattleUnit(Character character, CharacterHud hud = null) {
        _character = character;
        _hud = hud;
    }

    // default to null if an enemy
    private CharacterHud _hud = null;
    public CharacterHud Hud {
        get {
            return _hud;
        } set {
            _hud = value;
        }
    }

    private Character _character;
    public Character Character {
        get {
            return _character;
        }
    }

    private GameObject _currentModelInstance;
    public GameObject CurrentModelInstance{
        get {
            return _currentModelInstance;
        } set {
            _currentModelInstance = value;
        }
    }


    public void Setup() {
        if (_hud) {
            _hud.SetData(_character);
        }
    }
}
