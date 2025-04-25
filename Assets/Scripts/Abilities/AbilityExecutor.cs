using System.Collections;
using UnityEngine;

public class AbilityExecutor : MonoBehaviour {
     public IEnumerator ExecuteAbility(AbilityBase ability, BattleUnit user, BattleUnit target, BattleSystem battleSystem) {
        AbilityContext context = new(ability, user, target, battleSystem);
        Animator animator = user.CurrentModelInstance.GetComponent<Animator>();
        if( animator != null && user.Character.CharacterData.CharacerType == CharacerType.PartyMember) {
            animator.SetTrigger("Attack");

            yield return new WaitUntil(() => battleSystem.PlaySFX);
            MusicManager.Instance.PlaySoundByAudioClip(ability.Sound); 
            battleSystem.PlaySFX = false;

            yield return new WaitUntil(() => battleSystem.IsAnimating);
            battleSystem.IsAnimating = false;
        }

        foreach (AbilityEffectBase effect in ability.Effects) {
            yield return StartCoroutine(effect.ApplyToCharacter(context));
            yield return StartCoroutine(effect.ApplyToBattle(context));
        }
        foreach(Condition condition in ability.Conditions){
            yield return StartCoroutine(condition.ApplyToCharacter(context.target.Character));
        }
        if(ability.CostHP){
            user.Character.DecreaseHP(ability.ActionCost);
        }else{
            user.Character.DecreaseMP(ability.ActionCost);
        }

        yield return new WaitForEndOfFrame();
    }
}
