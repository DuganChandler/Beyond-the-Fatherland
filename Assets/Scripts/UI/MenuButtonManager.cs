using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonManager : MonoBehaviour {
    public void PlayerPressedSFX() {
        MusicManager.Instance.PlaySound("MenuConfirm");
    }
}
