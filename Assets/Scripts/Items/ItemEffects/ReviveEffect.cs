using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Revive Effect", menuName = "Items/Effects/RevivePlayer")]
public class ReviveEffect : ItemEffectBase {
    public override EffectInfo ApplyEffectToCharacter(Character user, Character target) {
        if (!target.IsAlive) {
            target.IsAlive = true;
            return new EffectInfo(Color.green, $"Revived");
        }
        return new EffectInfo(Color.white, "No Effect");
    }
}
