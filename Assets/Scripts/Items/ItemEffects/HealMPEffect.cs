using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New MP Heal Effect", menuName = "Items/Effects/HealMP")]
public class HealMPEffect : ItemEffectBase {
    [SerializeField] private int healAmount;

    public override IEnumerator ApplyToCharacter(ItemContext context) {
        if (!context.target.Character.IsAlive) {
            yield return null;
        }

        context.target.Character.IncraseMP(healAmount);
        yield return null;
    }
}
