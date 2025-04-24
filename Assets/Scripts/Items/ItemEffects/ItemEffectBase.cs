using System.Collections;
using UnityEngine;

public struct EffectInfo {
    public EffectInfo (Color textColor, string textInfo) {
        TextColor = textColor;
        TextInformation = textInfo;
    }

    public Color TextColor; 
    public string TextInformation;
}

public enum EffectType {
    Battle,
    Character,
    Both
}

public abstract class ItemEffectBase : ScriptableObject {
    [SerializeField] private string _effectName;
    [SerializeField] private EffectType _effectType;

    [TextArea]
    [SerializeField] private string _effectNameDescription;

    public EffectType EffectType { get => _effectType; }

    public virtual IEnumerator ApplyToCharacter(ItemContext context) { yield return null; }

    public virtual IEnumerator ApplyToBattle(ItemContext context) { yield return null; }
}
