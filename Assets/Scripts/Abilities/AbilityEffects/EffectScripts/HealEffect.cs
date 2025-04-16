using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Abilities/Effects/HealHP")]
public class AbilityHealEffect : AbilityEffectBase {
    [SerializeField] private int abilityHealAmount;

    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        context.target.Character.IncreaseHP(abilityHealAmount);
        yield return null;
    }

}