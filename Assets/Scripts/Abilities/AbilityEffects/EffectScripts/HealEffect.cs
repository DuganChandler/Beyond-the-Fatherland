using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Abilities/Effects/HealHP")]
public class AbilityHealEffect : AbilityEffectBase {

    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        int heal = (int)(((float)context.user.Character.Stats.Magic * (2.0f + (float)context.user.Character.Level)/(3.0f + 1.0f) + 100.0f) / (100.0f * (float)context.ability.Power));
        context.target.Character.IncreaseHP(heal);
        GameObject damageTextObject = context.target.CurrentModelInstance.transform.GetChild(0).gameObject;
            damageTextObject.SetActive(true);
            damageTextObject.GetComponent<DamageText>().text.text = $"<color=green>{heal}</color>";
        yield return null;
    }

}