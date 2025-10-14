using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private float movementX;
    private float movementY;

    private float respawnHeightOffset = 1f; // combien au-dessus de l'objet respawn

    [Header("Respawn")]
    [SerializeField] private Transform respawnObject; // objet à assigner dans l'inspecteur
    [SerializeField] private float respawnDelay = 2f; // délai avant respawn en secondes

    [Header("Références")]
    [SerializeField] private Transform cameraTransform;

    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Saut")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundMask;

    [Header("Dash")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;

    private bool isDashing = false;
    private bool isGrounded;

    private int clefs = 0;

    public event Action OnJumpButtonPressed;
    
    [Header("Collectibles")]
    [Tooltip("Prefab de l'épée tournante à instancier lorsque le joueur collecte une Epee")]
    [SerializeField] private GameObject spinningSwordPrefab;
    [Tooltip("Hauteur d'apparition relative au joueur pour l'épée tournante")]
    [SerializeField] private float swordSpawnHeight = 1f;
    private GameObject activeSpinningSword;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        count = 0;

        if (winTextObject != null)
            winTextObject.SetActive(false);

        SetCountText();
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        // Détection du sol
        float rayDistance = GetComponent<Collider>().bounds.extents.y + 0.1f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask);

        // Déplacement relatif à la caméra
        Vector3 inputDir = new Vector3(movementX, 0f, movementY).normalized;
        Vector3 moveDir = cameraTransform.forward * inputDir.z + cameraTransform.right * inputDir.x;
        moveDir.y = 0f;

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;

        // Rotation du joueur vers sa direction de mouvement
        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 input = movementValue.Get<Vector2>();
        movementX = input.x;
        movementY = input.y;
    }

    void OnJump(InputValue inputValue)
    {
        if (inputValue.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            OnJumpButtonPressed?.Invoke();
        }
    }

    void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        if (isDashing) yield break;
        if (movementX == 0 && movementY == 0) yield break;

        isDashing = true;

        Vector3 dashDir = (cameraTransform.forward * movementY + cameraTransform.right * movementX).normalized;
        dashDir.y = 0f;

        rb.velocity = Vector3.zero;
        rb.AddForce(dashDir * dashForce, ForceMode.VelocityChange);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    public void HandleCollectible(CollectibleData data)
    {
        switch (data.type)
        {
            case CollectibleType.PickUp:
                count += data.value;
                SetCountText();
                if (count >= 3 && winTextObject != null)
                    winTextObject.SetActive(true);
                break;
            case CollectibleType.Clef:
                clefs += data.value;
                foreach (var t in FindObjectsOfType<Transform>())
                {
                    if (t.name == "Couvercle")
                        Destroy(t.gameObject);
                }
                break;
            case CollectibleType.Trophee:
                winTextObject.SetActive(true);
                break;
            case CollectibleType.Epee:
                activeSpinningSword = Instantiate(spinningSwordPrefab, transform.position + Vector3.up * swordSpawnHeight, Quaternion.identity);
                if (activeSpinningSword.TryGetComponent<SpinningSword>(out var spinningSword))
                {
                    spinningSword.SetPlayer(transform);
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(RespawnCoroutine());
        }
    }


    void SetCountText()
    {
        if (countText != null)
            countText.text = "Count: " + count;
    }

    private IEnumerator RespawnCoroutine()
    {

        Vector3 spawnPosition = respawnObject.position + Vector3.up * respawnHeightOffset;
        transform.position = spawnPosition;
        rb.velocity = Vector3.zero;
        isDashing = false;

        yield return null; // nécessaire pour coroutine
    }


}
