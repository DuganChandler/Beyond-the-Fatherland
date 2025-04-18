using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Abilities/Effects/Damage")]
public class AbilityDamageEffect : AbilityEffectBase {
    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        // calcualte damage here
        // animate target damaged
        // animate damage number here
        
        int damage = context.user.Character.CalculateAbilityPower(context.ability.Power,context.target.Character);
        context.target.Character.DecreaseHP(damage);
        GameObject damageTextObject = context.target.CurrentModelInstance.transform.GetChild(0).gameObject;
            damageTextObject.SetActive(true);
            damageTextObject.GetComponent<DamageText>().text.text = $"{damage}";
        yield return null;
    }

}