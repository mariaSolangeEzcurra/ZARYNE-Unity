using UnityEngine;
using UnityEngine.InputSystem; // Usar el nuevo Input System

public class GloboController : MonoBehaviour
{
    public float rotationSpeed = 200f; // Ajusta la velocidad de rotación
    private bool isDragging = false;
    private Vector2 lastMousePosition;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isDragging = true;
            lastMousePosition = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 delta = mousePosition - lastMousePosition;

            // Rotación: mover en X rota alrededor del eje Y (izq/der)
            // mover en Y rota alrededor del eje X (arriba/abajo)
            float rotX = delta.y * rotationSpeed * Time.deltaTime;
            float rotY = -delta.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.right, rotX, Space.World);
            transform.Rotate(Vector3.up, rotY, Space.World);

            lastMousePosition = mousePosition;
        }
    }
}
