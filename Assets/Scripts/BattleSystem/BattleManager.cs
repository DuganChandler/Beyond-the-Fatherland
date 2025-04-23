using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleType {
    Random,
    Boss,
}

public class BattleManager : MonoBehaviour {
    private static BattleManager _Instance;
    PlayerController _playerController;

    public static BattleManager Instance { 
        get { 
            if (!_Instance) {
                _Instance = new GameObject().AddComponent<BattleManager>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        } 
    }

    private GameObject _ForestObjects;
    public GameObject ForestObjects {
        get {
            if (_ForestObjects== null) {
                _ForestObjects= GameObject.Find("ArborynForestObjects");
            }
            return _ForestObjects;
        }
    }

    public List<Character> PlayerPartyList { get; set; }
    public List<Character> EncounterPartyList { get; set; }
    public Inventory PlayerInventory { get; set; }
    public BattleState BattleState { get; set; }
    public BattleType BattleType { get; set; }

    public IEnumerator StartBattle() {
        GameManager.Instance.GameState = GameState.Battle;

        yield return StartCoroutine(SceneHelper.LoadSceneWithTransition("ForestBattleScene", true, true, null, () => {
            if (MusicManager.Instance) {
                MusicManager.Instance.PauseMusic();

            }

            CircleFadeTransition circleFade = FindObjectOfType<CircleFadeTransition>();
            if (circleFade != null) {
                circleFade.onTransitionComplete = () => {
                    SceneHelper.AllowActivation();
                    circleFade.Cleanup();
                    ForestObjects.SetActive(false);
                };
                StartCoroutine(circleFade.TriggerTransition());
            }
        }));

        yield return null;
    }

    public void EndBattle(bool won) {
        if (!won) {
            if (MusicManager.Instance != null) {
                MusicManager.Instance.PlayMusicNoFade("TitleTheme"); 
            }

            SceneHelper.LoadScene("GameOver", false, true);
            GameManager.Instance.GameState = GameState.GameOver;
            MusicManager.Instance.StopMusic();
            return;
        }

        if (BattleType == BattleType.Boss) {
            GameManager.Instance.GameState = GameState.Victory;

            if (MusicManager.Instance != null) {
                MusicManager.Instance.StopMusic();
            }

            SceneHelper.LoadScene("Victory", false, true);
            return;
        }

        GameManager.Instance.GameState = GameState.FreeRoam;
        MusicManager.Instance.StopMusic();

        MusicManager.Instance.PlayMusicNoFade("ForestTheme"); 

        foreach (var character in PlayerPartyList) {
            if (!character.IsAlive) {
                character.IncreaseHP(1);
                character.IsAlive = true;
            }
        }

        SceneHelper.UnloadScene("ForestBattleScene");
        CallAfterDelay.Create(1.0f, () => {
            ForestObjects.SetActive(true);
        });

        Debug.Log("now in grace period");
        _playerController.StartGracePeriod(); 
    }
    

}
