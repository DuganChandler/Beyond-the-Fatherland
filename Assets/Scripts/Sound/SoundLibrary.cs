using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundSFX {
    public string soundName;
    public AudioClip clip;
}

public class SoundLibrary : MonoBehaviour {
    public SoundSFX[] sounds;

    public AudioClip GetClipFromName(string soundName) {
        foreach (var sound in sounds) {
            if (sound.soundName == soundName) {
                return sound.clip;
            }
        }

        return null;
    }
}
