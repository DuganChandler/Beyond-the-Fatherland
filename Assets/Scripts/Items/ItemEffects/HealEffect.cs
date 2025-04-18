using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Items/Effects/HealHP")]
public class HealEffect : ItemEffectBase {
    [SerializeField] private int healAmount;

    public override EffectInfo ApplyEffectToCharacter(Character user, Character target) {
        if (target.IsAlive) {
            target.IncreaseHP(healAmount);
            return new EffectInfo(Color.green, $"{healAmount}");
        }
        return new EffectInfo(Color.red, "No Effect");
    }

}
