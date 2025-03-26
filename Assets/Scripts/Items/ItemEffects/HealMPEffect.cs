using UnityEngine;

[CreateAssetMenu(fileName = "New MP Heal Effect", menuName = "Items/Effects/HealMP")]
public class HealMPEffect : ItemEffectBase {
    [SerializeField] private int healAmount;

    public override void ApplyEffect(Character user, Character target) {
        target.IncreaseHP(healAmount);
    }
}
