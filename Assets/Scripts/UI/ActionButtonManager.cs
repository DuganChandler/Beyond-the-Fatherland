using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionButtonManager : MonoBehaviour {
    [SerializeField] private Image northImage;
    [SerializeField] private Image southImage;
    [SerializeField] private Image eastImage;
    [SerializeField] private Image westImage;

    [SerializeField] private TextMeshProUGUI northText;
    [SerializeField] private TextMeshProUGUI southText;
    [SerializeField] private TextMeshProUGUI eastText;
    [SerializeField] private TextMeshProUGUI westText;

    public TextMeshProUGUI NorthText { get; set; }
    public TextMeshProUGUI SouthText { get; set; }
    public TextMeshProUGUI EastText { get; set; }
    public TextMeshProUGUI WestText { get; set; }

    public void SetButtonText(string north, string south, string east, string west) {
        northText.text = north;
        southText.text = south;
        eastText.text = east;
        westText.text = west;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
