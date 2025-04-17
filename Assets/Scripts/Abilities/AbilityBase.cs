using System.Collections.Generic;
using UnityEngine;

public enum Element { 
    None
}

public enum AbilityCategory {
    Strength,
    Magic,
    Status,
    Special
}

public enum AbilityTarget {
    Enemy,
    Player,
    Battle
}


public enum AbilityProperty{
    None,
    Impede,
    Stun,
    Heal
}

[CreateAssetMenu(fileName = "Abilities", menuName = "Abilities/Create new Ability", order = 0)]
public class AbilityBase: ScriptableObject
{
    [Header("Ability Information")]
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [Header("Elemental Attribute")]
    [SerializeField] Element element; 

    [Header("Stats")]
    [SerializeField] int power;
    [SerializeField] int actionCost;

    [Header("Attributes")]
    [SerializeField] AbilityTarget target;
    [SerializeField] AbilityCategory category;
    [SerializeField] bool isAOE;
    //[SerializeField] List<Secondaries> secondaries;

    [Header("Audio")]
    [SerializeField] AudioClip sound;
    [SerializeField] List<AbilityEffectBase> effects;

    public string AbilityName { get => name; }
    public int Power { get => power; }
    public int ActionCost { get => actionCost; }
    public Element Element { get => element; }
    public List<AbilityEffectBase> Effects{ get => effects; }
    public AbilityCategory Category { get => category; }
    public AbilityTarget AbilityTarget { get => target; }
    public AudioClip Sound => sound;
}