public class BattleAction {
    private ActionType _type;
    private BattleUnit _user;
    private BattleUnit _target;
    private ItemSlot _itemSlot;
    private AbilityBase _abilityBase;

    public ActionType Type {
        get {
            return _type;
        } set {
            _type = value;
        }
    }

    public BattleUnit User {
        get {
            return _user;
        } set {
            _user = value;
        }
    }

    public BattleUnit Target {
        get {
            return _target;
        } set {
            _target = value;
        }
    }

    public ItemSlot ItemSlot {
        get {
            return _itemSlot;
        } set {
            _itemSlot = value;
        }
    }

    public AbilityBase AbilityBase {
        get {
            return _abilityBase;
        } set {
            _abilityBase = value;
        }
    }


}
