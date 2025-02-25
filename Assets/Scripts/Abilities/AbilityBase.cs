using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilities", menuName = "Abilities/Create new Ability", order = 0)]
public class AbilityBase: ScriptableObject
{
    [Header("Ability Name")]
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [Header("Types")]
    [SerializeField] Element element; //just in case for polish

    [Header("Stats")]
    [SerializeField] int power;
    [SerializeField] Modifier modifier;
    [SerializeField] int accuracy;
    [SerializeField] int actionCost;

    [Header("Attributes")]
    
    [SerializeField] AbilityTarget target;
    [SerializeField] AbilityCategory category;
    [SerializeField] AbilityEffects effects;
    [SerializeField] List<Secondaries> secondaries;

    [Header("Audio")]
    [SerializeField] AudioClip sound;

    public string Name {
        get { return name; }
    }

    public Element Element {
        get { return element; }
    }

    public int Power {
        get { return power; }
    }

    public int ActionCost {
        get { return actionCost; }
    }

    public int Accuracy {
        get { return accuracy; }
    }



    public AbilityCategory Category {
        get { return category; }
    }

    public AbilityEffects Effects {
        get { return effects; }
    }

    public AbilityTarget Target {
        get { return target; }
    }

    public List<Secondaries> Secondaries {
        get { return secondaries; }
    }

    public AudioClip Sound => sound;
}

[System.Serializable]
public class AbilityEffects 
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] AbilityProperty property;
    public List<StatBoost> Boosts {
        get { return boosts; }
    }
    public AbilityProperty Property {
        get { return property; }
    }
}

[System.Serializable]
public class Secondaries : AbilityEffects
{
    [SerializeField] int chance;
    [SerializeField] AbilityTarget target;

    public int Chance {
        get { return chance; }
    }

    public AbilityTarget Target {
        get { return target; }
    }
}

[System.Serializable]
public class StatBoost {
    public Stat stat;
    public int boost;
}

public enum Element { //again for later
    None
}

public enum AbilityCategory {
    Physical,
    Special,
    Status
}

public enum AbilityTarget {
    Foe,
    Self
}

[System.Serializable]
public class Modifier {
    public Stat stat;
}

public enum AbilityProperty{
    None,
    Impede,
    Cancel,
    Stun,
    Heal
}