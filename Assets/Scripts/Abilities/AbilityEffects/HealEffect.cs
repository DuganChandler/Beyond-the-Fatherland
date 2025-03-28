using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Abilities/Effects/HealHP")]
public class AbilityHealEffect : AbilityEffectBase {
    [SerializeField] private int abilityHealAmount;

    public override AbilityEffectInfo ApplyEffect(Character user, Character target) {
        target.IncreaseHP(abilityHealAmount);
        return new AbilityEffectInfo(Color.green, $"{abilityHealAmount}");
    }

}