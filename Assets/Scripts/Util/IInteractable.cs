using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    IEnumerator Interact(Transform initiator);
}
