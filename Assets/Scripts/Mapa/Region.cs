using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Region : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "OasisScene"; // <- pon aquí el nombre de tu escena

    void Update()
    {
        // Detecta clic izquierdo del mouse
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Lanza un raycast desde la cámara hacia el cursor
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Verifica si el objeto clickeado es este marcador
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Clic en " + gameObject.name + " → cargando escena: " + sceneToLoad);
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
        }
    }
}
