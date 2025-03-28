using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI abilityName;

    public TextMeshProUGUI AbilityName {
        get {
            return abilityName;
        } set {
            abilityName = value;
        }
    }
}
