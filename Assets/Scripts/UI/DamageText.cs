using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;

    public TextMeshProUGUI Text { get => text; set => text = value; }

    public void OnAnimationEnd() {
        Debug.Log("BROOOOlla");
        Destroy(gameObject);
    }

    // void LateUpdate() {
    //     transform.LookAt(Camera.main.transform);
    //     transform.RotateAround(transform.position, transform.up, 180f);
    // }
}
