using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Condition", menuName = "Abilities/Effects/Condition")]
public class Condition : AbilityEffectBase
{
    [SerializeField] public int initilaRound;
    [SerializeField] public int duration;
    [SerializeField] private string stat;
    public override IEnumerator ApplyToCharacter(AbilityContext context)
    {
        return base.ApplyToCharacter(context);
    }
}
