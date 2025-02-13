using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float walkSpeed = 10f;

    Vector2 moveInput;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        
    }

    void Update() {
        
    }

    void FixedUpdate() {
       Run(); 
    }

    void Run() {
        Vector3 velocity = new Vector3(moveInput.x * walkSpeed, rb.velocity.y, moveInput.y * walkSpeed);
        rb.velocity = transform.TransformDirection(velocity);
    }

    // Invoked by unity event via the Unity Input System
    public void onMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }
    
}
