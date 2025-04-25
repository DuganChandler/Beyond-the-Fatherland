using System;
using System.Collections;
using UnityEngine;

public abstract class Condition : ScriptableObject {
    [SerializeField] private string _conditionName;

    public String ConditionName{
        get{
            return _conditionName;
        }
    }


    [TextArea]
    [SerializeField] private string _conditionNameDescription;
    [SerializeField] private int duration;
    [SerializeField] private ConditionType type;

    public ConditionType Type{
        get{
            return type;
        }
    }
    
    public int Duration{get;}

    public virtual IEnumerator ApplyToCharacter(Character target) { yield return null; }
    public virtual IEnumerator RemoveFromCharacter(Character target) { yield return null; }
}

public enum ConditionType{
    Buff,
    Debuff,
}
/*using System.Collections;
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
}*/
