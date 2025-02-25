using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    float elapsedTime = 0f;
    bool battleEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 5f && !battleEnded) {
            battleEnded = true;
            BattleManager.Instance.EndBattle();
        }
    }

    public void EndBattle() {
        BattleManager.Instance.EndBattle();
    }
}
