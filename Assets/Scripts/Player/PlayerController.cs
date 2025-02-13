using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float walkSpeed = 10f;

    [Header("Encounter Settings")]
    [SerializeField] private LayerMask encounterLayer;
    [SerializeField] private float encounterChance = 0.3f;
    [SerializeField] private float distanceThreshhold = 10f;

    private Vector3 lastPosition;
    private float distanceAccumulated = 0f;

    private Vector2 moveInput;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;



    void Start() {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>(); 

        lastPosition = transform.position;
    }

    void Update() {
        Vector3 currentPos = transform.position;
        float distanceMoved = Vector3.Distance(currentPos, lastPosition);
        distanceAccumulated += distanceMoved;
        lastPosition = currentPos;

        if (IsEncounterLayer()) {
            if (distanceAccumulated >= distanceThreshhold) {
                if (Random.value <= encounterChance) {
                    Debug.Log("RANDOM ENCOUNTER");
                }
                distanceAccumulated = 0f;
            }
        } else {
            distanceAccumulated = 0f;
        } 
    }

    void FixedUpdate() {
       Run(); 
    }

    private bool IsEncounterLayer() {
        RaycastHit hit;

        Debug.DrawRay(capsuleCollider.bounds.center, Vector3.down * 1f, Color.red);
        if (Physics.Raycast(capsuleCollider.bounds.center, Vector3.down, out hit, 1f, encounterLayer)) {
            return true;
        }
        return false;
    }

    private void CheckRandomEncounter() {
        if (!IsEncounterLayer()) {
            return;
        }

    }

    void Run() {
        Vector3 velocity = new Vector3(moveInput.x * walkSpeed, rb.velocity.y, moveInput.y * walkSpeed);
        rb.velocity = transform.TransformDirection(velocity);
    }

    // Invoked by unity event via the Unity Input System
    public void onMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    void OGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 0.1f);
    }

}
