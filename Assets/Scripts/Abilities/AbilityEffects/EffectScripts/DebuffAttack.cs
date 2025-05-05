using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff Effect", menuName = "Condition/Attack Debuff")]
public class AttackDebuffEffect : Condition {

    [Header("Buff Ammount (ex: .1 = 10% buff/debuff)")]
    [SerializeField] private float buffAmmount;

    public override IEnumerator ApplyToCharacter(Character target)
    {
        target.AttackMod -= buffAmmount;
        target.AddCondition(this,target);

        yield return null;
    }
    public override IEnumerator RemoveFromCharacter(Character target)
    {
        target.AttackMod += buffAmmount;

        yield return null;
    }
}