using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Références")]
    public Transform player; // Ton joueur à suivre

    [Header("Paramètres caméra")]
    public float mouseSensitivity = 100f;
    public float distanceFromPlayer = 5f;
    public float verticalRotationLimit = 80f;
    public float rotationSmoothTime = 0.1f;

    private InputActionAsset inputAsset;
    private InputAction lookAction;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    private void Awake()
    {
        // Charge ton InputAction existant
        inputAsset = Resources.Load<InputActionAsset>("Input/InputAction");

        if (inputAsset == null)
        {
            Debug.LogError("❌ Impossible de trouver Input/InputAction.inputactions dans Resources !");
            return;
        }

        // Récupère l'action "Look" (dans ton InputAction)
        lookAction = inputAsset.FindAction("Look");

        if (lookAction == null)
        {
            Debug.LogError("❌ L'action 'Look' n'existe pas dans InputAction.inputactions !");
        }
    }

    private void OnEnable()
    {
        lookAction?.Enable();
    }

    private void OnDisable()
    {
        lookAction?.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (lookAction == null) return;

        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalRotationLimit, verticalRotationLimit);

        Vector3 targetRotation = new Vector3(xRotation, yRotation);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

        Quaternion rotation = Quaternion.Euler(currentRotation);

        transform.position = player.position - rotation * Vector3.forward * distanceFromPlayer;
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
