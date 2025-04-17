using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattlePosition : MonoBehaviour {
    [SerializeField] private GameObject position;
    public bool Occupied { get; set; } = false; 

    public GameObject Position { get => position; set => position = value; } 
}
