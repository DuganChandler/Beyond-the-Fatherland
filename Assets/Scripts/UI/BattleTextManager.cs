using System.Collections;
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

        StartCoroutine(FloatAndFade(instance, rt, localPoint));
    }

    private IEnumerator FloatAndFade(DamageText inst, RectTransform rt, Vector2 startPos) {
        float duration = 1f;
        float elapsed = 0f;
        Color startColor = inst.Text.color;
        Vector2 endPos = startPos + Vector2.up * 30f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            rt.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            inst.Text.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0f), t);

            yield return null;
        }

        Destroy(inst.gameObject);
    }
}
