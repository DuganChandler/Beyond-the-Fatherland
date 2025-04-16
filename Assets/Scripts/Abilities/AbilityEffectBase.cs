using System.Collections;
using UnityEngine;

public struct AbilityEffectInfo {
    public AbilityEffectInfo (Color textColor, string textInfo) {
        TextColor = textColor;
        TextInformation = textInfo;
    }

    public Color TextColor; 
    public string TextInformation;
}
// damage, type of attack, description of whad had happened; 

public abstract class AbilityEffectBase : ScriptableObject {
    [SerializeField] private string _effectName;
    [SerializeField] private EffectType _effectType;

    [TextArea]
    [SerializeField] private string _effectNameDescription;

    public EffectType EffectType { get => _effectType;}

    public virtual IEnumerator ApplyToCharacter(AbilityContext context) { yield return null; }

    public virtual IEnumerator ApplyToBattle(AbilityContext context) { yield return null; }
}
