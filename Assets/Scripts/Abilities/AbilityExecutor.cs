using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityExecutor : MonoBehaviour {
     public IEnumerator ExecuteAbility(AbilityBase ability, BattleUnit user, BattleUnit target, BattleSystem battleSystem) {
        AbilityContext context = new(ability, user, target, battleSystem);
        // wait for animations right here (sudo code):
        // yield return Waitfor(user.prefab.animate)
        Animator animator = user.CurrentModelInstance.GetComponent<Animator>();
            if( animator != null && user.Character.CharacterData.CharacerType == CharacerType.PartyMember) {
                animator.SetTrigger("Attack");
                yield return new WaitUntil(() => battleSystem.IsAnimating);
                battleSystem.IsAnimating = false;
            }

        foreach (AbilityEffectBase effect in ability.Effects) {
            yield return StartCoroutine(effect.ApplyToCharacter(context));
            yield return StartCoroutine(effect.ApplyToBattle(context));
        }
        yield return new WaitForEndOfFrame();
    }
}
