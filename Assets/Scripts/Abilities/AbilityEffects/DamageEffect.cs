using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Abilities/Effects/Damage")]
public class AbilityDamageEffect : AbilityEffectBase {
    [SerializeField] private int abilityDamageAmount;

    public override AbilityEffectInfo ApplyEffect(Character user, Character target) {
        target.DecreaseHP(abilityDamageAmount);
        return new AbilityEffectInfo(Color.red, $"{abilityDamageAmount}");
    }

}