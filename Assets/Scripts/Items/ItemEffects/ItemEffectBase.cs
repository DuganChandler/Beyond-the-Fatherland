using UnityEngine;

public abstract class ItemEffectBase : ScriptableObject {
    [SerializeField] private string _effectName;

    [TextArea]
    [SerializeField] private string _effectNameDescription;
    public abstract void ApplyEffect(Character user, Character target);
}
