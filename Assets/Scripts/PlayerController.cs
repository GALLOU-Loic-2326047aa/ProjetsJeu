using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private float movementX;
    private float movementY;

    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Saut")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundMask;

    private bool isGrounded;

    public event Action OnJumpButtonPressed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Debug.Log("✅ PlayerController initialisé !");
    }

    private void FixedUpdate()
    {

        if (isDashing) return; // pas de contrôle pendant le dash

        // Détection du sol (raycast depuis le centre)
        float rayDistance = GetComponent<Collider>().bounds.extents.y + 0.1f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask);

        Debug.DrawRay(transform.position, Vector3.down * rayDistance, isGrounded ? Color.green : Color.red);

        // Déplacement horizontal
        Vector3 horizontalMove = new Vector3(movementX, 0.0f, movementY) * moveSpeed;
        Vector3 velocity = new Vector3(horizontalMove.x, rb.velocity.y, horizontalMove.z);
        rb.velocity = velocity;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            Debug.Log($"Jump input reçu | isGrounded = {isGrounded}");
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                OnJumpButtonPressed?.Invoke();
            }
        }
    }

    void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool isDashing = false;

    private IEnumerator DashCoroutine()
    {
        if (isDashing) yield break; // empêche plusieurs dashs simultanés
        if (movementX == 0 && movementY == 0) yield break; // pas de dash si immobile

        isDashing = true;

        // Direction du dash
        Vector3 dashDirection = new Vector3(movementX, 0, movementY).normalized;

        // Supprime la vitesse actuelle et applique un coup de boost
        rb.velocity = Vector3.zero;
        rb.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);

        Debug.Log("💨 Dash lancé !");

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        Debug.Log("🏁 Dash terminé !");
    }


}
