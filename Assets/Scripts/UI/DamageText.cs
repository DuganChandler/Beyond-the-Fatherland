using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public TextMeshPro text;
    // Update is called once per frame
    void Update() {    
    }

    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.RotateAround(transform.position, transform.up, 180f);
    }
}
