using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Items/Effects/HealHP")]
public class HealEffect : ItemEffectBase {
    [SerializeField] private int healAmount;
    public override IEnumerator ApplyToCharacter(ItemContext context) {
        if (context.target.Character.IsAlive) {
            context.target.Character.IncreaseHP(healAmount);
            // GameObject damageTextObject = context.target.CurrentModelInstance.transform.GetChild(0).gameObject;
            // damageTextObject.SetActive(true);
            // damageTextObject.GetComponent<DamageText>().text.text = $"<color=green>{healAmount}</color>";
            context.battleActions.CreateDamageTextAtTarget(context.target.CurrentModelInstance.transform, $"{healAmount}", Color.green);
            yield return null;
        }
        yield return null;
    }

}
