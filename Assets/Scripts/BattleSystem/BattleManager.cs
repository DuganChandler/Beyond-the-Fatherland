using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public PartyList PlayerPartyList { get; set; }
    public BattleState battleState { get; set; }

    public void StartBattle() {
        ForestObjects.SetActive(false);
        SceneHelper.LoadScene("ForestBattleScene", true, true);
    }

    public void EndBattle() {
        SceneHelper.UnloadScene("ForestBattleScene");
        CallAfterDelay.Create(1.0f, () => {
            ForestObjects.SetActive(true);
        });
    }
}
