using UnityEngine;

[CreateAssetMenu(fileName = "New MP Heal Effect", menuName = "Items/Effects/HealMP")]
public class HealMPEffect : ItemEffectBase {
    [SerializeField] private int healAmount;

    public override EffectInfo ApplyEffectToCharacter(Character user, Character target) {
        if (!target.IsAlive) {
            return new EffectInfo(Color.red, $"N");
        }

        target.IncreaseHP(healAmount);
        return new EffectInfo(Color.blue, $"{healAmount}");
    }
}
