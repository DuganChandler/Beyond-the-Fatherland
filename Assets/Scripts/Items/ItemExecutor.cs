using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExecutor : MonoBehaviour {
     public IEnumerator ExecuteItem(CombatItemData item, BattleUnit user, BattleUnit target, BattleSystem battleSystem) {
        ItemContext context = new(item, user, target, battleSystem);
        Animator animator = user.CurrentModelInstance.GetComponent<Animator>();
            if(animator != null && user.Character.CharacterData.CharacerType == CharacerType.PartyMember) {
                animator.SetTrigger("Attack");
                yield return new WaitUntil(() => battleSystem.IsAnimating);
                battleSystem.IsAnimating = false;
            }

        foreach (ItemEffectBase effect in item.Effects) {
            yield return StartCoroutine(effect.ApplyToCharacter(context));
            yield return StartCoroutine(effect.ApplyToBattle(context));
        }

        battleSystem.HandleRemoveItem(item);

        yield return new WaitForEndOfFrame();
    }
}
