public class AbilityContext {
    public AbilityBase ability;
    public BattleUnit user;
    public BattleUnit target; // May be null for battlefield-only abilities
    public IBattleActions battleActions;

    public AbilityContext(AbilityBase ability, BattleUnit user, BattleUnit target, IBattleActions battleActions) {
        this.ability = ability;
        this.user = user;
        this.target = target;
        this.battleActions = battleActions;
    }
}
