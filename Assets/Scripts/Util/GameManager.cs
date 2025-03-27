using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    FreeRoam,
    Battle,
    Dialog,
    Busy
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
