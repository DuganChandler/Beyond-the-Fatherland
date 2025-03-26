using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {
    [SerializeField] Dialog dialog;

    void OnTriggerEnter(Collider other) {
        Debug.Log(other);
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Collided With Player");
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
        } 
    }
}
