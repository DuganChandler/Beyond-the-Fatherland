using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Revive Effect", menuName = "Items/Effects/RevivePlayer")]
public class ReviveEffect : ItemEffectBase {
    public override IEnumerator ApplyToCharacter(ItemContext context) {
        if (!context.target.Character.IsAlive) {
            context.target.Character.IsAlive = true;
            yield return null;
        }
        yield return null;
    }
}
