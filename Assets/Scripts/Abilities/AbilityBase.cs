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
    
    [SerializeField] int cost;

    [Header("Attributes")]
    
    [SerializeField] AbilityTarget target;
    [SerializeField] bool isAOE;
    [SerializeField] AbilityCategory category;
    //[SerializeField] List<Secondaries> secondaries;

    [Header("Audio")]
    [SerializeField] AudioClip sound;
    [SerializeField] List<AbilityEffectBase> effects;

    public string AbilityName {
        get { return name; }
    }

    public Element Element {
        get { return element; }
    }

    public int Power {
        get { return power; }
    }

    public int Cost {
        get { return cost; }
    }

    public List<AbilityEffectBase> Effects{
        get{
            return effects;
        }
    }


    public AbilityCategory Category {
        get { return category; }
    }

    public  AbilityTarget AbilityTarget{
        get{
          return target;
        }
    }

    /*public List<Secondaries> Secondaries {
        get { return secondaries; }
    }*/

    public AudioClip Sound => sound;
}



public enum Element { //again for later
    None
}

public enum AbilityCategory {
    Strength,
    Magic,
    Status
}

public enum AbilityTarget {
    Enemy,
    Player
}


public enum AbilityProperty{
    None,
    Impede,
    Stun,
    Heal
}