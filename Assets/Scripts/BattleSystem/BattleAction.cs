public class BattleAction {
    public BattleAction(ActionType type, BattleUnit user, BattleUnit target, ItemSlot itemSlot, AbilityBase abilityBase) {
        Type = type;
        User = user;
        Target = target;
        ItemSlot = itemSlot;
        AbilityBase = abilityBase;
    }
    public ActionType Type;
    public BattleUnit User;
    public BattleUnit Target;
    public ItemSlot ItemSlot;
    public AbilityBase AbilityBase;

    public void ResetBattleAction() {
        Type = ActionType.None;
        User = null;
        Target = null;
        ItemSlot = new();
        AbilityBase = null;
    }
}
