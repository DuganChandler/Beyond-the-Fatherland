using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _Instance;
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
                _ForestObjects= GameObject.Find("ChandlerTestSceneObjects");
            }
            return _ForestObjects;
        }
    }

    public List<Character> PlayerPartyList { get; set; }
    public List<Character> EncounterPartyList { get; set; }
    public BattleState BattleState { get; set; }

    public IEnumerator StartBattle() {
        foreach (var character in PlayerPartyList) {
            character.IncreaseHP(1000);
            character.IsAlive = true;
        }
        GameManager.Instance.GameState = GameState.Battle;

        yield return StartCoroutine(SceneHelper.LoadSceneWithTransition("ForestBattleScene", true, true, null, () => {
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

    public void EndBattle() {
        GameManager.Instance.GameState = GameState.FreeRoam;
        MusicManager.Instance.StopMusic();
        SceneHelper.UnloadScene("ForestBattleScene");
        CallAfterDelay.Create(1.0f, () => {
            ForestObjects.SetActive(true);
        });
    }

}
