using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable {
    [SerializeField] Potion item;

    public bool Used { get; set; } = false;

    public IEnumerator Interact(Transform initiator) {
        if (!Used) {
            initiator.GetComponent<Inventory>().AddItem(item);

            Used = true;

            yield return null;
        }
    }
}
