using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleActions {
    bool TrySummonEnemy(Character enemy);
    void HandleAnimationEnd();
}
