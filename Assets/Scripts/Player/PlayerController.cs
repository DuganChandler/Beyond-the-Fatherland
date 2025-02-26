using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float walkSpeed = 10f;

    [Header("Encounter Settings")]
    [SerializeField] private LayerMask encounterLayer;
    [SerializeField] private float encounterChance = 0.3f;
    [SerializeField] private float distanceThreshhold = 10f;
    [SerializeField] private GameObject sceneObjects;

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
        CalculateDistanceTraveled();
        CheckRandomEncounter();
    }

    void FixedUpdate() {
       Run(); 
    }

    private (bool, GameObject) IsEncounterLayer() {
        RaycastHit hit;

        Debug.DrawRay(capsuleCollider.bounds.center, Vector3.down * 1f, Color.red);
        if (Physics.Raycast(capsuleCollider.bounds.center, Vector3.down, out hit, 1f, encounterLayer)) {
            return (true, hit.collider.gameObject);
        }
        return (false, null);
    }

    private void CalculateDistanceTraveled() {
        Vector3 currentPos = transform.position;
        float distanceMoved = Vector3.Distance(currentPos, lastPosition);
        distanceAccumulated += distanceMoved;
        lastPosition = currentPos;
    }

    private void CheckRandomEncounter() {
        var encounterLayer = IsEncounterLayer();
        if (!encounterLayer.Item1) {
            distanceAccumulated = 0f;
            return;
        }

        if (distanceAccumulated >= distanceThreshhold) {
            if (Random.value <= encounterChance) {
                BattleManager.Instance.EncounterPartyList = encounterLayer.Item2.GetComponent<EncounterMapArea>().GetRandomEncounter();
                BattleManager.Instance.PlayerPartyList = GetComponent<PartyList>().CharacterList;
                Debug.Log(GetComponent<PartyList>().CharacterList);
                BattleManager.Instance.StartBattle();
            }
            distanceAccumulated = 0f;
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
}
