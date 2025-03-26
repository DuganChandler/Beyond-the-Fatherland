using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUser : MonoBehaviour {
    public IEnumerator UseItem(CombatItemData item, Character user, Character target) {
        List<EffectInfo> effectInfoList = new();
        foreach (ItemEffectBase effect in item.effects) {
            effectInfoList.Add(effect.ApplyEffect(user, target));
        }
        yield return new WaitForEndOfFrame();
    }
}
