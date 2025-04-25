using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New MP Swap", menuName = "Abilities/Effects/MpSwap")]
public class MpSwap : AbilityEffectBase {

    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        context.target.Character.IncraseMP(context.ability.Power);
        context.battleActions.CreateDamageTextAtTarget(context.target.CurrentModelInstance.transform, $"{context.ability.Power}", Color.blue);
        yield return null;
    }

}