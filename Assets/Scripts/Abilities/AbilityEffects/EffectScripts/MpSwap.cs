using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New MP Swap", menuName = "Abilities/Effects/MpSwap")]
public class MpSwap : AbilityEffectBase {

    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        context.target.Character.IncraseMP(context.ability.Power);
        // GameObject damageTextObject = context.target.CurrentModelInstance.transform.GetChild(0).gameObject;
        //     damageTextObject.SetActive(true);
        //     damageTextObject.GetComponent<DamageText>().text.text = $"<color=blue>{context.ability.Power}</color>";
        context.battleActions.CreateDamageTextAtTarget(context.target.CurrentModelInstance.transform, $"{context.ability.Power}", Color.blue);
        yield return null;
    }

}