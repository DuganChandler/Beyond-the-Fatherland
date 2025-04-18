using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(Animator))]
[RequireComponent(typeof(PartyList), typeof(Inventory))]
public class PlayerController : MonoBehaviour { 
    [SerializeField] private float walkSpeed = 10f;

    [Header("Encounter Settings")]
    [SerializeField] private LayerMask encounterLayer;
    [SerializeField] private float encounterChance = 0.3f;
    [SerializeField] private float distanceThreshhold = 10f;
    [SerializeField] private GameObject sceneObjects;

    [Header("Interactable Settings")]
    [SerializeField] private LayerMask interactableLayer; 

    [SerializeField] private GameObject bagMenuUI;

    private Vector3 lastPosition;
    private float distanceAccumulated = 0f;

    private Animator animator;

    private Vector3 moveInput;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    public static event System.Action OnDialogContinue;

    private bool _isMoving = false;
    public bool IsMoving {
        get {
            return _isMoving;
        } private set {
            _isMoving = value;
            animator.SetBool("Moving", value);
        }
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>(); 
        animator = GetComponent<Animator>();

        lastPosition = transform.position;
    }

    void Update() {
        CalculateDistanceTraveled();
        CheckRandomEncounter();
    }

    void FixedUpdate() {
        if (GameManager.Instance.GameState == GameState.FreeRoam) {
            Rotate();
            Run();
        } else {
            rb.velocity = Vector3.zero;
        }
    }

    private (bool, GameObject) IsEncounterLayer() {

        Debug.DrawRay(capsuleCollider.bounds.center, Vector3.down * 1.5f, Color.red);
        if (Physics.Raycast(capsuleCollider.bounds.center, Vector3.down, out RaycastHit hit, 1.5f, encounterLayer)) {
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
        if (GameManager.Instance.GameState != GameState.FreeRoam) {
            Debug.Log("Need to be in Free Roam to enter Battle");
            return;
        }

        var encounterLayer = IsEncounterLayer();
        Debug.Log(encounterLayer);
        if (!encounterLayer.Item1) {
            distanceAccumulated = 0f;
            return;
        }

        if (distanceAccumulated >= distanceThreshhold) {
            if (Random.value <= encounterChance) {
                PartyList partyList = GetComponent<PartyList>();

                BattleManager.Instance.EncounterPartyList = encounterLayer.Item2.GetComponent<EncounterMapArea>().GetRandomEncounter(partyList.CalculateAveragePartyLevel());
                BattleManager.Instance.PlayerPartyList = partyList.CharacterList;
                BattleManager.Instance.PlayerInventory = GetComponent<Inventory>();
                BattleManager.Instance.BattleType = BattleType.Random;

                StartCoroutine(BattleManager.Instance.StartBattle());
            }
            distanceAccumulated = 0f;
        }
    }

    void Run() {
        if (GameManager.Instance.inDialog || GameManager.Instance.GameState == GameState.Battle) {
            rb.velocity = Vector3.zero;
            return;
        }
        Quaternion rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 rotatedMoveInput = rotation * moveInput;
        Vector3 velocity = new Vector3(rotatedMoveInput.x * walkSpeed, rb.velocity.y, rotatedMoveInput.z * walkSpeed);
        rb.velocity = velocity;
    }

    void Rotate() {
         if (moveInput.sqrMagnitude > 0.001f) {
            Quaternion cameraYaw = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            Vector3 rotatedInput = cameraYaw * moveInput;
            Quaternion targetRotation = Quaternion.LookRotation(rotatedInput, Vector3.up);
            transform.rotation = targetRotation; //Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    IEnumerator Interact() {
        var facingDirection = transform.forward;
        var InteractPos = transform.position + facingDirection;

        var collider = Physics.OverlapSphere(InteractPos, 0.3f, interactableLayer);
        if (collider.Length > 0) {
            Debug.Log("Interacted");
            yield return collider[0].GetComponent<IInteractable>()?.Interact(transform);
        }
    }

    // Invoked by unity event via the Unity Input System
    public void OnMove(InputAction.CallbackContext context) {
        if (GameManager.Instance.inDialog) {
            moveInput = Vector3.zero;
            return;
        }
        moveInput = context.ReadValue<Vector2>();
        moveInput = new Vector3(moveInput.x, 0, moveInput.y);
        if (moveInput.x != 0 || moveInput.y != 0) {
            IsMoving = true;
        } else {
            IsMoving = false;
        }
    }

    // need to check if in free roam
    public void OnInteract(InputAction.CallbackContext context) {
        if (context.started && GameManager.Instance.GameState == GameState.FreeRoam) {
            StartCoroutine(Interact());
        }
    }

    public void OnDialog(InputAction.CallbackContext context) {
        if (context.started) {
            Debug.Log("Dialog Selected");
            OnDialogContinue?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context) {
        if (context.started) {
            if (GameManager.Instance.GameState != GameState.Pause && GameManager.Instance.GameState == GameState.FreeRoam) {
                bagMenuUI.SetActive(true);
                GameManager.Instance.GameState = GameState.Pause;
            } else {
                bagMenuUI.SetActive(false);
                GameManager.Instance.GameState = GameState.FreeRoam;
            }
        }
    }

}
