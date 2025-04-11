using TMPro;
using UnityEngine;

public class InfoPanelManager : MonoBehaviour {
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI infoText; 

    public GameObject Panel { get; set; } 
    public TextMeshProUGUI InfoText { get; set; } 
}