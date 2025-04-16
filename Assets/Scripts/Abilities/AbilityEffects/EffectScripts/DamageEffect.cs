using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Abilities/Effects/Damage")]
public class AbilityDamageEffect : AbilityEffectBase {
    public override IEnumerator ApplyToCharacter(AbilityContext context) {
        // calcualte damage here
        // animate target damaged
        // animate damage number here
        context.target.Character.DecreaseHP(25);
        Debug.Log($"{context.target.Character.CharacterData.name} took 25 Damage");
        yield return null;
    }

}