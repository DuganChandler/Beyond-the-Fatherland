using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Abilities/Effects/Damage")]
public class AbilityDamageEffect : AbilityEffectBase {
    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        int damage = context.user.Character.CalculateAbilityPower(context.ability.Power,context.target.Character);
        context.target.Character.DecreaseHP(damage);

        context.battleActions.CreateDamageTextAtTarget(context.target.CurrentModelInstance.transform, $"{damage}", Color.red);
        // GameObject damageTextObject = context.target.CurrentModelInstance.transform.GetChild(0).gameObject;
        // damageTextObject.SetActive(true);
        // damageTextObject.GetComponent<DamageText>().text.text = $"{damage}";

        yield return null;
    }

}