using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Decrease HP Effect", menuName = "Abilities/Effects/DeceaseHP")]
public class DecreaseHPEffect : AbilityEffectBase {
    [SerializeField] private int hpAmount;

    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        context.user.Character.DecreaseHP(hpAmount);
        yield return null;
    }

}
