using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUser : MonoBehaviour {
    public IEnumerator UseItem(CombatItemData item, Character user, Character target) {
        foreach (ItemEffectBase effect in item.effects) {
            effect.ApplyEffect(user, target);
        }
        yield return new WaitForEndOfFrame();
    }
}
