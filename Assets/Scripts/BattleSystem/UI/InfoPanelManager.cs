using TMPro;
using UnityEngine;

public class InfoPanelManager : MonoBehaviour {
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI infoText; 

    public GameObject Panel { get; set; } 
    public TextMeshProUGUI InfoText { get; set; } 

    public void SetText(string text, bool setActive) {
        panel.SetActive(setActive);
        infoText.text = text;
    }
}