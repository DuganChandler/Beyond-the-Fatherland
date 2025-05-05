public class ItemContext {
    public CombatItemData combatItem;
    public BattleUnit user;
    public BattleUnit target;
    public IBattleActions battleActions;


    public ItemContext(CombatItemData combatItem, BattleUnit user, BattleUnit target, IBattleActions battleActions) {
        this.combatItem = combatItem;
        this.user = user;
        this.target = target;
        this.battleActions = battleActions;
    }
}
