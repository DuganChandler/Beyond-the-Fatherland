using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[System.Serializable]
public struct DialogLine {
    public string Name;
    public string Text;
    public Sprite portrait;
} 

[System.Serializable]
public class Dialog {
    [SerializeField] List<DialogLine> dialogLines;

    public List<DialogLine> DialogLines {
        get {
            return dialogLines;
        }
    }
}
