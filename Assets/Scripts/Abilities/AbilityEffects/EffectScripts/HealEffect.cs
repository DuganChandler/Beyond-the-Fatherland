using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Abilities/Effects/HealHP")]
public class AbilityHealEffect : AbilityEffectBase {

    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        // int heal = (int)(((float)context.user.Character.Stats.Magic * (2.0f + (float)context.user.Character.Level)/(3.0f + 1.0f) + 100.0f) / (100.0f * (float)context.ability.Power));
        int heal = 35;
        context.target.Character.IncreaseHP(heal);
        context.battleActions.CreateDamageTextAtTarget(context.target.CurrentModelInstance.transform, $"{heal}", Color.green);
        yield return null;
    }

}