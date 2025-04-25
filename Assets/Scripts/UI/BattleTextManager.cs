using UnityEngine;

public class BattleTextManager : MonoBehaviour {
    [SerializeField] private DamageText textPrefab;
    [SerializeField] private Canvas canvas;

    public void CreateBattleText(Transform target, string text, Color color) {
        DamageText instance = Instantiate(textPrefab, canvas.transform);

        instance.Text.text = text;
        instance.Text.color = color;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
              ? null
              : canvas.worldCamera,
            out Vector2 localPoint
        );

        RectTransform rt = instance.GetComponent<RectTransform>();
        rt.anchoredPosition = localPoint;
    }
}
