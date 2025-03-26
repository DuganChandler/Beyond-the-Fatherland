using UnityEngine;

public struct EffectInfo {
    public EffectInfo (Color textColor, string textInfo) {
        TextColor = textColor;
        TextInformation = textInfo;
    }

    public Color TextColor; 
    public string TextInformation;
}

public abstract class ItemEffectBase : ScriptableObject {
    [SerializeField] private string _effectName;

    [TextArea]
    [SerializeField] private string _effectNameDescription;
    public abstract EffectInfo ApplyEffect(Character user, Character target);
}
