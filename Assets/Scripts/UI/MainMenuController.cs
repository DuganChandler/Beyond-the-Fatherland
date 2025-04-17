using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
    [SerializeField] Button startButton;

    void Awake() {
        startButton.Select(); 
    }

    void Start() {
        if (MusicManager.Instance != null) {
            MusicManager.Instance.PlayMusicNoFade("TitleTheme"); 
        }
    }

    public void OnQuitGame() {
        Application.Quit();
    }

    public void OnStartGame() {
        MusicManager.Instance.PlayMusicNoFade("ForestTheme"); 
        SceneHelper.LoadScene("ArborynForestV2", false, true);
    }

    public void OnMainMenu() {
        if (MusicManager.Instance != null) {
            MusicManager.Instance.PlayMusicNoFade("TitleTheme"); 
        }

        SceneHelper.LoadScene("MainMenu", false, true);
    }
}
