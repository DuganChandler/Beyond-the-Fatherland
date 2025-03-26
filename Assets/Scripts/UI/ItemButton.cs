using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;

    public TextMeshProUGUI ItemName {
        get {
            return itemName;
        } set {
            itemName = value;
        }
    }

    public TextMeshProUGUI ItemCount {
        get {
            return itemCount;
        } set {
            itemCount = value;
        }
    }
}
