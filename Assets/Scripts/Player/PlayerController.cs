using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private Vector3 moveInput;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    public static event System.Action OnDialogContinue;

    void Start() {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>(); 

        lastPosition = transform.position;
    }

    void Update() {
        // CalculateDistanceTraveled();
        // CheckRandomEncounter();
        // if (Input.GetKeyDown(KeyCode.B)) {
        //     CircleFadeTransition shatterEffect = FindObjectOfType<CircleFadeTransition>();
        //     if (shatterEffect != null) {
        //         // Start the shatter effect and load the battle scene in its callback.
        //         StartCoroutine(shatterEffect.TriggerTransition());
        //         shatterEffect.onTransitionComplete = () => {
        //             // Load the battle scene additively once the shatter is done.
        //             BattleManager.Instance.StartBattle();
        //             // SceneHelper.LoadScene("ForestBattleScene", true, true);
        //         };
        //     }
        // }
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
                BattleManager.Instance.PlayerInventory = GetComponent<Inventory>();
                Debug.Log(GetComponent<PartyList>().CharacterList);
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
