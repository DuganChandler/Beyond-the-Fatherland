using UnityEngine;

[CreateAssetMenu(fileName = "New Decrease MP Effect", menuName = "Abilities/Effects/DecraseMP")]
public class DecreaseMPEffect : AbilityEffectBase {
    [SerializeField] private int mpAmount;

    public override AbilityEffectInfo ApplyEffect(Character user, Character target) {
        user.DecreaseMP(mpAmount);
        Color color = Color.white;
        color.a = 0f;
        return new AbilityEffectInfo(color, $"{mpAmount}");
    }

}
