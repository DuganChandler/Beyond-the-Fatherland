using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogBox : MonoBehaviour {
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject dialogBox;
    [SerializeField] int lettersPerSecond;

    public IEnumerator TypeDialog(string dialog) {
        dialogText.text = "";
        dialogBox.SetActive(true);

        foreach (var letter in dialog.ToCharArray()) {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
        dialogBox.SetActive(false);
    }
}
