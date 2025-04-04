using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CircleFadeTransition : MonoBehaviour
{
    public GameObject transitionObject;
    public RawImage rawImage;         // Assign your full-screen RawImage.
    public Material circleFadeMaterial; // Material using the Custom/CircleFade shader.
    public float transitionDuration = 1f; // How long the fade takes.
    public Action onTransitionComplete;   // Optional callback once done.

    private Texture2D screenTexture;
    private Texture2D newScreenTexture;
    
    void StartFade() {
        // Optionally, you can start the transition automatically.
        // Otherwise, call TriggerTransition() from your manager script.
        StartCoroutine(TriggerTransition());
    }
    
    public IEnumerator TriggerTransition() {
        // Wait until the end of the frame so the screen is fully rendered.
        yield return new WaitForEndOfFrame();
        
        // Capture the current screen.
        screenTexture = ScreenCapture.CaptureScreenshotAsTexture();

        newScreenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        newScreenTexture.SetPixels(screenTexture.GetPixels());
        newScreenTexture.Apply();
        
        // Set the captured texture as the main texture of the material.
        circleFadeMaterial.SetTexture("_MainTex", newScreenTexture);
        // Assign the material to the RawImage.
        rawImage.material = circleFadeMaterial;
        transitionObject.SetActive(true);
        
        // Animate _CircleRadius from 1 (full image) to 0 (only a tiny circle visible).
        float time = 0f;
        while (time < transitionDuration)
        {
            float t = time / transitionDuration;
            // Lerp from 1 to 0 over time.
            float currentRadius = Mathf.Lerp(1f, 0f, t);
            circleFadeMaterial.SetFloat("_CircleRadius", currentRadius);
            time += Time.deltaTime;
            yield return null;
        }
        // Ensure it’s fully faded.
        circleFadeMaterial.SetFloat("_CircleRadius", 0f);

        yield return new WaitForSeconds(0.5f);
        // Optionally, invoke a callback (e.g., to load another scene).
        onTransitionComplete?.Invoke();
        
    }

    public void Cleanup() {
        transitionObject.SetActive(false);
        Destroy(screenTexture);
        Destroy(newScreenTexture);
    }
}
