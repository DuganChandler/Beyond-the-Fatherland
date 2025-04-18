using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState {
    FreeRoam,
    Battle,
    Dialog,
    Pause,
    Busy,
    MainMenu,
    GameOver,
    Victory
}

public class GameManager : MonoBehaviour {
    private static GameManager _Instance;
    public static GameManager Instance { 
        get { 
            if (!_Instance) {
                _Instance = new GameObject().AddComponent<GameManager>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        } 
    }

    public GameState GameState { get; set; } = GameState.FreeRoam;
    public bool inDialog { get; set; } = false;
    public bool FirstBattle { get; set; } = true;
}
