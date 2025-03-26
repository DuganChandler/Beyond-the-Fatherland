using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Items/Effects/HealHP")]
public class HealEffect : ItemEffectBase {
    [SerializeField] private int healAmount;

    public override EffectInfo ApplyEffect(Character user, Character target) {
        target.IncreaseHP(healAmount);
        return new EffectInfo(Color.green, $"{healAmount}");
    }

}
