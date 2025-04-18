using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Abilities/Effects/HealHP")]
public class AbilityHealEffect : AbilityEffectBase {

    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        int heal = (int)((context.user.Character.Stats.Magic * (2 + context.user.Character.Level)/(3 + 1) +100) / (100 * context.ability.Power));
        context.target.Character.IncreaseHP(heal);
        GameObject damageTextObject = context.target.CurrentModelInstance.transform.GetChild(0).gameObject;
            damageTextObject.SetActive(true);
            damageTextObject.GetComponent<DamageText>().text.text = $"<color=green>{heal}</color>";
        yield return null;
    }

}