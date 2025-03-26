using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHelper {
	private static AsyncOperation currentAsyncOperation;

	public static void LoadScene(string str, bool additive = false, bool setActive = false) {
		str ??= SceneManager.GetActiveScene ().name;

		SceneManager.LoadScene(
			str, additive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : 0);

		if (setActive) {
            // to mark it active we have to wait a frame for it to load.
			CallAfterDelay.Create(1.0f, () => {
				SceneManager.SetActiveScene(
					SceneManager.GetSceneByName(str));
			});
		}
	}

	 public static IEnumerator LoadSceneWithTransition(string str, bool additive = false, bool setActive = false, GameObject objectsToDisable = null, System.Action onLoaded = null) {
        str ??= SceneManager.GetActiveScene().name;
        
        // Start loading the scene asynchronously.
        AsyncOperation op = SceneManager.LoadSceneAsync(str, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        // Prevent automatic activation.
        op.allowSceneActivation = false;
        
        // Store the operation so it can be activated later.
        currentAsyncOperation = op;
        
        // Wait until the scene is almost loaded.
        while (op.progress < 0.9f) {
            yield return null;
        }
        
        // Invoke callback to notify that the scene is ready (but not yet active).
        onLoaded?.Invoke();
        
        // Now wait until the caller (via SignalSceneActivation or AllowActivation) sets allowSceneActivation to true.
        while (!op.allowSceneActivation) {
            yield return null;
        }
        
        // Wait until the scene activation is complete.
        while (!op.isDone) {
            yield return null;
        }
        
        // Clear the stored operation.
        currentAsyncOperation = null;
        
        // Optionally, set the new scene as active and disable the specified objects.
        if (setActive) {
			if (objectsToDisable != null)
				objectsToDisable.SetActive(false);
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(str));
        }
    }

	public static void UnloadScene(string str) {
		SceneManager.UnloadSceneAsync(str);
	}

	    /// <summary>
    /// Call this method from your transition (e.g. in your fade transition callback) to allow the scene to activate.
    /// </summary>
    public static void SignalSceneActivation() {
        if (currentAsyncOperation != null) {
            currentAsyncOperation.allowSceneActivation = true;
            currentAsyncOperation = null;
        }
    }

    /// <summary>
    /// This is an alias for SignalSceneActivation.
    /// </summary>
    public static void AllowActivation() {
        SignalSceneActivation();
    }
}