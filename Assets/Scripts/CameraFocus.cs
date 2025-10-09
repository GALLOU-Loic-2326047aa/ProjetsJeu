using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] private Transform target; // le joueur
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float rotationSpeed = 150f;

    private float yaw;
    private float pitch = 15f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // bloque le curseur au centre de l’écran
        Cursor.visible = false;                    // rend le curseur invisible
    }


    void LateUpdate()
    {
        if (!target) return;

        // Rotation uniquement avec clic droit
        
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, 5f, 60f);
        

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset + Vector3.up * height;

        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }
}
