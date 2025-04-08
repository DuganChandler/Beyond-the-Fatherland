using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI abilityName;
    [SerializeField] private TextMeshProUGUI abilityCost;

    public TextMeshProUGUI AbilityName {
        get {
            return abilityName;
        } set {
            abilityName = value;
        }
    }
    public TextMeshProUGUI AbilityCost {
        get {
            return abilityCost;
        } set {
            abilityCost = value;
        }
    }
}
