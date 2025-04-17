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

    void Update() {
        if (GameManager.Instance.GameState == GameState.Battle) {
            itemName.color = Color.white;
            itemCount.color = Color.white;
        } else {
            itemName.color = Color.black;
            itemCount.color = Color.black;
        }
    }
}
