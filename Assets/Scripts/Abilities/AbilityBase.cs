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
public class AbilityBase: ScriptableObject {
    [Header("Ability Information")]
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [Header("Elemental Attribute")]
    [SerializeField] Element element; 

    [Header("Stats")]
    [SerializeField] int power;
    [SerializeField] int actionCost;

    [SerializeField] bool costHP;

    [Header("Attributes")]
    [SerializeField] AbilityTarget target;
    [SerializeField] AbilityCategory category;
    [SerializeField] bool isAOE;

    [Header("Audio")]
    [SerializeField] AudioClip sound;

    [Header("Ability Effects")]
    [SerializeField] List<AbilityEffectBase> effects;

    public string AbilityName => name;
    public int Power => power;
    public int ActionCost => actionCost;
    public bool CostHP => costHP;
    public Element Element => element;
    public List<AbilityEffectBase> Effects => effects;
    public AbilityCategory Category => category;
    public AbilityTarget AbilityTarget => target;
    public AudioClip Sound => sound;
    public string AbilityDescription => description;
}