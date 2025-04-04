using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    [Header("UI")]
    [SerializeField] GameObject dialogBox;
    [SerializeField] UnityEngine.UI.Image characterPortrait;
    [SerializeField] TextMeshProUGUI speaker;

    [Header("Dialog")]
    [SerializeField] TextMeshProUGUI dialogText;

    [Header("Dialog Speed")]
    [SerializeField] int letterPerSecond;

    public event System.Action OnShowDialog;
    public event System.Action OnDialogFinished;

    public bool IsShowing { get; private set; }

    public static DialogueManager Instance { get; private set; }
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    public IEnumerator ShowDialogText(string text, bool waitForInput = true, bool autoClose = true) {
        GameManager.Instance.inDialog = true;
        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        // AudioManager.i.PlaySfx(AudioID.UISelect);
        yield return TypeDialog(text);
        if (waitForInput) {
            yield return WaitForDialogInput();
        }

        if (autoClose) {
            CloseDialog();
        }

        OnDialogFinished?.Invoke();
    }

    private IEnumerator WaitForDialogInput() {
        bool inputReceived = false;

        System.Action onDialogInput = null;
        onDialogInput = () => {
            inputReceived = true;
            PlayerController.OnDialogContinue -= onDialogInput;
        };

        PlayerController.OnDialogContinue += onDialogInput;

        yield return new WaitUntil(() => inputReceived);
    }

    public void CloseDialog() {
        GameManager.Instance.inDialog = false;
        dialogBox.SetActive(false);
        IsShowing = false;
    }

    public IEnumerator ShowDialog(Dialog dialog) {
        yield return new WaitForEndOfFrame();

        GameManager.Instance.inDialog = true;
        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        foreach (var line in dialog.DialogLines) {
            // AudioManager.i.PlaySfx(AudioID.UISelect);
            characterPortrait.sprite = line.portrait;
            yield return TypeDialog(line.Text);
            yield return WaitForDialogInput();
        }

        CloseDialog();
        OnDialogFinished?.Invoke();
    }

    public IEnumerator TypeDialog(string line) {
        dialogText.text = "";
        bool skip = false;
        void skipTyping() { skip = true; }
        PlayerController.OnDialogContinue += skipTyping;

        foreach (var letter in line.ToCharArray()) {
            if (skip) {
                dialogText.text = line;
                break;
            }

            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
    }
}
