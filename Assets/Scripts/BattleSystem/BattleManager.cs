using System.Collections.Generic;
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

    public void StartBattle() {
        foreach (var character in PlayerPartyList) {
            character.IncreaseHP(1000);
            character.IsAlive = true;
        }

        MusicManager.Instance.PlayMusic("BattleTheme", 0.25f);
        ForestObjects.SetActive(false);
        SceneHelper.LoadScene("ForestBattleScene", true, true);
    }

    public void EndBattle() {
        MusicManager.Instance.StopMusic();
        SceneHelper.UnloadScene("ForestBattleScene");
        CallAfterDelay.Create(1.0f, () => {
            ForestObjects.SetActive(true);
        });
    }
}
