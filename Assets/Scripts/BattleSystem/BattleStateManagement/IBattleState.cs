public interface IBattleState {
    BattleState State { get; }

    void OnEnter();
    void OnExit();

    IBattleState OnBack();
}
