using UnityEngine;

public struct AbilityEffectInfo {
    public AbilityEffectInfo (Color textColor, string textInfo) {
        TextColor = textColor;
        TextInformation = textInfo;
    }

    public Color TextColor; 
    public string TextInformation;
}

public abstract class AbilityEffectBase : ScriptableObject {
    [SerializeField] private string _effectName;

    [TextArea]
    [SerializeField] private string _effectNameDescription;
    public abstract AbilityEffectInfo ApplyEffect(Character user, Character target);
}
