using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryCharacterInfo : MonoBehaviour {
    [Header("Character Info UI Elements")]
    [SerializeField] private Image characterPorait; 
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterLevel;
    [SerializeField] private TextMeshProUGUI characterEXP;
    [SerializeField] private GameObject characterArrow;

    public Image CharacterPortrait { get => characterPorait; }
    public TextMeshProUGUI CharacterName { get => characterName; }
    public TextMeshProUGUI CharacterLevel { get => characterLevel; }
    public TextMeshProUGUI CharacterEXP { get => characterEXP; }
    public GameObject CharacterArrow { get => characterArrow; }
}
