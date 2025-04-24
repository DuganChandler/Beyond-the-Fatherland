using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Revive Effect", menuName = "Items/Effects/RevivePlayer")]
public class ReviveEffect : ItemEffectBase {
    public override IEnumerator ApplyToCharacter(ItemContext context) {
        if (!context.target.Character.IsAlive) {
            if (context.target.CurrentModelInstance.TryGetComponent<Animator>(out var animator)) {
                animator.SetTrigger("Revive");
            }

            context.target.Character.IsAlive = true;

            yield return null;
        }
        yield return null;
    }
}
