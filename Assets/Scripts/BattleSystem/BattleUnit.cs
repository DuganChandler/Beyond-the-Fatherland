using UnityEngine;

public class BattleUnit {
    public BattleUnit(Character character, BattlePosition battlePosition, CharacterHud hud = null) {
        _character = character;
        _hud = hud;
        _battlePosition = battlePosition;
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

    private BattlePosition _battlePosition;
    public BattlePosition BattlePosition{
        get {
            return _battlePosition;
        } set {
            _battlePosition = value;
        }
    }


    public void Setup() {
        if (_character.CharacterData.CharacerType != CharacerType.PartyMember) {
            _character?.Init();
        }

        if (_hud) {
            _hud.SetData(_character);
        }

        if (_battlePosition) {
            _battlePosition.Occupied = true;
        }

    }
}
