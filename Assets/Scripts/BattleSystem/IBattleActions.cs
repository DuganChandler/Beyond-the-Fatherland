using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleActions {
    bool TrySummonEnemy(Character enemy);
    void HandleAnimationEnd();
    void HandleRemoveItem(ItemBase item);
    void HandleSFXTriggered();
    void CreateDamageTextAtTarget(Transform target, string text, Color color);
}
