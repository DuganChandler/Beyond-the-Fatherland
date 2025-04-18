using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointerManager : MonoBehaviour {
    [SerializeField] private RectTransform pointerPrefab;
    [SerializeField] private Canvas canvas;
    
    // Optional offset for positioning relative to the selected element
    [SerializeField] private Vector2 pointerOffset = new Vector2(20, 0);
    
    private RectTransform activePointer;
    private GameObject lastSelected;

    public GameObject LastSelected {
        get {
            return lastSelected;
        } set {
            lastSelected = value;
        }
    }

    void Update() {
        GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
        if (selectedObj != null && selectedObj != lastSelected) {
            if (selectedObj.TryGetComponent<RectTransform>(out var selectedRect)) {
                // If there's no pointer yet, create one.
                if (activePointer == null) {
                    activePointer = Instantiate(pointerPrefab, canvas.transform);
                }

                activePointer.position = selectedRect.Find("PointerPosition").transform.position;

                if (lastSelected != null) {
                    if (MusicManager.Instance != null) {
                        MusicManager.Instance.PlaySound("MenuScroll");
                    }
                }

                lastSelected = selectedObj;
            }
        } else if (selectedObj == null) {
            // Optionally clear the pointer if nothing is selected
            if (activePointer != null) {
                Destroy(activePointer.gameObject);
                activePointer = null;
                lastSelected = null;
            }
        }
    }
}
