using UnityEngine;
using UnityEngine.UI;      // for Color
using UnityEngine.EventSystems; // for RectTransformUtility

public class BattleTextManager : MonoBehaviour {
    [SerializeField] private DamageText textPrefab; // change name for clarity
    [SerializeField] private Canvas canvas;

    public void CreateBattleText(Transform target, string text, Color color) {
        // 1. Instantiate the DamageText itself, not just its RectTransform
        DamageText instance = Instantiate(textPrefab, canvas.transform);

        // 2. Set its text + color on the instance
        instance.Text.text = text;
        instance.Text.color = color;

        // 3. Figure out where on the canvas we should stick it:
        //    Convert world → screen point
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        //    Convert screen → local position in our canvas RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
              ? null
              : canvas.worldCamera,
            out Vector2 localPoint
        );

        // 4. Assign to the spawned RectTransform's anchoredPosition
        RectTransform rt = instance.GetComponent<RectTransform>();
        rt.anchoredPosition = localPoint;
    }
}
