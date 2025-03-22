using System.Collections.Generic;
using UnityEngine;

public class PointerManager : MonoBehaviour {
    [SerializeField] private RectTransform pointer;
    [SerializeField] private Canvas canvas; 

    private List<RectTransform> activePointers = new List<RectTransform>();
    private Vector3 worldOffset = new(-1f, 1f, 0);

    public void TargetMultiple(List<Transform> targets) {
        ClearPointers();

        foreach (Transform target in targets) {
            RectTransform newPointer = Instantiate(pointer, canvas.transform);
            activePointers.Add(newPointer);
        }
        
        UpdateMultiplePointers(targets);
    }

    public void UpdateMultiplePointers(List<Transform> targetTransforms) {
        for (int i = 0; i < targetTransforms.Count; i++) {
            if (i < activePointers.Count) {
                // Convert the target's world position to screen position
                Vector3 screenPos = Camera.main.WorldToScreenPoint(targetTransforms[i].position + worldOffset);
                activePointers[i].position = screenPos;
            }
        }
    }

    public void TargetSingle(Transform target) {
        ClearPointers();
        RectTransform newPointer = Instantiate(pointer, canvas.transform);
        activePointers.Add(newPointer);
        UpdateSinglePointer(target);
    }

    public void UpdateSinglePointer(Transform target) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + worldOffset);
        activePointers[0].position = screenPos;
    }

    public void ClearPointers() {
        foreach (RectTransform pointer in activePointers) {
            Destroy(pointer.gameObject);
        }
        activePointers.Clear();
    }
}
