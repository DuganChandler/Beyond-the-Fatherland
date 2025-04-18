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
    [SerializeField] private EffectType effectType;

    [TextArea]
    [SerializeField] private string _effectNameDescription;

    public abstract EffectInfo ApplyEffectToCharacter(Character user, Character target);

    public virtual EffectInfo ApplyEffectToBattle(BattleSystem battleSystem) { return new EffectInfo(Color.clear, "No effect here"); }
}
