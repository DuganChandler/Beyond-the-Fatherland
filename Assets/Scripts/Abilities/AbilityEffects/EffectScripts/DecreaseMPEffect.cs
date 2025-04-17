using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Decrease MP Effect", menuName = "Abilities/Effects/DecraseMP")]
public class DecreaseMPEffect : AbilityEffectBase {
    [SerializeField] private int mpAmount;

    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        context.user.Character.DecreaseMP(mpAmount);
        yield return null;
    }

}
